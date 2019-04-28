using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class UIMainCanvas : MonoBehaviour
{
    #region Mono-Singleton

    private static UIMainCanvas instance;
    public static UIMainCanvas Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<UIMainCanvas>();
            return instance;
        }
    }

    #endregion

    [SerializeField] private RectTransform recPanel;

    [SerializeField] private Button backBtn;

    [SerializeField] private Image frameImage;

    public int FrameWidth => (int) frameImage.rectTransform.rect.width;
    public int FrameHeight => (int) frameImage.rectTransform.rect.height;

    public RectTransform FrameImage => frameImage.rectTransform;

    public Image RecognizingImage;
    public Image ProgressImage;
    public RawImage WaveImage;
    public Text ProgressValue;
    
    private float rawUVx;

    private bool isShowEffect;

    private RecognizeManager m_RecognizeManager; //识别管理

    void Awake()
    {
        //startRecBtn.onClick.AddListener(
        //    () => OnRecBtnClickHandler?.Invoke());
        //OnRecBtnClick += RecBtnStartRec;
        
        backBtn.onClick.AddListener(ClickBackBtnFunc);
        //m_GameData = GameData.Instance;
        m_RecognizeManager = RecognizeManager.Instance;

        //添加旋转特效
        //RecognizingImage.gameObject.AddComponent<RecognizingRotate>();
    }

    /// <summary> 
    /// 返回按钮灰掉 </summary>
    public void BackBtnDisable()
    {
        backBtn.gameObject.SetActive(false);
    }

    /// <summary> 
    /// 返回按钮激活 </summary>
    public void BackBtnActive()
    {
        backBtn.gameObject.SetActive(true);
    }

    public void RecPanelHide()
    {
        recPanel.gameObject.SetActive(false);
    }

    public void RecPanelActive()
    {
        recPanel.gameObject.SetActive(true);
    }

    /// <summary> 
    /// 设置返回键点击 </summary>
    private void ClickBackBtnFunc()
    {
        var gd = GameDataMono.Instance;        

        if (!gd.CreateSuccessed && gd.CreatedObject == null)
        {
            SceneManager.LoadScene(StaticData.HomeSceneStr);
            return;
        }

        if (gd.CreatedObject != null && gd.CreatedObject.activeInHierarchy)
        {
            gd.CreatedObject.SetActive(false);

            SetDefaultToggle(gd);

            gd.SetCurrentNull();
            
            ObjectControl.GameObjectLost();

            AudioManager.Instance.PauseAudio();

            RecPanelActive();

            m_RecognizeManager.StartRecognize();                        
        }
    }

    public void SetLoadingPercentage(float displayProgress)
    {
        if (!ProgressImage.gameObject.activeInHierarchy)
        {
            ProgressImage.gameObject.SetActive(true);
            ProgressImage.fillAmount = 0;
            return;
        }

        rawUVx -= Time.deltaTime * 1f;
        WaveImage.uvRect = new Rect(new Vector2(rawUVx, 0), Vector2.one);

        float v = displayProgress;
        ProgressImage.fillAmount = v;
        ProgressValue.text = (int) (v * 100) + "%";
    }

    public void DisableProgressImage()
    {
        ProgressImage.gameObject.SetActive(false);
        ProgressImage.fillAmount = 0;
        ProgressValue.text = null;
        rawUVx = 0;
    }

    public void RecognizingEffect(bool isShow)
    {
        if (!isShowEffect && !isShow)
            return;

        isShowEffect = isShow;
        RecognizingImage.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 每次重新成功创建模型时，都默认显示介绍Toggle（演示版智慧课本用）
    /// </summary>
    /// <param name="gd"></param>
    private void SetDefaultToggle(GameDataMono gd)
    {
        try
        {
            Transform togglesContent = UtilsSearch.FindChildByName(gd.CreatedObject.transform, "TogglesContent");
            if (togglesContent != null)
            {
                for (int i = 0; i < togglesContent.childCount; i++)
                {
                    togglesContent.GetChild(i).GetComponent<Toggle>().isOn = false;
                }

                Toggle toggleDefault = togglesContent.GetChild(1).GetComponent<Toggle>();
                toggleDefault.isOn = true;
                //Debug.Log(toggleDefault.isOn);
            }

            var mainToggle = UtilsSearch.FindComponentChild<Toggle>(gd.CreatedObject.transform, "MainToggle");
            var pracToggle = UtilsSearch.FindComponentChild<Toggle>(gd.CreatedObject.transform, "PracToggle");
            if (mainToggle != null && pracToggle != null)
            {                
                pracToggle.isOn = false;
                mainToggle.isOn = true;
            }
        }
        catch { }   
    }
}