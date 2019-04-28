using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPracBottom : MonoBehaviour
{
    //供答案方法注册
    public event Func<bool> OnAnswerFunc;

    [Header("按钮")] [SerializeField] private Button answerBtn;

    [SerializeField] 
    private Button closeAnswerBtn, applyBtn;

    [Header("Panel")] 
    [SerializeField] private RectTransform pracPanel;
    [SerializeField] private RectTransform rightAnswer, wrongAnswer;
    [SerializeField] private RectTransform answerPanel;

    [Header("答案")]
    [SerializeField] private Image answerImg;
    [SerializeField] private Image taskImg;

    private WaitForSeconds waitTime = new WaitForSeconds(2f);

    void Start()
    {
        answerBtn.onClick.AddListener(() => SwitchAnswerPanel(true));
        closeAnswerBtn.onClick.AddListener(() => SwitchAnswerPanel(false));

        pracPanel.gameObject.SetActive(false);
        applyBtn.onClick.AddListener(ApplyAnswer);
    }

    private void SwitchAnswerPanel(bool aoh)
    {
        answerPanel.gameObject.SetActive(aoh);
    }

    public void ShowAnswer(bool isShow)
    {
        answerBtn.gameObject.SetActive(isShow);
    }

    public void AddButtonFunc(UIPanelCommon upc)
    {
        if (!ReferenceEquals(upc, null))
        {
            upc.OnMainClick += SwitchOffPracPanel;
            upc.OnPracClick += SwitchOnPracPanel;
        }
    }

    private void SwitchOnPracPanel()
    {
        pracPanel.gameObject.SetActive(true);
    }

    private void SwitchOffPracPanel()
    {
        pracPanel.gameObject.SetActive(false);
    }

    /// <summary> 
    /// 提交答案 </summary>
    private void ApplyAnswer()
    {
        if (OnAnswerFunc != null)
        {
            rightAnswer.gameObject.SetActive(false);
            wrongAnswer.gameObject.SetActive(false);

            bool ans = OnAnswerFunc.Invoke();
            if (ans)
                rightAnswer.gameObject.SetActive(true);
            else
                wrongAnswer.gameObject.SetActive(true);

            //服务器返回前，不允许重复提交
            applyBtn.interactable = false;
            StopAllCoroutines();
            StartCoroutine(DelayHide());
        }
    }

    /// <summary> 
    /// 异步加载图片 </summary>
    public void LoadSprites(Sprite ans, Sprite task = null)
    {
        if (task != null)
            taskImg.sprite = task;

        answerImg.sprite = ans;
    }

    public void ApplyButtonOn()
    {
        applyBtn.interactable = true;
    }

    IEnumerator DelayHide()
    {
        yield return waitTime;
        rightAnswer.gameObject.SetActive(false);
        wrongAnswer.gameObject.SetActive(false);
    }
}