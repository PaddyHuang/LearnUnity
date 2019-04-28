using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Calculation : MonoBehaviour
{
    public GameObject CalculShowPrefab, SizePrefab;

    //中下方展示
    [SerializeField] private Text showAllText;
    [SerializeField] private Button playBtn, pauseBtn;

    private Dictionary<int, CalCulShowAnim> CalculationColl = new Dictionary<int, CalCulShowAnim>();
    private Queue<CalCulShowAnim> ShowQueue = new Queue<CalCulShowAnim>();

    private AnimState animState;

    private float elapsedTime;
    private float curtTime = GameData.Instance.AudioInterval;

    private CalCulShowAnim savePrvAnim;

    private AudioManager m_AudioManager;

    void Start()
    {
        m_AudioManager = AudioManager.Instance;
        playBtn.onClick.AddListener(PlayAnim);
        playBtn.onClick.AddListener(ContinueAnim);
        pauseBtn.onClick.AddListener(PauseAnim);
    }

    void Update()
    {
        if (animState == AnimState.Play)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > curtTime)
            {
                elapsedTime = 0;

                ShowAnimFunc();
            }
        }
    }

    void PlayAnim()
    {
        if (animState != AnimState.Await)
            return;

        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);

        animState = AnimState.Play;
        elapsedTime = 0;
        savePrvAnim = null;
        //初始化队列
        ShowQueue.Clear();
        foreach (var c in CalculationColl)
        {
            ShowQueue.Enqueue(c.Value);
            c.Value.Hide();
        }
    }

    void ContinueAnim()
    {
        if (animState != AnimState.Pause)
            return;
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
        animState = AnimState.Play;
    }

    void PauseAnim()
    {
        if (animState != AnimState.Play)
            return;

        animState = AnimState.Pause;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }

    void ShowAnimFunc()
    {
        if (ShowQueue.Count > 0)
        {
            savePrvAnim?.Hide();

            CalCulShowAnim show = ShowQueue.Dequeue();
            curtTime = show.Audio.length + GameData.Instance.AudioInterval;

            show.Show();
            m_AudioManager.PlayAudio(show.Audio);
            savePrvAnim = show;
            //Loop
            //ShowQueue.Enqueue(show);
        }
        else
        {
            ResetState(); //如果完成了一个循环
        }
    }

    /// <summary> 
    /// 重置状态 </summary>
    private void ResetState()
    {
        elapsedTime = 0;
        savePrvAnim = null;

        animState = AnimState.Await;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);

        foreach (var c in CalculationColl)
            c.Value.Hide();
    }

    /// <summary> 
    /// 切换暂停按钮 </summary>
    public void OnSwichPanel(PracMainSwitch pom)
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (pom == PracMainSwitch.Practice)
            PauseAnim();
    }

    /// <summary> 
    /// 构建显示的结构UI </summary>
    public void CreatCalculUI(TypeCalculation tcl, GameObject mainGo)
    {
        //--------------------------首先创建动态的计算物体--------------------------

        Dictionary<int, CalCulShowAnim> sortDic = new Dictionary<int, CalCulShowAnim>();

        CalculationSingle[] csg = tcl.CalcultionsList.ToArray();

        DownLoaderMono sm = DownLoaderMono.Instance;

        foreach (CalculationSingle ss in csg)
        {
            var creatModel = mainGo;

            //寻找绑定的物体
            GameObject findGo = StaticClass.RecursiveFindChild(creatModel.transform, ss.GameObjectName);

            if (ReferenceEquals(findGo, null))
                continue;

            GameObject go = Instantiate(CalculShowPrefab);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            //成功创建后
            UICalculShowSingle css = go.GetComponent<UICalculShowSingle>();

            if (!ReferenceEquals(css, null))
            {
                css.Init(ss, findGo);

                CalCulShowAnim calCul = new CalCulShowAnim
                {
                    ModelObjectInGame = findGo,
                    UIObjcet = go
                };

                sm.DownLoadAudio(ss.AudioUrl, a => calCul.Audio = a);
                //下载音频
                sortDic.Add(ss.Index, calCul);
            }
        }

        //排序
        CalculationColl = sortDic.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);

        showAllText.text = tcl.CalculComponentInfo;

        foreach (var c in CalculationColl)
            c.Value.Hide();
        //--------------------------至此计算物体创建完毕--------------------------

        //--------------------------下面开始创建尺寸 如“H”--------------------------

        CalculSizes[] ccsg = tcl.CalculSizesList.ToArray();

        foreach (var ss in ccsg)
        {
            var fg = StaticClass.RecursiveFindChild(mainGo.transform, ss.FromGoName);
            var tg = StaticClass.RecursiveFindChild(mainGo.transform, ss.ToGoName);

            GameObject sizeGo = Instantiate(SizePrefab, Vector3.zero, Quaternion.identity);
            UISizeSingleCreator ccc = sizeGo.GetComponentInChildren<UISizeSingleCreator>();

            if (!ReferenceEquals(ccc, null))
            {
                ccc.SetSizeLineCalcul(fg.transform, tg.transform, ss.Length);
                sizeGo.transform.SetParent(mainGo.transform);
                CalCulShowAnim calCul = new CalCulShowAnim {SizeGameObject = sizeGo};

                sm.DownLoadAudio(ss.AudioUrl, a => calCul.Audio = a);
                CalculationColl.Add(ss.Index, calCul);
            }
        }

        //初始化完成后隐藏
        foreach (var c in CalculationColl)
            c.Value.Hide();
    }

    public void ToggleFunc(bool b)
    {
        if (!b)
        {
            ResetState();
        }
    }

    /// <summary> 
    /// 重新识别物体时 </summary>
    public void OnReFound()
    {
        ResetState();
    }
}

public class CalCulShowAnim
{
    public GameObject ModelObjectInGame;
    public GameObject UIObjcet;

    public GameObject SizeGameObject;

    public AudioClip Audio;

    public void Show()
    {
        //如果是尺寸的物体
        if (!ReferenceEquals(SizeGameObject, null))
        {
            SizeGameObject.SetActive(true);
            return;
        }

        //如果不是
        if (ReferenceEquals(ModelObjectInGame, null) || ReferenceEquals(UIObjcet, null))
            return;

        ModelObjectInGame.SetActive(true);
        UIObjcet.SetActive(true);
    }

    public void Hide()
    {
        //如果是尺寸的物体
        if (!ReferenceEquals(SizeGameObject, null))
        {
            SizeGameObject.SetActive(false);
            return;
        }

        //如果不是
        if (ReferenceEquals(ModelObjectInGame, null) || ReferenceEquals(UIObjcet, null))
            return;

        ModelObjectInGame.SetActive(false);
        UIObjcet.SetActive(false);
    }
}