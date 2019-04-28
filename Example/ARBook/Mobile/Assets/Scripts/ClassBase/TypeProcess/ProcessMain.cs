using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AudioTimeCompare
{
    public AudioClip Audio;
    public float Interval;

    public float DValue()
    {
        return Audio.length - Interval;
    }
}

public class ProcessMain : MonoBehaviour
{
    /// <summary> 
    /// 控制animstate的状态 </summary>
    private enum State
    {
        Await,
        Pause,
        Play
    }

    /// <summary> 
    /// 控制update是否进行 </summary>
    private enum Control
    {
        Playing,
        Pause,
        Stop
    }

    [SerializeField] private Text textShow;

    public Button PlayBtn, PauseBtn;

    //用于存储展示的信息
    private string[] ShowInfoGroup;
    private Queue<string> InfoQueue = new Queue<string>(); //队列用以展示

    private AudioTimeCompare[] TimeComparesGroup;
    private Queue<AudioTimeCompare> TimeComparesQueue = new Queue<AudioTimeCompare>();

    private AudioManager m_AudioManager;

    private Animator saveAnim;
    private string firstAnimName;

    private State state = State.Play;
    private Control control = Control.Stop;

    private float continueTime;
    private float stopTime;
    private float elapsedTime;

    void Start()
    {
        PlayBtn.onClick.AddListener(AnimStart);
        PlayBtn.onClick.AddListener(AnimContinue);
        PauseBtn.onClick.AddListener(AnimPause);

        m_AudioManager = AudioManager.Instance;
    }

    void Update()
    {
        if (control == Control.Playing)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > stopTime && state == State.Await)
            {
                saveAnim.enabled = false;
                state = State.Pause;
            }

