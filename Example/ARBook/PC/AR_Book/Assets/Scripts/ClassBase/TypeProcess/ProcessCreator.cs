using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessCreator : CreatorBase, ICreatorHandler<GameObject>
{
    //private event Action<PracMainSwitch> OnSwichPanelHandler;
    //public event Action<PracMainSwitch> OnSwichPanel
    //{
    //    add
    //    {
    //        OnSwichPanelHandler -= value;
    //        OnSwichPanelHandler += value;
    //    }
    //    remove { OnSwichPanelHandler -= value; }
    //}

    [SerializeField] private ProcessMain m_ProcessMain;
    [SerializeField] private ProcessPractice m_ProcessPractice;

    public void CreatComponent(ObjectData od, GameObject go)
    {
        TypeProcess tp = (TypeProcess) od;
        if (tp == null)
        {
            Debug.LogError("类型转换失败");
            return;
        }

        //transform.parent为根物体
        GameObject aGo = Instantiate(go.gameObject, transform.parent);

        //注册激活时的方法，防止再次激活时无法再控制物体
        MainGameObject mgo = transform.root.GetComponent<MainGameObject>();
        if (mgo == null)
        {
            Debug.LogError("找不到根MainGameObject类");
            return;
        }

        mgo.OnFound += () => ObjectControl.SetTarget(aGo.transform);
        //激活结束

        GameObject coGo = Instantiate(m_ProcessMain.gameObject, transform, false);
        ProcessMain pm = coGo.GetComponent<ProcessMain>();
        OnSwichPanel += pm.OnSwichPanel;
        mgo.OnFound += pm.OnReFound;

        //初始化时先激活一次
        ObjectControl.SetTarget(aGo.transform);

        pm.CreatShowInfo(tp.PsList, aGo, tp.Frames);

        CreatPractice(tp);
    }

    public override void CreatPractice(ObjectData od)
    {
        //类型转换
        var tp = od as TypeProcess;

        if (tp?.m_ProcessPracticeData != null)
        {
            GameObject ccGo = Instantiate(m_ProcessPractice.gameObject, transform, false);

            //------加载通用功能
            UIPanelCommon upc = transform.GetComponentInChildren<UIPanelCommon>();
            if (ReferenceEquals(upc, null))
            {
                Debug.LogError("Not Found UIPanelCommon");
                return;
            }

            upc.SetMianBtnTitle(tp.Name);
            upc.OnPracClick += ObjectControl.Pause;
            upc.OnMainClick += ObjectControl.Continue;

            UIPracBottom pbp = ccGo.GetComponent<UIPracBottom>();
            if (!ReferenceEquals(pbp, null))
            {
                //pbp.ShowAnswer(tp.m_ProcessPracticeData.IsShowAnswer);
                pbp.AddButtonFunc(upc);
            }
            //------通用功能加载完毕

            ProcessPractice pp = ccGo.GetComponent<ProcessPractice>();
            pp.CreatPractice(tp.m_ProcessPracticeData);

            //特有方法
            upc.OnMainClick += OnMainClick;
            upc.OnPracClick += OnPracClick;

            //异步加载图片
            StartCoroutine(DownLoadSprites(tp.m_ProcessPracticeData, pbp));
        }
    }

    /// <summary> 
    /// 加载练习的图 </summary>
    IEnumerator DownLoadSprites(ProcessPracticeData ppd, UIPracBottom pbp)
    {
        List<string> urls = new List<string> {ppd.AnswerPicUrl, ppd.TaskPicUrl};

        List<Sprite> sps = new List<Sprite>();

        yield return StartCoroutine(StaticClass.DownloadImages(urls.ToArray(), v => sps = v));

        Sprite answer = null, task = null;

        foreach (var s in sps)
        {
            if (s.name == ppd.TaskPicName)
                answer = s;

            if (s.name == ppd.AnswerPicName)
                task = s;
        }

        //加载图片
        pbp.LoadSprites(task, answer);
    }
}