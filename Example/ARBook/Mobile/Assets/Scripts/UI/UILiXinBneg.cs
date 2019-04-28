using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UILiXinBneg : MonoBehaviour {
    
    private Animator animator;
    private AroundCameraMobile aroundCamera;
    private ItemIntroduce audioManager;    
    private Toggle toggleStruct, toggleIntroduce, togglePrac, toggleMain;
    private Button startShowBtn, pauseShowBtn;
    private Button replayBtn;

    private Vector3 targetPosition = new Vector3(-3, -0.6f, 0);
    private Vector3 originPosition = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private bool move;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aroundCamera = AroundCameraMobile.Instance;
    }

    private void OnEnable()
    {
        MoveEnd();

        // 由于每一个带有动画的模型都需要各自的光照，故添加此动态调节的光照
        var light = UtilsSearch.FindComponentChild<Light>(GameObject.FindGameObjectWithTag(StaticData.LightSystemTag).transform, StaticData.LightSourceName);
        light.intensity = 2;
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

    private void Update()
    {
        if (move)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1);
        }
        if (!move)
        {
            transform.position = Vector3.SmoothDamp(transform.position, originPosition, ref currentVelocity, 1);
        }
    }
        
    #region 动画控制函数
    /// <summary>
    /// 暂停动画
    /// </summary>
    private void PauseAnim()
    {
        animator.speed = 0.0f;
        audioManager.PauseShow();
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    private void StartAnim()
    {
        animator.speed = 0.8f;

        pauseShowBtn.gameObject.SetActive(true);
        startShowBtn.gameObject.SetActive(false);
    }    

    /// <summary>
    /// 重复播放动画
    /// </summary>
    public void ReplayAnim()
    {
        animator.Play(StaticData.animeName, 0, 0.0f);          // 重播动画
        audioManager.OnReFound();   // 重置音频               
        PauseAnim();

        MoveEnd();

        pauseShowBtn.gameObject.SetActive(false);
        startShowBtn.gameObject.SetActive(true);
    }

    #endregion

    #region 动画调用函数
    /// <summary>
    /// 由于离心泵太长，需要在动画播放时，动态移动模型
    /// </summary>
    public void MoveStart()
    {
        move = true;
    }

    /// <summary>
    /// 模型归位
    /// </summary>
    public void MoveEnd()
    {
        move = false;
    }
    #endregion
    
    #region Toggle响应
    /// <summary>
    /// 离心泵介绍toggle响应，默认还原
    /// </summary>
    private void IntrodeuceBtnClick()
    {
        if (toggleIntroduce.isOn)
        {
            ReplayAnim();            
        }                
    }

    /// <summary>
    /// 离心泵结构toggle响应，默认展开
    /// </summary>
    private void StructBtnClick()
    {
        if (toggleStruct.isOn)
        {            
            animator.Play(StaticData.animeName, 0, 1.0f);  // 直接展示动画       
            PauseAnim();
            audioManager.PauseShow();
            MoveStart();
        }                               
    }

    /// <summary>
    /// 离心泵练习toggle响应，默认展开
    /// </summary>
    private void PractiseBtnClick()
    {
        if (togglePrac.isOn)
        {
            animator.Play(StaticData.animeName, 0, 1.0f);  // 直接展示动画      
            PauseAnim();
            audioManager.PauseShow();
            MoveStart();
        }
    }

    public void MainBtnClick()
    {
        PauseAnim();
        if (toggleMain.isOn)
        {
            // 以介绍为默认打开的选项
            IntrodeuceBtnClick();
        }
    }

    #endregion 
    
}
