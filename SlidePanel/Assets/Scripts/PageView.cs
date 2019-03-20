using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

    private ScrollRect rect;                        //滑动组件  

    // 页面：0，1，2 从0开始
    // 每页占的比例
    private float[] page = { 0, 0.5f, 1 };

    // 滑动速度
    [SerializeField] private float smooth = 4;

    // 滑动起始坐标
    [SerializeField] private float targetHorizontal = 0;

    // 是否拖拽结束
    private bool isDrag = false;

    [SerializeField] private RectTransform toggleList;

    private void Start()
    {
        rect = transform.GetComponent<ScrollRect>();
    }

    private void LateUpdate()
    {
        // 如果不判断，在拖拽的时候也会执行插值，会出现闪烁
        // 在Update中加入判断，只要拖拽结束，就进行插值
        if (!isDrag)
        {
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targetHorizontal, Time.deltaTime * smooth);
        }
    }

    /// <summary>
    /// 拖拽开始
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    /// <summary>
    /// 拖拽结束
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        float posX = rect.horizontalNormalizedPosition;
        // 起始在第一页
        int index = 0;
        float offset = Mathf.Abs(page[index] - posX);

        for (int i = 1; i < page.Length; i++)
        {
            float temp = Mathf.Abs(page[i] - posX);
            if (temp < offset)
            {
                index = i;
                // 保存当前偏移量，如果到最后一页，则反向翻页
                offset = temp;
            }
        }

        targetHorizontal = page[index];
        SetToggle(index);
    }

    private void SetToggle(int index)
    {
        var toggle = toggleList.GetChild(index).GetComponent<Toggle>();
        toggle.isOn = true;
    }

}
