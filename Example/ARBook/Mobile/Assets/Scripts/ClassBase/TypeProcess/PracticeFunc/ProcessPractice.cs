using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class ProcessPractice : MonoBehaviour
{
    [SerializeField] private Text pracTitle; //练习题标题

    [SerializeField] private SelectTemplate selectTemplate; //选择类型末班

    [SerializeField] private UISnapSelect m_UISnapSelect; //滑动方法组件

    private SelectTemplate currentSelect; //当前选择

    private UIPracBottom uiPracBottom;

    public Dictionary<int, string> AnswersColl = new Dictionary<int, string>();
    public Dictionary<int, SelectTemplate> InputColl = new Dictionary<int, SelectTemplate>();

    public void CreatPractice(ProcessPracticeData ppd)
    {
        //注意索引是从1开始的
        for (int i = 1; i < ppd.PracticesGroup.Length + 1; i++)
        {
            SelectTemplate cg = Instantiate(selectTemplate, selectTemplate.transform.parent);

            cg.StepText.text = "第" + i + "步：";

            cg.SelectButton.onClick.AddListener(() => StartSelect(cg));

            InputColl.Add(i, cg);
        }

        //创建选择列表
        if (!ReferenceEquals(m_UISnapSelect, null))
        {
            m_UISnapSelect.CreatSnap(ppd.PracticesGroup);
            m_UISnapSelect.OnComfirm += GetRuturnValue;
        }

        selectTemplate.gameObject.SetActive(false);

        UIPracBottom pbp = GetComponent<UIPracBottom>();
        if (!ReferenceEquals(pbp, null))
        {
            pbp.ShowAnswer(ppd.IsShowAnswer);
            //注册答案方法
            pbp.OnAnswerFunc += CheckAnswer;
            uiPracBottom = pbp;
        }

        pracTitle.text = ppd.TextInfo;

        AnswersColl = new Dictionary<int, string>(ppd.AnswersColl);

        //刷新canva组
        Canvas.ForceUpdateCanvases();
    }

    /// <summary> 
    /// 检查和上传答案 </summary>
    private bool CheckAnswer()
    {
        List<ProcessPost> res = new List<ProcessPost>();

        foreach (var an in InputColl)
        {
            //空格去掉
            bool isr = an.Value.GetInputInfo.Trim() == AnswersColl[an.Key].Trim();

            ProcessPost n = new ProcessPost
            {
                Index = an.Key,
                IsRight = isr,
                UserAnswer = an.Value.GetInputInfo,
                RightAnswer = AnswersColl[an.Key]
            };

            res.Add(n);
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

    private void StartSelect(SelectTemplate cst)
    {
        if (ReferenceEquals(m_UISnapSelect, null))
            return;

        m_UISnapSelect.ActivePanel();

        currentSelect = cst;
    }

    /// <summary>
    /// 获取确认的返回值
    /// </summary>
    /// <param name="value"></param>
    private void GetRuturnValue(string value)
    {
        if (!ReferenceEquals(currentSelect, null))
        {
            var txt = currentSelect.SelectButton.GetComponentInChildren<Text>();
            if (txt != null)
                txt.text = value;
            m_UISnapSelect.DeActivePanel();
        }
    }
}