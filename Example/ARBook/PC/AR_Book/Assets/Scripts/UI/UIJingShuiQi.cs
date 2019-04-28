using Developer.CameraExtension;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIJingShuiQi : MonoBehaviour {

    [SerializeField] private Transform WaiKe;

    private AroundCameraMobile aroundCamera;
    private Animator animator;
    private ItemIntroduce audioManager;

    private Toggle toggleStruct, toggleIntroduce, togglePrac, toggleMain;
    private Button startShowBtn, pauseShowBtn;
    private Button replayBtn;

    private bool continueFlag;

    private Range showRangeX = new Range(20, 90);
    private Range hideRangeX = new Range(-90, 90);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aroundCamera = AroundCameraMobile.Instance;        
    }

    private void OnEnable()
    {        
        continueFlag = false;
        animator.Play(StaticData.animeName, 0, (930 / 1360.0f));  // 直接展示动画 
        animator.speed = 0.0f;

        // 由于每一个带有动画的模型都需要各自的光照，故添加此动态调节的光照
        var light = UtilsSearch.FindComponentChild<Light>(GameObject.FindGameObjectWithTag(StaticData.LightSystemTag).transform, StaticData.LightSourceName);
        light.intensity = 1.0f;        
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

    private void OnDisable()
    {
        WaiKeHide();                   
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
        animator.speed = 0.6f;        
                
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
        animator.Play(StaticData.animeName, 0, (930 / 1360.0f));  // 直接展示动画        
        PauseAnim();

        audioManager.OnReFound();   // 重置音频
          
        continueFlag = false;

        var targetAngels = Quaternion.Euler(-20f, -180f, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetAngels, 1.0f);
        aroundCamera.angleRangeX = showRangeX;
                
        pauseShowBtn.gameObject.SetActive(false);
        startShowBtn.gameObject.SetActive(true);
    }
    #endregion

    #region 动画调用函数
    /// <summary>
    /// 播放动画时隐藏水槽外壳
    /// </summary>
    public void WaiKeHide()
    {        
        WaiKe.gameObject.SetActive(false);
        aroundCamera.angleRangeX = hideRangeX;
    }

    /// <summary>
    /// 播放动画时显示水槽外壳
    /// </summary>
    public void WaiKeShow()
    {
        //WaiKe.gameObject.SetActive(true);
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
            animator.Play(StaticData.animeName, 0, (930 / 1360.0f));  // 直接展示动画
            PauseAnim();
            audioManager.PauseShow();
            //WaiKeShow();

            continueFlag = false;
        }
    }

    /// <summary>
    /// 净水器结构toggle响应，默认展开
    /// </summary>
    private void StructBtnClick()
    {
        if (toggleStruct.isOn)
        {
            animator.Play(StaticData.animeName, 0, (930 / 1360.0f));  // 直接展示动画     
            PauseAnim();
            audioManager.PauseShow();
            //WaiKeHide();

            continueFlag = false;
        }
    }

    /// <summary>
    /// 净水器练习toggle响应，默认展开
    /// </summary>
    private void PractiseBtnClick()
    {
        if (togglePrac.isOn)
        {
            animator.Play(StaticData.animeName, 0, (930 / 1360.0f));  // 直接展示动画       
            PauseAnim();
            audioManager.PauseShow();
            WaiKeHide();

            continueFlag = false;
        }
    }

    /// <summary>
    /// 净水器MainToggle响应
    /// </summary>
    public void MainBtnClick()
    {        
        if (toggleMain.isOn)
        {
            animator.Play(StaticData.animeName, 0, (930 / 1360.0f));  // 直接展示动画
            PauseAnim();
            audioManager.PauseShow();
            WaiKeHide();

            continueFlag = false;
        }
    }

    #endregion 
}
