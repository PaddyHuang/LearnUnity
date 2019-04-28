using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

[RequireComponent(typeof(Image))]
public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public bool DragOnSurfaces = true;

    private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();
    public string BringStr;

    private Text bringText;
    // Test    
    private AroundCameraMobile aroundCameraMobile;

    void Start()
    {
        // Test       
        aroundCameraMobile = UtilsSearch.FindComponentChild<AroundCameraMobile>(GameObject.FindGameObjectWithTag("EasyAR_Startup").transform, "ObjectCamera");
    }

    public void CreatDragItem(string str)
    {
        bringText = StaticClass.FindInChild<Text>(transform.gameObject);
        if (ReferenceEquals(bringText, null))
            throw new NullReferenceException();
        bringText.text = str;        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = StaticClass.FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        var cd = Instantiate(gameObject);

        cd.transform.SetParent(canvas.transform, false);
        cd.transform.SetAsLastSibling();
        cd.GetComponent<RectTransform>().sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
        //m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
        //m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

        //var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        // The icon will be under the cursor.
        // We want it to be ignored by the event system.
        var group = cd.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;

        m_DraggingIcons[eventData.pointerId] = cd;
        //var text = GetComponentInChildren<Text>();

        BringStr = bringText.text;

        //image.sprite = GetComponent<Image>().sprite;
        //image.SetNativeSize();

        //if (DragOnSurfaces)
        //    m_DraggingPlanes[eventData.pointerId] = transform as RectTransform;
        //else
        //m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;

        SetDraggedPosition(eventData);

        //停止模型控制
        //ObjectControl.Pause();
        // Test
        aroundCameraMobile.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons[eventData.pointerId] != null)
            SetDraggedPosition(eventData);               
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        //if (DragOnSurfaces && eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
        //    m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
        //Vector3 globalMousePos;

        rt.position = eventData.position;

        //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos))
        //{
        //    rt.position = globalMousePos;
        //    //rt.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons[eventData.pointerId] != null)
            Destroy(m_DraggingIcons[eventData.pointerId]);

        m_DraggingIcons[eventData.pointerId] = null;

        //停止模型控制
        //ObjectControl.Continue();
        // Test
        aroundCameraMobile.enabled = true;
    }    
}