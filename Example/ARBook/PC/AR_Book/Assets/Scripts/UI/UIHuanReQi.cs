using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIHuanReQi : MonoBehaviour {

    [SerializeField] private Transform toushi, CRG;       // 透视

    private Animator animator;
    private ItemIntroduce audioManager;
    private AroundCameraMobile aroundCamera;

    private Toggle toggleStruct, toggleIntroduce, togglePrac, toggleMain;
    private Button startShowBtn, pauseShowBtn;
    private Button replayBtn;

    private Color hideColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    private Color showColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Vector3 targetPosition = new Vector3(-1.6f, -4, 0);
    private Vector3 originPosition = new Vector3(0, -4, 0);
    private Vector3 currentVelocity = Vector3.zero;
    private bool move;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aroundCamera = AroundCameraMobile.Instance;
    }

    private void OnEnable()
    {
        move = false;

        // 由于每一个带有动画的模型都需要各自的光照，故添加此动态调节的光照
        var light = UtilsSearch.FindComponentChild<Light>(GameObject.FindGameObjectWithTag(StaticData.LightSystemTag).transform, StaticData.LightSourceName);
        light.intensity = 0.5f;        
    }

    // Use this for initialization
    void Start() {               
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

    private void LateUpdate()
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
    /// 播放动画
    /// </summary>
    private void StartAnim()
    {
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
        audioManager.PauseShow();
    }

    /// <summary>
    /// 重置动画
    /// </summary>
    private void ReplayAnim()
    {
        animator.Play(StaticData.animeName, 0, 0.0f);
        PauseAnim();
        audioManager.OnReFound();   // 重置音频
        
        MoveEnd();
        TouShiEnd();

        pauseShowBtn.gameObject.SetActive(false);
        startShowBtn.gameObject.SetActive(true);
    }
    #endregion
    
    #region 动画调用函数
    /// <summary>
    /// 播放时把toushi改成透明(运动动画	0-120)
    /// </summary>
    public void TouShiStart()
    {
        for (int i = 0; i < CRG.GetComponent<MeshRenderer>().materials.Length; i++)
        {
            UtilsRenderingSetting.setMaterialRenderingMode(CRG.GetComponent<MeshRenderer>().materials[i], UtilsRenderingSetting.RenderingMode.Fade);
            CRG.GetComponent<MeshRenderer>().materials[i].color = hideColor;
        }

        for (int i = 0; i < toushi.childCount; i++)
        {
            for (int j = 0; j < toushi.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++)
            {
                UtilsRenderingSetting.setMaterialRenderingMode(toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j], UtilsRenderingSetting.RenderingMode.Fade);
                toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j].color = hideColor;
                //Debug.Log(toushi.GetChild(i).GetComponent<Renderer>().materials[j].name + toushi.GetChild(i).GetComponent<Renderer>().materials[j].color.ToString());                
            }
        }
    }

    /// <summary>
    /// 播放完毕把toushi改回不透明
    /// </summary>
    public void TouShiEnd()
    {
        for (int i = 0; i < CRG.GetComponent<MeshRenderer>().materials.Length; i++)
        {
            UtilsRenderingSetting.setMaterialRenderingMode(CRG.GetComponent<MeshRenderer>().materials[i], UtilsRenderingSetting.RenderingMode.Opaque);
            CRG.GetComponent<MeshRenderer>().materials[i].color = showColor;
        }
        
        for (int i = 0; i < toushi.childCount; i++)
        {
            for (int j = 0; j < toushi.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++)
            {
                UtilsRenderingSetting.setMaterialRenderingMode(toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j], UtilsRenderingSetting.RenderingMode.Opaque);
                toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j].color = showColor;
            }
        }
    }

    /// <summary>
    /// 由于传热管太长，需要在动画播放时，动态移动模型
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
    /// 换热器介绍toggle响应，默认还原
    /// </summary>
    private void IntrodeuceBtnClick()
    {
        if (toggleIntroduce.isOn)
        {
            ReplayAnim();
        }        
    }

    /// <summary>
    /// 换热器结构toggle响应，默认展开
    /// </summary>
    private void StructBtnClick()
    {
        if (toggleStruct.isOn)
        {
            animator.Play(StaticData.animeName, 0, (380 / 530.0f));  // 直接展示动画     
            PauseAnim();
            audioManager.PauseShow();
            MoveStart();
            TouShiEnd();
        }        
    }

    /// <summary>
    /// 换热器练习toggle响应，默认展开
    /// </summary>
    private void PractiseBtnClick()
    {
        if (togglePrac.isOn)
        {
            animator.Play(StaticData.animeName, 0, (380 / 530.0f));  // 直接展示动画       
            PauseAnim();
            audioManager.PauseShow();
            MoveStart();
            TouShiEnd();
        }       
    }

    /// <summary>
    /// 换热器MainToggle响应
    /// </summary>
    public void MainBtnClick()
    {
        animator.Play(StaticData.animeName, 0, (380 / 530.0f));  // 直接展示动画
        PauseAnim();
        if (toggleMain.isOn)
        {            
            // 以介绍为默认打开的选项
            IntrodeuceBtnClick();
        }        
    }

    #endregion

        
}
