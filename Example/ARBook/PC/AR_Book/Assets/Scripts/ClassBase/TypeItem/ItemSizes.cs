using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ItemSizes : MonoBehaviour
{
    public GameObject LinePrefab, Controller;
    public Text ShowAllInfo;

    private readonly Dictionary<int, Introduce.Sizes> SizesColl = new Dictionary<int, Introduce.Sizes>();
    private Queue<Introduce.Sizes> SizesQueue = new Queue<Introduce.Sizes>();

    private StringBuilder stringBuilder = new StringBuilder();
    private Introduce.Sizes prevSave;

    private AudioManager m_AudioManager; //播放声音

    private bool isActiveThis; //切换练习题时需要判断

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
        m_AudioManager = AudioManager.Instance;
    }

    public void CreatSizesUI(SizeSingle[] ss, GameObject mainGo)
    {
        Dictionary<int, string> sortDic = new Dictionary<int, string>();

        DownLoaderMono m_downLoader = DownLoaderMono.Instance;

        Init();

        foreach (var s in ss)
        {
            GameObject gf = StaticClass.RecursiveFindChild(mainGo.transform, s.FromGoName) /*GameObject.Find(s.FromGoName)*/;
            GameObject tf = StaticClass.RecursiveFindChild(mainGo.transform, s.ToGoName) /*GameObject.Find(s.ToGoName)*/;
            if (ReferenceEquals(gf, null) || ReferenceEquals(tf, null))
                continue;

            //此处设置父物体会影响缩放值
            GameObject lgo = Instantiate(LinePrefab, Vector3.zero, Quaternion.identity);

            UISizeSingleCreator uiSizes = lgo.GetComponentInChildren<UISizeSingleCreator>();
            if (!ReferenceEquals(uiSizes, null))
            {
                uiSizes.SetSizeLine(gf.transform, tf.transform, s.Length);
                //注册隐藏和显示的方法
                OnToggle += uiSizes.ActOrHideLine;
            }

            //绑定在父物体上
            lgo.transform.SetParent(gf.transform);

            string cmob = s.ObjcetName + " " + s.Length;
            sortDic.Add(s.Index, cmob);
            //Intro
            Introduce.Sizes iss = new Introduce.Sizes
            {
                Info = cmob + "mm",
                UISizeSingle = uiSizes
            };
            m_downLoader.DownLoadAudio(s.AudioUrl, iss.SetAudio);

            SizesColl.Add(s.Index, iss);
        }

        IntroduceReset();
        //排序
        for (int i = 1; i < sortDic.Count + 1; i++)
            stringBuilder.Append(i).Append("-").Append(sortDic[i]).Append("mm;");

        //除去末尾符号
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        ShowAllInfo.text = stringBuilder.ToString();

        OnToggleHandler?.Invoke(false);
        Controller.SetActive(false);
    }

    /// <summary> 
    /// 开始动画 </summary>
    public Introduce.Sizes IntroduceShow()
    {
        if (SizesQueue.Count < 0)
            return null;

        if (SizesQueue.Count == 0)
        {
            prevSave = null;

            foreach (var s in SizesColl)
                s.Value.UISizeSingle.ResetEffect(false);
            return null;
        }

        prevSave?.UISizeSingle.AlphaEffect();

        Introduce.Sizes iso = SizesQueue.Dequeue();
        prevSave = iso;
        iso.UISizeSingle.gameObject.SetActive(true);
        m_AudioManager.PlayAudio(iso.Audio);

        return iso;
    }

    /// <summary> 
    /// 重置结构的队列 </summary>
    public void IntroduceReset()
    {
        SizesQueue.Clear();
        foreach (var sd in SizesColl)
        {
            sd.Value.UISizeSingle.ResetEffect(true);
            SizesQueue.Enqueue(sd.Value);
        }

        prevSave = null;
    }

    public void ToggleFunc(bool b)
    {
        OnToggleHandler?.Invoke(b);
        isActiveThis = b;
    }

    public void SwitchPractice(bool isthis)
    {
        if (isActiveThis)
        {
            ToggleFunc(isthis);
            isActiveThis = true;
        }
    }


}