using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UISnapSelect : MonoBehaviour
{
    private event Action<string> OnComfirmHandler;
    public event Action<string> OnComfirm
    {
        add
        {
            OnComfirmHandler -= value;
            OnComfirmHandler += value;
        }
        remove { OnComfirmHandler -= value; }
    }

    public RectTransform ChoosePanel; //将ChoosePanel拖入
    private RectTransform[] elements; //将Content中的若干个元素拖入 
    public RectTransform Center; //在ChoosePanel内新建一个EmptyObject，以centerToCompare命名，并将此拖入
    public RectTransform CreatTemplate;

    public Button ConfirmBtn;
    //public RectTransform Distance1, Distance2;

    public int ElemnetDistance = 80;

    private float lerpTime = 1f;
    private float elapsed;

    // 私有变量
    private int distanceBetweenEles; //相邻两个元素的距离，在Start方法计算
    private float[] distanceToCenter; //每个元素距离center的距离，在Update方法计算
    private int minEleNum; //在所有元素中，距离center最近的元素索引
    private bool dragging; //Element是否在被拖拽；

    private bool isCreated;

    void Start()
    {
        //int eleLength = elements.Length;
        //distanceToCenter = new float[eleLength];

        //Get distance between elements
        //distanceBetweenEles = (int) Mathf.Abs(elements[1].anchoredPosition.y - elements[0].anchoredPosition.y);

        //distanceBetweenEles = (int) Mathf.Abs(Distance2.anchoredPosition.y - Distance1.anchoredPosition.y);

        //Distance1.gameObject.SetActive(false);
        //Distance2.gameObject.SetActive(false);
        distanceBetweenEles = ElemnetDistance;
    }

    public void CreatSnap(string[] sg)
    {
        List<RectTransform> tempL = new List<RectTransform>();
        //数组重排
        string[] randomSg = new string[sg.Length];
        List<string> ran = sg.ToList();
        for (int i = 0; i < sg.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, ran.Count);

            randomSg[i] = ran[index];

            ran.RemoveAt(index);
        }

        for (int i = 0; i < sg.Length; i++)
        {
            RectTransform cg = Instantiate(CreatTemplate, CreatTemplate.transform.parent);
            cg.localPosition = new Vector3(cg.localPosition.x, i * 80, cg.localPosition.z);
            cg.gameObject.SetActive(true);
            Text st = cg.GetComponent<Text>();
            if (ReferenceEquals(st, null))
                continue;

            st.text = randomSg[i];
            tempL.Add(cg);
        }

        elements = tempL.ToArray();

        distanceToCenter = new float[elements.Length];

        isCreated = true;

        ConfirmBtn.onClick.AddListener(ConfirmFunc);
    }

    /// <summary> 
    /// 点击确认以及获取值 </summary>
    private void ConfirmFunc()
    {
        Text tt = elements[minEleNum].GetComponent<Text>();
        string temp = tt.text;

        OnComfirmHandler?.Invoke(temp);
    }

    void Update()
    {
        if (!isCreated)
            return;

        for (int i = 0; i < elements.Length; i++)
            distanceToCenter[i] = Mathf.Abs(Center.transform.position.y - elements[i].transform.position.y); //计算每个元素距离center的距离

        float minDist = Mathf.Min(distanceToCenter);

        for (int i = 0; i < elements.Length; i++)
            if (Math.Abs(minDist - distanceToCenter[i]) < 1f)
                minEleNum = i; //找到最小距离的元素索引

        //如果目前没有滑动
        if (!dragging)
            LerpEleToCenter(minEleNum * -distanceBetweenEles); //LerpEleToCenter作用是自然地滑到目标距离
    }

    private bool isArrive;

    void LerpEleToCenter(int tarY)
    {
        if (isArrive)
            return;

        elapsed += Time.deltaTime;
        float newX = Mathf.Lerp(ChoosePanel.anchoredPosition.y, tarY, elapsed / lerpTime); //使用Mathf.Lerp函数让数据的顺滑地变化

        //Vector2 newPosition = new Vector2(newX, ChoosePanel.anchoredPosition.y); //目标距离
        Vector2 newPosition = new Vector2(ChoosePanel.anchoredPosition.x, newX); //目标距离

        ChoosePanel.anchoredPosition = newPosition;

        if (elapsed > lerpTime)
        {
            elapsed = 0;
            isArrive = true;
        }
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrap()
    {
        dragging = false;
        isArrive = false;
    }

    public void DeActivePanel()
    {
        gameObject.SetActive(false);
    }

    public void ActivePanel()
    {
        gameObject.SetActive(true);
    }
}