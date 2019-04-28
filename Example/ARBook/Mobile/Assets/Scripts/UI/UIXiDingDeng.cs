using Developer.CameraExtension;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIXiDingDeng : MonoBehaviour {

    [SerializeField] private Transform Light;

    private AroundCameraMobile aroundCamera;
    private Animator animator;
    private ItemIntroduce audioManager;

    private Toggle toggleStruct, toggleIntroduce, togglePrac, toggleMain;
    private Button startShowBtn, pauseShowBtn;
    private Button replayBtn;

    private bool continueFlag;

    private Range rangeX = new Range(-90, 40);
    private Range rangeY = new Range(-45, 45);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aroundCamera = AroundCameraMobile.Instance;
    }

    private void OnEnable()
    {
        animator.Play(StaticData.animeName, 0, 1.0f);  
        animator.speed = 0.0f;

        aroundCamera.angleRangeX = rangeX;
        aroundCamera.angleRangeY = rangeY;

        continueFlag = false;

        // 由于每一个带有动画的模型都需要各自的光照，故添加此动态调节的光照
        var light = UtilsSearch.FindComponentChild<Light>(GameObject.FindGameObjectWithTag(StaticData.LightSystemTag).transform, StaticData.LightSourceName);
        light.intensity = 0.5f;                
    }

    // Use this for initialization
    void Start () {
        startShowBtn = UtilsSearch.FindComponentChild<Button>(transform.root, StaticData.ButtonShowName);
        pauseShowBtn = UtilsSearch.FindComponentChild<Button>(transform.root, StaticData.ButtonPauseName);
        replayBtn = UtilsSearch.FindComponentChild<Button>(transform.root, StaticData.ButonReplayName);
        audioManager = UtilsSearch.FindComponentChild<ItemIntroduce>(transform.root, StaticData.ComponentIntroduceName);
        
        toggleMain = UtilsSearch.FindComponentChild<Toggle>(transform.root, StaticData.MainToggleName);
        togglePrac = UtilsSearch.FindComponentChild<Toggle>(transform.root, StaticData.PracToggleName);
        toggleIntroduce = UtilsSearch.FindChildByName(transform.root, StaticData.ToggleContentName).GetChild(1).GetComponent<Toggle>();
        toggleStruct = UtilsSearch.FindChildByName(transform.root, StaticData.ToggleContentName).GetChild(2).GetComponent<Toggle>();

        startShowBtn.onClick.AddListener(StartAnim);
        pauseShowBtn.onClick.AddListener(PauseAnim);
        replayBtn.onClick.AddListener(ReplayAnim);

        toggleMain.onValueChanged.AddListener(delegate { MainBtnClick(); });
        togglePrac.onValueChanged.AddListener(delegate { PractiseBtnClick(); });
        toggleIntroduce.onValueChanged.AddListener(delegate { IntrodeuceBtnClick(); });
        toggleStruct.onValueChanged.AddListener(delegate { StructBtnClick(); });

        MainBtnClick();
    }
    
    #region 动画控制函数
    /// <summary>
    /// 播放动画
    /// </summary>
    private void StartAnim()
    {
        if (!continueFlag)
        {
            animator.Play(StaticData.animeName, 0, 0.0f);  // 从头播放动画
        }
        animator.speed = 0.8f;
                
        pauseShowBtn.gameObject.SetActive(true);
        startShowBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    private void PauseAnim()
    {
        animator.speed = 0.0f;
        continueFlag = true;
        audioManager.PauseShow();
    }

    /// <summary>
    /// 重置动画
    /// </summary>
    private void ReplayAnim()
    {
        animator.Play(StaticData.animeName, 0, 1.0f);  // 直接展示动画        
        PauseAnim();

        audioManager.OnReFound();   // 重置音频
        
        TurnLightOff();

        continueFlag = false;
                
        pauseShowBtn.gameObject.SetActive(false);
        startShowBtn.gameObject.SetActive(true);
    }
    #endregion


    #region 动画调用函数
    /// <summary>
    /// 开灯
    /// </summary>
    public void TurnLightOn()
    {
        Light.gameObject.SetActive(true);
    }

    /// <summary>
    /// 关灯
    /// </summary>
    public void TurnLightOff()
    {
        Light.gameObject.SetActive(false);
    }
    #endregion

    #region Toggle响应
    /// <summary>
    /// 换热器介绍toggle响应
    /// </summary>
    private void IntrodeuceBtnClick()
    {
        if (toggleIntroduce.isOn)
        {
            animator.Play(StaticData.animeName, 0, 1.0f);  // 直接展示动画
            PauseAnim();
            audioManager.PauseShow();
            
            continueFlag = false;
        }
    }

    /// <summary>
    /// 换热器结构toggle响应，默认展开
    /// </summary>
    private void StructBtnClick()
    {
        if (toggleStruct.isOn)
        {
            animator.Play(StaticData.animeName, 0, (205 / 240.0f));  // 直接展示动画     
            PauseAnim();
            audioManager.PauseShow();
            TurnLightOff();

            continueFlag = false;
        }
    }

    /// <summary>
    /// 换热器练习toggle响应，默认展开
    /// </summary>
    private void PractiseBtnClick()
    {
        if (togglePrac.isOn)
        {
            animator.Play(StaticData.animeName, 0, (205 / 240.0f));  // 直接展示动画       
            PauseAnim();
            audioManager.PauseShow();
            TurnLightOff();

            continueFlag = false;
        }
    }

    /// <summary>
    /// 换热器MainToggle响应
    /// </summary>
    public void MainBtnClick()
    {
        if (toggleMain.isOn)
        {
            animator.Play(StaticData.animeName, 0, 1.0f);  // 直接展示动画
            PauseAnim();
            audioManager.PauseShow();
            TurnLightOff();

            if (toggleStruct.isOn)
            {
                StructBtnClick();
            }

            continueFlag = false;
        }
    }

    #endregion 
}
