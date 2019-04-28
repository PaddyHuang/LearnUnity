using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class ItemPractice : MonoBehaviour
{
    [SerializeField] private RectTransform dragItemArea, containerPa;

    [SerializeField] private Text pracTitle;

    public GameObject ContainerPrefab, DragItemPrefab;

    //正确答案存储器
    //private Dictionary<ItemDragContainer, UIDrag> bindingDrag = new Dictionary<ItemDragContainer, UIDrag>();
    private Dictionary<ItemDragContainer, string> BindingDragPostColl = new Dictionary<ItemDragContainer, string>();

    private UIPracBottom uiPracBottom;

    /// <summary>
    /// 构建练习物体
    /// </summary>
    /// <param name="ipd">ItemPracticeData 数据</param>
    /// <param name="cg"></param>
    public void CreatPractice(ItemPracticeData ipd, GameObject cg)
    {
        if (ipd.PracticeDatas.Count != 0)
        {
            ScrollRect sr = dragItemArea.GetComponent<ScrollRect>();

            foreach (var ip in ipd.PracticeDatas)
            {
                GameObject findGo = StaticClass.RecursiveFindChild(cg.transform, ip.GameObjectName);
                if (ReferenceEquals(findGo, null))
                    continue;

                GameObject cp = Instantiate(ContainerPrefab, containerPa);
                //添加容器
                var idc = cp.GetComponent<ItemDragContainer>();
               
                //绑定容器的游戏物体
                idc.CreatContainer(findGo.transform, ip.LeftOrRight);

                //添加拖动物体
                GameObject di = Instantiate(DragItemPrefab, sr.content);
                UIDrag udn = di.GetComponent<UIDrag>();
                udn?.CreatDragItem(ip.Info);

                //UIDrag uid = di.GetComponent<UIDrag>();
                //if (uid != null)
                //    uid.CreatDragItem(ip.Info, containerPa);

                BindingDragPostColl.Add(idc, ip.Info);
            }

            Canvas.ForceUpdateCanvases();
        }

        UIPracBottom pbp = GetComponent<UIPracBottom>();
        if (!ReferenceEquals(pbp, null))
        {
            pbp.ShowAnswer(ipd.IsShowAnswer);
            //注册答案方法
            pbp.OnAnswerFunc += CheckAnswer;
            uiPracBottom = pbp;
        }

        //赋值标题
        pracTitle.text = ipd.PracTitle;
    }

    /// <summary> 
    /// 检查和上传答案 </summary>
    private bool CheckAnswer()
    {
        List<ItemPost> res = new List<ItemPost>();
        foreach (var result in BindingDragPostColl)
        {
            ItemPost inr = new ItemPost
            {
                //Index = result.Key.Index,
                RightAnswer = result.Value,
                //UserAnswer = result.Key.CurrentGrab.InfoText
            };

            inr.UserAnswer = result.Key.UIDrop?.ReceivingText.text;

            inr.IsRight = inr.RightAnswer == inr.UserAnswer;
            res.Add(inr);
        }

        var resultStr = JsonMapper.ToJson(res);
        StartCoroutine(StartPost(resultStr));

        //如果有一个错了
        foreach (var r in res)
        {
            if (r.IsRight == false)
                return false;
        }

        return true;
    }

    /// <summary> 
    /// 上传成绩数据 </summary>
    IEnumerator StartPost(string jsonData)
    {
        bool isSuccess = false;
        yield return StartCoroutine(StaticClass.Post(StaticData.AnswerPostUrl, jsonData, b => isSuccess = b));

        if (isSuccess)
            uiPracBottom?.ApplyButtonOn();
    }
}