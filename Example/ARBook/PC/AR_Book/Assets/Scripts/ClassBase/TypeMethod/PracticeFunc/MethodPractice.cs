using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class MethodPractice : MonoBehaviour
{
    [SerializeField] private Text pracTitle;
    [SerializeField] private Toggle selectTemplate;

    //[SerializeField] private Image taskImage, answerImg;

    private Dictionary<int, Toggle> SelectionColl = new Dictionary<int, Toggle>();
    private List<int> AnswersList = new List<int>();

    private UIPracBottom uiPracBottom;

    public void CreatPractice(MethodPracticeData mpd)
    {
        foreach (var c in mpd.PracticeUrls)
        {
            Toggle cg = Instantiate(selectTemplate, selectTemplate.transform.parent);
            cg.gameObject.SetActive(true);

            SelectionColl.Add(c.Key, cg);
        }

        AnswersList = mpd.Answers;

        UIPracBottom pbp = GetComponent<UIPracBottom>();
        if (!ReferenceEquals(pbp, null))
        {
            pbp.ShowAnswer(mpd.IsShowAnswer);
            //注册答案方法
            pbp.OnAnswerFunc += CheckAnswer;
            uiPracBottom = pbp;
        }

        pracTitle.text = mpd.TextInfo;


        //刷新canva组
        Canvas.ForceUpdateCanvases();
    }

    /// <summary> 
    /// 异步加载图片，并未进行排序 </summary>
    public void LoadSprites(List<Sprite> sg)
    {
        int i = 0;

        foreach (var s in SelectionColl)
        {
            s.Value.targetGraphic.GetComponent<Image>().sprite = sg[i];
            i++;
        }
    }

    /// <summary> 
    /// 检查和上传答案 </summary>
    private bool CheckAnswer()
    {
        List<MethodPost> res = new List<MethodPost>();

        foreach (var result in SelectionColl)
        {
            if (result.Value.isOn)
            {
                MethodPost mp = new MethodPost {Selected = result.Key};
                res.Add(mp);
            }
        }

        var resultStr = JsonMapper.ToJson(res);
        StartCoroutine(StartPost(resultStr));

        //数量不等直接错误
        if (res.Count != AnswersList.Count)
            return false;

        //如果有一个错了

        foreach (var r in res)
        {
            if (!AnswersList.Contains(r.Selected))
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