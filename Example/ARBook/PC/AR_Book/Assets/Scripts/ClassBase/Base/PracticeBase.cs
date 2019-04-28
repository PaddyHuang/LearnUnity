using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class PracticeBase : MonoBehaviour
{
    [SerializeField] private Text pracTitle;

    protected UIPracBottom uiPracBottom;

    protected abstract bool CheckAnswer();

    //public virtual void CreatPractice(MethodPracticeData mpd)
    //{
    //    UIPracBottom pbp = GetComponent<UIPracBottom>();
    //    if (!ReferenceEquals(pbp, null))
    //    {
    //        pbp.ShowAnswer(mpd.IsShowAnswer);
    //        //注册答案方法
    //        pbp.OnAnswerFunc += CheckAnswer;
    //        uiPracBottom = pbp;
    //    }

    //    pracTitle.text = mpd.TextInfo;
    //}



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