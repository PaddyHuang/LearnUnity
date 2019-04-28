using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelCommon : MonoBehaviour
{
    private enum InOut
    {
        In,
        Out
    }

    private event Action OnPracClickHandler;
    public event Action OnPracClick
    {
        add
        {
            OnPracClickHandler -= value;
            OnPracClickHandler += value;
        }
        remove { OnPracClickHandler -= value; }
    }

    private event Action OnMainClickHandler;
    public event Action OnMainClick
    {
        add
        {
            OnMainClickHandler -= value;
            OnMainClickHandler += value;
        }
        remove { OnMainClickHandler -= value; }
    }

    [SerializeField] private Toggle pracTg, mainTg;

    [SerializeField] private Button outBtn, inBtn;

    [SerializeField] private RectTransform switchPanel;

    private float switchDistance;
    private bool isTranslating;

    void Start()
    {
        if (pracTg != null && mainTg != null)
        {
            pracTg.onValueChanged.AddListener(b => { OnPracClickHandler?.Invoke(); });
            mainTg.onValueChanged.AddListener(b => { OnMainClickHandler?.Invoke(); });
        }

        if (switchPanel != null)
            switchDistance = switchPanel.rect.width;
        if (outBtn != null && inBtn != null)
        {
            outBtn.onClick.AddListener(BeginOut);
            inBtn.onClick.AddListener(BeginIn);
        }

        //todo:临时策略
        //var uim = UIMainCanvas.Instance;
        //OnPracClick += uim.BackBtnDisable;
        //OnMainClick += uim.BackBtnActive;
    }

    /// <summary> 
    /// 设置主按钮的名字 </summary>
    public void SetMianBtnTitle(string title)
    {
        if (title != null)
        {
            Text paText = mainTg.GetComponentInParent<Text>();
            if (!ReferenceEquals(paText, null))
                paText.text = title;
        }
    }

    private void BeginOut()
    {
        if (isTranslating)
            return;
        outBtn.gameObject.SetActive(false);
        inBtn.gameObject.SetActive(true);
        StartCoroutine(SwitchMove(InOut.Out));
    }

    private void BeginIn()
    {
        if (isTranslating)
            return;
        outBtn.gameObject.SetActive(true);
        inBtn.gameObject.SetActive(false);
        StartCoroutine(SwitchMove(InOut.In));
    }

    //开关过度动画
    IEnumerator SwitchMove(InOut io)
    {
        float t = 0;

        float transTime = 0.8f;

        float elasped = 0;

        Vector2 startPos = switchPanel.anchoredPosition;
        Vector2 endPos;
        if (io == InOut.In)
            endPos = startPos - new Vector2(switchDistance, 0);
        else
            endPos = startPos + new Vector2(switchDistance, 0);

        isTranslating = true;
        while (t <= 1)
        {
            elasped += Time.deltaTime;

            t = elasped / transTime;
            switchPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        isTranslating = false;
    }
}