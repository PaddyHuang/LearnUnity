using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIHuanReQi : MonoBehaviour {

    [SerializeField] private Transform toushi;       // 透视

    private Animator animator;
    //private AroundCameraMobile aroundCamera;    
    //private AudioManager audioManager;

    private Toggle toggleStruct, toggleIntroduce, togglePrac, toggleMain;
    private Button startShowBtn, pauseShowBtn;
    private Button replayBtn;
        
    private Vector3 targetPosition = new Vector3(-1.9f, -4, 0);
    private Vector3 originPosition = new Vector3(0, -4, 0);

    private void Awake()
    {
        animator = GetComponent<Animator>();        
        //audioManager = AudioManager.Instance;
    }

    private void OnEnable()
    {
        // 由于每一个带有动画的模型都需要各自的光照，故添加此动态调节的光照
        //var light = UtilsSearch.FindComponentChild<Light>(GameObject.FindGameObjectWithTag(StaticData.LightSystemTag).transform, StaticData.LightSourceName);
        //light.intensity = 0.5f;
    }

    // Use this for initialization
    void Start() {               
        //startShowBtn = UtilsSearch.FindComponentChild<Button>(transform.root, StaticData.ButtonShowName);
        //pauseShowBtn = UtilsSearch.FindComponentChild<Button>(transform.root, StaticData.ButtonPauseName);
        //replayBtn = UtilsSearch.FindComponentChild<Button>(transform.root, StaticData.ButonReplayName);

        //toggleMain = UtilsSearch.FindComponentChild<Toggle>(transform.root, StaticData.MainToggleName);
        //togglePrac = UtilsSearch.FindComponentChild<Toggle>(transform.root, StaticData.PracToggleName);
        //toggleIntroduce = UtilsSearch.FindChildByName(transform.root, StaticData.ToggleContentName).GetChild(1).GetComponent<Toggle>();
        //toggleStruct = UtilsSearch.FindChildByName(transform.root, StaticData.ToggleContentName).GetChild(2).GetComponent<Toggle>();

        //startShowBtn.onClick.AddListener(StartAnim);
        //pauseShowBtn.onClick.AddListener(PauseAnim);        
        //replayBtn.onClick.AddListener(ReplayAnim);

        //toggleMain.onValueChanged.AddListener(delegate { MainBtnClick(); });
        //togglePrac.onValueChanged.AddListener(delegate { PractiseBtnClick(); });
        //toggleIntroduce.onValueChanged.AddListener(delegate { IntrodeuceBtnClick(); });
        //toggleStruct.onValueChanged.AddListener(delegate { StructBtnClick(); });

        //MainBtnClick();
    }

    private void OnDisable()
    {
        //replayBtn.gameObject.SetActive(false);
    }

    #region 动画控制函数
    /// <summary>
    /// 播放动画
    /// </summary>
    private void StartAnim()
    {
        //animator.speed = 1.0f;

        //replayBtn.gameObject.SetActive(true);
        //pauseShowBtn.gameObject.SetActive(true);
        //startShowBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    private void PauseAnim()
    {
        //animator.speed = 0.0f;
        //audioManager.PauseAudio();
    }

    /// <summary>
    /// 重置动画
    /// </summary>
    private void ReplayAnim()
    {
        //animator.Play(StaticData.animeName, 0, 0.0f);
        //PauseAnim();
        //audioManager.PauseAudio();
        //MoveEnd();

        //toushi.gameObject.SetActive(true);
        //pauseShowBtn.gameObject.SetActive(false);
        //startShowBtn.gameObject.SetActive(true);
    }
    #endregion
    
    #region 动画调用函数
    /// <summary>
    /// 播放时把toushi改成透明(运动动画	0-120)
    /// </summary>
    public void TouShiStart()
    {
        //toushi.gameObject.SetActive(false);

        for (int i = 0; i < toushi.childCount; i++)
        {
            for (int j = 0; j < toushi.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++)
            {                
                setMaterialRenderingMode(toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j], RenderingMode.Fade);
                toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                //Debug.Log(toushi.GetChild(i).GetComponent<Renderer>().materials[j].name + toushi.GetChild(i).GetComponent<Renderer>().materials[j].color.ToString());                
            }
        }
    }

    /// <summary>
    /// 播放完毕把toushi改回不透明
    /// </summary>
    public void TouShiEnd()
    {
        //toushi.gameObject.SetActive(true);

        for (int i = 0; i < toushi.childCount; i++)
        {
            for (int j = 0; j < toushi.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++)
            {                
                setMaterialRenderingMode(toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j], RenderingMode.Opaque);
                toushi.GetChild(i).GetComponent<MeshRenderer>().materials[j].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    /// <summary>
    /// 由于传热管太长，需要在动画播放时，动态移动模型
    /// </summary>
    public void MoveStart()
    {
        //transform.position = targetPosition;
        // 待平滑处理
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2);

    }

    /// <summary>
    /// 模型归位
    /// </summary>
    public void MoveEnd()
    {
        //transform.position = originPosition;
        //transform.position = Vector3.Lerp(transform.position, originPosition, Time.deltaTime * 2);
    }
    #endregion
    
    #region Toggle响应
    /// <summary>
    /// 换热器介绍toggle响应，默认还原
    /// </summary>
    private void IntrodeuceBtnClick()
    {
        //if (toggleIntroduce.isOn)
        //{
        //    ReplayAnim();
        //}        
    }

    /// <summary>
    /// 换热器结构toggle响应，默认展开
    /// </summary>
    private void StructBtnClick()
    {
        //if (toggleStruct.isOn)
        //{
        //    animator.Play(StaticData.animeName, 0, (380 / 530.0f));  // 直接展示动画     
        //    PauseAnim();
        //    audioManager.PauseAudio();
        //    MoveStart();
        //    TouShiEnd();
        //}        
    }

    /// <summary>
    /// 换热器练习toggle响应，默认展开
    /// </summary>
    private void PractiseBtnClick()
    {
        //if (togglePrac.isOn)
        //{
        //    animator.Play(StaticData.animeName, 0, (380 / 530.0f));  // 直接展示动画       
        //    PauseAnim();
        //    audioManager.PauseAudio();
        //    MoveStart();
        //    TouShiEnd();
        //}       
    }

    /// <summary>
    /// 换热器MainToggle响应
    /// </summary>
    public void MainBtnClick()
    {
        //animator.Play(StaticData.animeName, 0, (380 / 530.0f));  // 直接展示动画
        //PauseAnim();
        //if (toggleMain.isOn)
        //{            
        //    // 以介绍为默认打开的选项
        //    IntrodeuceBtnClick();
        //}        
    }

    #endregion


    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }
    //设置材质的渲染模式
    private static void setMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                //material.SetFloat("" _Mode & quot;", 2);
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
