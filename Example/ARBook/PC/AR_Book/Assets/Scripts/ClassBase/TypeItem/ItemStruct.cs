using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ItemStruct : MonoBehaviour
{
    #region Introduce使用

    private Dictionary<int, Introduce.Structs> structSortedDic = new Dictionary<int, Introduce.Structs>();
    private Queue<Introduce.Structs> StructQueue = new Queue<Introduce.Structs>();
    private Introduce.Structs prevSave;

    #endregion

    //高亮的字典组
    private Dictionary<Transform, string> DetailColl = new Dictionary<Transform, string>();

    //预制件
    public GameObject TextShowPrefab, Controller;

    //按钮功能
    public Button Hide, Show, All, Detail;

    //中下方展示
    public Text ShowAllText;

    private StringBuilder stringBuilder = new StringBuilder();

    private CameraChange m_CameraChange;

    private CameraHighLighting m_CameraHighLighting;

    private GameObject mainGoRef, saveChanging;

    private AudioManager m_AudioManager; //播放声音

    private bool isHiding;

    private bool isActiveThis; //切换练习题时需要判断

    //控制显示和隐藏
    private event Action<bool> OnToggleHandler;

    public event Action<bool> OnToggle
    {
        add
        {
            OnToggleHandler -= value;
            OnToggleHandler += value;
        }
        remove { OnToggleHandler -= value; }
    }

    void Init()
    {
        Hide.onClick.AddListener(ClickHide);
        Show.onClick.AddListener(ClickShow);
        Detail.onClick.AddListener(ClickDetail);
        All.onClick.AddListener(ClickAll);
        m_CameraChange = CameraChange.Instance;
        m_AudioManager = AudioManager.Instance;
    }

    /// <summary> 
    /// 构建显示的结构UI </summary>
    public void CreatStructUI(StructSingle[] os, Transform thisTrans, GameObject mainGo)
    {
        Init();

        Dictionary<int, string> sortDic = new Dictionary<int, string>();
        var tempDic = new Dictionary<int, Introduce.Structs>();

        //需要确保该组件存在
        m_CameraHighLighting = GetComponent<CameraHighLighting>();

        DownLoaderMono m_downLoader = DownLoaderMono.Instance;

        foreach (StructSingle ss in os)
        {
            var creatModel = mainGo;
            //寻找绑定的物体
            var findGo = StaticClass.RecursiveFindChild(creatModel.transform, ss.GameObjectName);

            if (ReferenceEquals(findGo, null))
                continue;

            GameObject go = Instantiate(TextShowPrefab);
            go.transform.SetParent(thisTrans);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            //成功创建后
            UITextSingleCreator ts = go.GetComponent<UITextSingleCreator>();
            if (!ReferenceEquals(ts, null))
            {
                ts.Init(ss, findGo);

                //注册自身隐藏的方法
                OnToggle += ts.ActiveFunc;
                sortDic.Add(ss.Index, ss.Info);

                Introduce.Structs it = new Introduce.Structs
                {
                    UITextSingle = ts,
                    Info = ss.CombinedStr
                };

                m_downLoader.DownLoadAudio(ss.AudioUrl, a => it.SetAudio(a));

                tempDic.Add(ss.Index, it);
            }

            //标记为高亮物体
            HighLighter hl = findGo.AddComponent<HighLighter>();
            //将高亮方法注册进去
            m_CameraHighLighting.OnHighLight += hl.SwitchHighLighting;

            DetailColl.Add(findGo.transform, ss.CombinedStr);
        }

        structSortedDic = tempDic.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
        IntroduceReset();

        //排序
        foreach (var c in sortDic.OrderBy(o => o.Key))
            stringBuilder.Append(c.Key).Append("-").Append(c.Value).Append(";");
        

        //for (int i = 1; i < sortDic.Count + 1; i++)
        //    stringBuilder.Append(i).Append("-").Append(sortDic[i]).Append(";");

        //出去末尾符号
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        ShowAllText.text = stringBuilder.ToString();

        //初始化后隐藏掉
        OnToggleHandler?.Invoke(false);
        Controller.SetActive(false);
    }

    /// <summary> 
    /// 介绍的动画 </summary>
    public Introduce.Structs IntroduceShow()
    {
        if (StructQueue.Count < 0)
            return null;
        //全出列以后
        if (StructQueue.Count == 0)
        {
            prevSave = null;

            foreach (var s in structSortedDic)
                s.Value.UITextSingle.ResetEffect(false);
            return null;
        }

        //上一个的动画
        prevSave?.UITextSingle.AlphaEffect();

        Introduce.Structs iso = StructQueue.Dequeue();
        prevSave = iso;
        iso.UITextSingle.gameObject.SetActive(true);

        m_AudioManager.PlayAudio(iso.Audio);

        return iso;
    }

    /// <summary> 
    /// 重置尺寸的队列 </summary>
    public void IntroduceReset()
    {
        StructQueue.Clear();
        foreach (var sd in structSortedDic)
        {
            sd.Value.UITextSingle.ResetEffect(true);
            StructQueue.Enqueue(sd.Value);
        }

        prevSave = null;
    }

    /// <summary> 
    /// 检测是否点击到了可以交互放大的细节 </summary>
    public void GetHighlightInfo(Transform target)
    {
        string ns;
        if (DetailColl.TryGetValue(target, out ns))
        {
            Detail.interactable = true;
            ShowAllText.text = ns;
            ClickHide();

            saveChanging = target.gameObject;
        }
    }

    /// <summary> 
    /// 整个标签开关时会调用 </summary>
    public void ToggleFunc(bool b)
    {
        OnToggleHandler?.Invoke(b);
        isActiveThis = b;
        if (b)
        {
            ShowAllText.text = stringBuilder.ToString();
            isHiding = false;
            //只有在结构时 才需要该方法
            m_CameraHighLighting.enabled = true;
        }
        else
        {
            ClickAll();
            //只有在结构时 才需要该方法
            m_CameraHighLighting.enabled = false;
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
    /// 点击隐藏标签 </summary>
    public void ClickHide()
    {
        if (isHiding)
            return;

        OnToggleHandler?.Invoke(false);

        Show.gameObject.SetActive(true);
        Hide.gameObject.SetActive(false);
        isHiding = true;
    }

    /// <summary> 
    /// 点击显示标签 </summary>
    public void ClickShow()
    {
        OnToggleHandler?.Invoke(true);

        Show.gameObject.SetActive(false);
        Hide.gameObject.SetActive(true);
        ClickAll();
        ShowAllText.text = stringBuilder.ToString();
        isHiding = false;
    }

    /// <summary> 
    /// 点击细节 </summary>
    private void ClickDetail()
    {
        if (saveChanging == null)
            return;

        All.interactable = true;
        Detail.interactable = Show.interactable = false;

        //更改相机层级
        m_CameraChange.ChangeToSingle(saveChanging.transform);

        //重置高亮
        m_CameraHighLighting.HighLightPause();
    }

    /// <summary> 
    /// 点击显示全部 </summary>
    private void ClickAll()
    {
        Show.interactable = true;

        All.interactable = Detail.interactable = false;

        m_CameraChange.ChangeToDefault();

        m_CameraHighLighting.HighLightStart();

        ShowAllText.text = null;

        if (!ReferenceEquals(saveChanging, null))
            saveChanging = null;
    }
}