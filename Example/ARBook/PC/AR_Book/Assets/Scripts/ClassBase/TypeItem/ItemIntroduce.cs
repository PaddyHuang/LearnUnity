using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class ItemIntroduce : MonoBehaviour
{
    private enum IntroStep
    {
        Main = 0,
        Struct = 1,
        Size = 2,
        Finished = 3
    }

    private enum TypeChanged
    {
        None = 0,
        Structed = 1,
        Sized = 2
    }

    public GameObject Controller;
    [SerializeField] private Text showAllInfo;
    [SerializeField] private Button startShowBtn, pauseShowBtn, replayBtn;

    private List<int> SaveStepList = new List<int> {0, 4};
    private Queue<int> StepQueue = new Queue<int>();

    private List<int> SaveTypeList = new List<int>();
    private Queue<int> TypeQueue = new Queue<int>();

    private IntroStep introStep = IntroStep.Main;
    private TypeChanged typeChanged = TypeChanged.None;

    //保存引用
    private Introduce.MainShow m_MainShow;

    private ItemStruct m_Struct;

    private ItemSizes m_Sizes;

    private AudioManager m_AudioManager;

    private StringBuilder cacheStr = new StringBuilder();
        
    private float elapsedTime;
    private float curtTime = GameData.Instance.AudioInterval;

    private float CurtTime
    {
        get { return curtTime; }
        set { curtTime = value + GameData.Instance.AudioInterval; }
    }

    private AnimState animState = AnimState.Await;

    private bool isActiveThis; //切换练习题时需要判断

    private void OnEnable()
    {
        // 产品手册才显示重置按钮
        if (string.Equals(StaticData.BookInfo.currentBookID, StaticData.BookInfo.ChanPinShouCeID))
        {
            replayBtn.gameObject.SetActive(true);
        }
    }

    void Start()
    {        
        startShowBtn.onClick.AddListener(StartShow);
        startShowBtn.onClick.AddListener(ContinueShow);
        pauseShowBtn.onClick.AddListener(PauseShow);
        replayBtn.onClick.AddListener(Replay);
    }

    void Update()
    {
        if (animState == AnimState.Play)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > CurtTime)
            {                
                elapsedTime = 0;
                ShowingSelect();                
            }
        }        
    }

    private void OnDisable()
    {
        // 不是物件类不显示重置按钮
        replayBtn.gameObject.SetActive(false);
    }

    private IntroStep GetStep()
    {
        introStep = (IntroStep) StepQueue.Dequeue();
        return introStep;
    }

    private TypeChanged GetTypeChanged()
    {
        typeChanged = (TypeChanged) TypeQueue.Dequeue();
        return typeChanged;
    }

    /// <summary> 
    /// 重置步骤等 </summary>
    private void StepReset()
    {
        StepQueue.Clear();
        foreach (int t in SaveStepList)
            StepQueue.Enqueue(t);
        TypeQueue.Clear();
        foreach (int t in SaveTypeList)
            TypeQueue.Enqueue(t);
    }

    /// <summary> 
    /// 是否完成了一个类型模块 </summary>
    private void TypeChanging()
    {
        switch (typeChanged)
        {
            case TypeChanged.Structed:
                m_Struct.IntroduceReset();
                typeChanged = TypeChanged.None;
                break;
            case TypeChanged.Sized:
                m_Sizes.IntroduceReset();
                typeChanged = TypeChanged.None;
                break;
        }
    }

    /// <summary> 
    /// 决定进行哪个步骤 </summary>
    private void ShowingSelect()
    {
        TypeChanging();

        switch (introStep)
        {
            case IntroStep.Struct:
                ShowingStruct();
                break;
            case IntroStep.Size:
                ShowingSizes();
                break;
            case IntroStep.Finished:
                LoopFunc();
                break;
        }        
    }

    /// <summary> 
    /// 循环 </summary>
    private void LoopFunc()
    {
        showAllInfo.text = m_MainShow.Info;
        StepReset();
        introStep = GetStep();

        CurtTime = m_MainShow.Audio.length;
        //播放主音频
        m_AudioManager.PlayAudio(m_MainShow.Audio);
                
        elapsedTime = 0;        
    }

    //开始动画的播放
    private void StartShow()
    {
        if (animState != AnimState.Await)
            return;

        StepReset();

        //延长显示主信息的时间
        CurtTime = m_MainShow.Audio.length;

        //播放主音频
        m_AudioManager.PlayAudio(m_MainShow.Audio);

        showAllInfo.text = m_MainShow.Info;
                
        introStep = GetStep();

        animState = AnimState.Play;

        pauseShowBtn.gameObject.SetActive(true);
        startShowBtn.gameObject.SetActive(false);        
    }

    private void ContinueShow()
    {
        if (animState != AnimState.Pause)
            return;
        
        animState = AnimState.Play;
        
        pauseShowBtn.gameObject.SetActive(true);
        startShowBtn.gameObject.SetActive(false);

        m_AudioManager.ContinueAudio();
    }

    public void PauseShow()
    {
        if (animState != AnimState.Play)
            return;
               
        animState = AnimState.Pause;
        pauseShowBtn.gameObject.SetActive(false);
        startShowBtn.gameObject.SetActive(true);

        m_AudioManager.PauseAudio();
    }

    private void Replay()
    {
        AroundCameraMobile.Instance.Reset(true);
    }

    //结构动画
    private void ShowingStruct()
    {
        Introduce.Structs m_struct = m_Struct.IntroduceShow();

        if (m_struct != null)
        {
            showAllInfo.text = m_struct.Info;
            //字符串拼接缓存，所有结构动画展示完后 需要展示该拼接字符串
            cacheStr.Append(m_struct.Info).Append(";");

            CurtTime = m_struct.Audio.length;
        }
        else
        {
            introStep = GetStep();
            showAllInfo.text = cacheStr.ToString();
            cacheStr.Clear();
            //标记进行模块切换
            typeChanged = GetTypeChanged();
        }
    }

    //尺寸动画
    private void ShowingSizes()
    {
        Introduce.Sizes m_sizes = m_Sizes.IntroduceShow();

        if (m_sizes != null)
        {
            showAllInfo.text = m_sizes.Info;
            cacheStr.Append(m_sizes.Info).Append(";");

            CurtTime = m_sizes.Audio.length;
        }
        else
        {
            introStep = GetStep();
            showAllInfo.text = cacheStr.ToString();
            cacheStr.Clear();
            //标记进行模块切换
            typeChanged = GetTypeChanged();
        }
    }

    /// <summary> 
    /// 创建入口 </summary>
    public void CreatIntroduceUI(Introduce it)
    {
        if (it != null)
        {
            m_MainShow = new Introduce.MainShow();

            DownLoaderMono.Instance.DownLoadAudio(it.AudioUrl, m_MainShow.SetAudio);
            m_MainShow.Info = it.IntroduceInfo;

            //存储主信息
            showAllInfo.text = m_MainShow.Info;

            //先把main和finied预先添加进去
            SaveStepList = new List<int> {3};

            isActiveThis = true;//因为一开始就是激活的

            m_AudioManager = AudioManager.Instance;
        }
    }

    public void ToggleFunc(bool b)
    {
        if (!b)
        {
            m_Struct?.IntroduceReset();
            m_Sizes?.IntroduceReset();            
            animState = AnimState.Await;
            elapsedTime = 0;
            showAllInfo.text = m_MainShow.Info;
            cacheStr.Clear();
            
            pauseShowBtn.gameObject.SetActive(false);
            startShowBtn.gameObject.SetActive(true);
        }

        isActiveThis = b;
    }

    /// <summary>
    ///  回调
    /// </summary>
    public void OnReFound()
    {
        m_Struct?.IntroduceReset();
        m_Sizes?.IntroduceReset();       
        animState = AnimState.Await;
        elapsedTime = 0;
        showAllInfo.text = m_MainShow.Info;
        cacheStr.Clear();

        pauseShowBtn.gameObject.SetActive(false);
        startShowBtn.gameObject.SetActive(true);
                
        m_AudioManager.PauseAudio();
    }

    /// <summary> 
    /// 切换暂停按钮 </summary>
    public void OnSwichPanel(PracMainSwitch pom)
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (pom == PracMainSwitch.Practice)
        {
            PauseShow();            
        }
    }

    public void SwitchPractice(bool isthis)
    {
        if (isActiveThis)
        {
            ToggleFunc(isthis);
            isActiveThis = true;
        }
    }

    /// <summary> 
    /// 设置结构模块引用，可以为空 </summary>
    public void SetStruct(ItemStruct iss)
    {
        if (iss != null)
        {
            m_Struct = iss;
            //type
            SaveTypeList.Add(1);
            //step
            SaveStepList.Add(1);
            SaveStepList.Sort();
        }
    }

    /// <summary> 
    /// 设置尺寸引用，可以为空 </summary>
    public void SetSizes(ItemSizes iss)
    {
        if (iss != null)
        {
            m_Sizes = iss;
            SaveStepList.Add(2);
            SaveStepList.Sort();
            SaveTypeList.Add(2);
        }
    }
      
}