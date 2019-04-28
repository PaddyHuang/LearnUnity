using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class CalculPractice : MonoBehaviour
{
    public Text InfoContainer;

    [Header("图片")] [SerializeField] private Text textTemplate;

    [SerializeField] private InputField inputTemplate;

    private Dictionary<int, InputField> InputsColl = new Dictionary<int, InputField>();
    private Dictionary<int, float> AnswersColl = new Dictionary<int, float>();

    private UIPracBottom uiPracBottom;

    public void CreatPractice(CalculPracticeData cpd)
    {
        var markStr = inputTemplate.textComponent.text;
        //根据占位符创建
        var rp = cpd.TextInfo.Replace(TypeData.Symbol, markStr);

        //深拷贝,必须在recode前
        AnswersColl = new Dictionary<int, float>(cpd.AnswersColl);
        RecodeString(rp);

        UIPracBottom pbp = GetComponent<UIPracBottom>();
        if (!ReferenceEquals(pbp, null))
        {
            pbp.ShowAnswer(cpd.IsShowAnswer);
            //注册答案方法
            pbp.OnAnswerFunc += CheckAnswer;
            uiPracBottom = pbp;
        }
    }

    /// <summary> 
    /// 检查和上传答案 </summary>
    private bool CheckAnswer()
    {
        List<CalculPost> res = new List<CalculPost>();

        foreach (var an in InputsColl)
        {
            if (string.IsNullOrEmpty(an.Value.text))
                continue;

            //误差在0.001范围内
            bool isr = Mathf.Abs(AnswersColl[an.Key] - float.Parse(an.Value.text)) < 0.001f;

            CalculPost n = new CalculPost
            {
                Index = an.Key,
                IsRight = isr,
                Input = an.Value.text
            };

            res.Add(n);
        }

        var resultStr = JsonMapper.ToJson(res);
        StartCoroutine(StartPost(resultStr));

        //白卷
        if (res.Count == 0)
            return false;

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

    /// <summary> 
    /// 递归重组字符串 </summary>
    private void RecodeString(string info)
    {
        //加载到text框架中并刷新
        InfoContainer.text = info;
        Canvas.ForceUpdateCanvases();

        string useStr = info;
        var tempList = new List<string>();
        var cc = InfoContainer.cachedTextGenerator;

        //根据行数切割字符串
        for (var i = 0; i < cc.lines.Count; i++)
        {
            string rrsult;
            if (i == cc.lineCount - 1)
                rrsult = useStr.Substring(cc.lines[i].startCharIdx);
            else
                rrsult = useStr.Substring(cc.lines[i].startCharIdx, cc.lines[i + 1].startCharIdx - cc.lines[i].startCharIdx);

            tempList.Add(rrsult);
        }

        //将截断的字段进行重排
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].EndsWith("$"))
            {
                int last = tempList[i].LastIndexOf("#", StringComparison.Ordinal);
                var spStr = tempList[i].Substring(last);

                tempList[i] = tempList[i].Remove(last);

                tempList[i] = tempList[i].Insert(tempList[i].Length, "\n");
                tempList[i + 1] = tempList[i + 1].Insert(0, spStr);

                string cb = null;
                foreach (var ag in tempList)
                    cb += ag;

                RecodeString(cb);
                return;
            }
        }

        CreatFormatText(tempList);
    }

    /// <summary> 
    /// 根据重排成功的字符串数组生成组件 </summary>
    private void CreatFormatText(List<string> sg)
    {
        string markStr = inputTemplate.textComponent.text;
        var tempLists = sg;

        Queue<int> inputQue = new Queue<int>();

        foreach (var a in AnswersColl)
            inputQue.Enqueue(a.Key);
        
        var inputWidth = inputTemplate.GetComponent<RectTransform>().rect.width;

        var cx = InfoContainer.rectTransform.anchoredPosition.x;
        var cy = InfoContainer.rectTransform.anchoredPosition.y;
        var ch = -textTemplate.rectTransform.rect.height; //这里是负数

        var lineIndex = 0;
        //var inputCount = 0;
        foreach (var line in tempLists)
        {
            //总长度,每次循环都会重置
            var totalLength = cx;

            //创建Input
            if (line.Contains(markStr)) //如果该行需要创建Input
            {
                var ng = line.Split(new[] {markStr}, StringSplitOptions.None);

                for (var i = 0; i < ng.Length; i++)
                {
                    var nInner = Instantiate(textTemplate, InfoContainer.transform.parent);
                    nInner.text = ng[i];
                    nInner.rectTransform.anchoredPosition = new Vector2(totalLength, cy + ch * lineIndex);

                    //如果不是最后一行
                    if (i != ng.Length - 1)
                    {
                        InputField iff = Instantiate(inputTemplate, InfoContainer.transform.parent);
                        iff.gameObject.SetActive(true);
                        iff.text = null;
                        //需要强制刷新一次
                        Canvas.ForceUpdateCanvases();
                        var textLength = nInner.rectTransform.rect.width;
                        //总长变化
                        totalLength += textLength;
                        iff.GetComponent<RectTransform>().anchoredPosition = new Vector2(totalLength, nInner.rectTransform.anchoredPosition.y);
                        //总长变化
                        totalLength += inputWidth;

                        //字典存储
                        //inputCount++;
                        InputsColl.Add(inputQue.Dequeue(), iff);
                    }
                }
            }
            else
            {
                var nT = Instantiate(textTemplate, InfoContainer.transform.parent);
                nT.text = line;
                nT.rectTransform.anchoredPosition = new Vector2(cx, cy + ch * lineIndex);
            }

            lineIndex++;
        }

        InfoContainer.gameObject.SetActive(false);
    }
}