            if (state == State.Pause && elapsedTime > continueTime + GameData.Instance.AudioInterval)
            {
                elapsedTime = continueTime = stopTime = 0;

                state = State.Await;

                saveAnim.enabled = true;

                ShowingFunc();
            }
        }
    }

    /// <summary> 
    /// 开始动画 </summary>
    private void AnimStart()
    {
        if (control != Control.Stop)
            return;

        ResetState();
        saveAnim.enabled = true;

        PauseBtn.gameObject.SetActive(true);
        PlayBtn.gameObject.SetActive(false);

        ShowingFunc();
        control = Control.Playing;
        state = State.Await;
    }

    /// <summary> 
    /// 继续动画 </summary>
    private void AnimContinue()
    {
        if (control != Control.Pause)
            return;

        PauseBtn.gameObject.SetActive(true);
        PlayBtn.gameObject.SetActive(false);

        control = Control.Playing;
        m_AudioManager.ContinueAudio();
        //saveAnim.enabled = true;
    }

    /// <summary> 
    /// 暂停动画
    /// </summary>
    private void AnimPause()
    {
        if (control != Control.Playing)
            return;

        PauseBtn.gameObject.SetActive(false);
        PlayBtn.gameObject.SetActive(true);

        saveAnim.enabled = false;

        control = Control.Pause;

        m_AudioManager.PauseAudio();
    }

    //todo:重新开关物体时，动画会被重置的

    public void CreatShowInfo(List<ProcessSingle> pg, GameObject go, string frames)
    {
        //初始化字符串数组
        var odList = pg.OrderBy(x => x.Index).ToArray();
        ShowInfoGroup = new string[odList.Length];
        for (int i = 0; i < odList.Length; i++)
            ShowInfoGroup[i] = odList[i].Info;

        float[] rexIt = RexFrameInterval(frames);

        TimeComparesGroup = new AudioTimeCompare[rexIt.Length + 1];

        for (int i = 0; i < rexIt.Length; i++)
        {
            AudioTimeCompare atc = new AudioTimeCompare();

            //添加音频
            DownLoaderMono.Instance.DownLoadAudio(odList[i].AudioUrl, a => atc.Audio = a);

            atc.Interval = rexIt[i];

            TimeComparesGroup[i] = atc;
            //  TimeComparesList.Add(atc);
        }

        var anim = go.GetComponent<Animator>();
        //AnimationClip aniclip = anim.runtimeAnimatorController.animationClips[0];
        saveAnim = anim;
        firstAnimName = saveAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        #region 弃用

        //获取帧数组
        //float[] frameKg = RexFrameKeys(frames);
        //添加animator events
        //AnimationEvent[] es = new AnimationEvent[frameKg.Length];

        //for (int i = 0; i < frameKg.Length; i++)
        //{
        //es[i] = new AnimationEvent
        //{
        //    functionName = "ShowText",
        //    time = frameKg[i]
        //};
        //}

        //aniclip.events = es;

        //var pac = go.gameObject.AddComponent<ProcessAnimContainer>();
        //pac.OnStateChange += ShowingFunc;

        #endregion

        ResetState();
        //赋值第一个info
        textShow.text = InfoQueue.Peek();
        saveAnim.enabled = false;
    }

    /// <summary> 
    /// 这是最主要的执行方法 </summary>
    private void ShowingFunc()
    {
        //全队出列
        if (InfoQueue.Count == 0)
        {
            EndQueueMethod();
            return;
        }

        textShow.text = InfoQueue.Dequeue();

        CheckTime();

        var cp = TimeComparesQueue.Dequeue();

        m_AudioManager.PlayAudio(cp.Audio);
    }

    private void EndQueueMethod()
    {
        control = Control.Stop;
        state = State.Await;

        saveAnim.enabled = false;

        PauseBtn.gameObject.SetActive(false);
        PlayBtn.gameObject.SetActive(true);
    }

    /// <summary> 
    /// 检查动画和音频的同步性 </summary>
    private void CheckTime()
    {
        var cp = TimeComparesQueue.Peek();
        float it = cp.DValue();

        //如果音频时间大于动画时间，则暂停等待
        if (it > 0)
        {
            stopTime = cp.Interval;
            continueTime = cp.Audio.length;
        }
    }

    /// <summary> 
    /// 重置队列 </summary>
    private void ResetState()
    {
        InfoQueue.Clear();
        TimeComparesQueue.Clear();
        for (int i = 0; i < ShowInfoGroup.Length; i++)
            InfoQueue.Enqueue(ShowInfoGroup[i]);

        for (int i = 0; i < TimeComparesGroup.Length; i++)
            TimeComparesQueue.Enqueue(TimeComparesGroup[i]);

        saveAnim.enabled = false;

        saveAnim.Play(firstAnimName, -1, 0f);
        saveAnim.Update(0f);

        elapsedTime = continueTime = stopTime = 0;
    }

    /// <summary> 
    /// 重新识别时 </summary>
    public void OnReFound()
    {
        ResetState();
    }

    /// <summary> 
    /// 切换暂停按钮 </summary>
    public void OnSwichPanel(PracMainSwitch pom)
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (pom == PracMainSwitch.Practice)
            AnimPause();
    }

    private float[] RexFrameKeys(string str)
    {
        //like 18-50等等
        Regex re = new Regex("[0-9]\\d{0,50}-[0-9]\\d{0,50}");
        //分割
        string[] sg = str.Split(',');

        List<float> temp = new List<float>();
        //进行匹配
        foreach (var s in sg)
        {
            if (re.IsMatch(s))
            {
                string rs = re.Match(s).Value; //like 10-55
                var ssss = rs.Split('-'); //取第一个,帧动画开始时

                float st = float.Parse(ssss[0]) / 30;
                temp.Add(st); //动画开始，这个是控制文字播放的
            }
        }

        return temp.ToArray();
    }

    private float[] RexFrameInterval(string str)
    {
        //like 18-50等等
        Regex re = new Regex("[0-9]\\d{0,50}-[0-9]\\d{0,50}");
        //分割
        string[] sg = str.Split(',');

        List<float> temp = new List<float>();
        //进行匹配

        float sp = 0; // savepre
        foreach (var s in sg)
        {
            if (re.IsMatch(s))
            {
                string rs = re.Match(s).Value; //like 10-55
                var ssss = rs.Split('-'); //取第一个,帧动画开始时

                //float st = float.Parse(ssss[0]);
                float ed = float.Parse(ssss[1]);
                float iv = (ed - sp) / 30f /*+ GameData.AudioInterval*/;

                temp.Add(iv); //动画间隔，用以判断是否音乐播放完
                sp = ed;
            }
        }

        return temp.ToArray();
    }
}

public class ProcessAnimContainer : MonoBehaviour
{
    private event Action OnStateChangeHandler;
    public event Action OnStateChange
    {
        add
        {
            OnStateChangeHandler -= value;
            OnStateChangeHandler += value;
        }
        remove { OnStateChangeHandler -= value; }
    }

    /// <summary> 
    /// 通过动画调用 </summary>
    public void ShowText()
    {
        OnStateChangeHandler?.Invoke();
    }
}