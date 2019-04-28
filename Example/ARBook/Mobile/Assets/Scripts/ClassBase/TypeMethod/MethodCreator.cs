using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MethodCreator : CreatorBase, ICreatorHandler<GameObject[]>
{
    [SerializeField] private MethodTemplate m_MethodTemplate;
    [SerializeField] private MethodPractice m_MethodPractice;

    //存储创建的游戏物体，以找到再次激活时，到底控制哪个物体
    private List<GameObject> AllCreatedList = new List<GameObject>();

    public void CreatComponent(ObjectData od, GameObject[] gog)
    {
        var tm = (TypeMethod) od;
        if (tm == null)
        {
            Debug.LogError("类型转换失败");
            return;
        }

        //排序
        var orderList = tm.MethodList.OrderBy(x => x.Index).ToList();

        for (int i = 0; i < orderList.Count; i++)
        {
            GameObject coGo = Instantiate(m_MethodTemplate.gameObject, transform, false);

            //注册激活时的方法，防止再次激活时无法再控制物体
            MainGameObject mgo = transform.root.GetComponent<MainGameObject>();
            if (mgo == null)
            {
                Debug.LogError("找不到根MainGameObject类");
                return;
            }

            mgo.OnFound += () =>
            {
                foreach (var o in AllCreatedList)
                    if (o.activeInHierarchy)
                        ObjectControl.SetTarget(o.transform);
            };
            //激活结束

            MethodTemplate col = coGo.GetComponent<MethodTemplate>();

            OnSwichPanel += col.OnSwichPanel;
            mgo.OnFound += col.OnReFound; //重新识别时

            //申请一个toggle
            ApplyForOneToggle(coGo, orderList[i].Title, col.ToggleFunc, i == 0);

            //查找gameobject
            MethodSigle ts = orderList[i];
            foreach (var go in gog)
            {
                if (go.name == ts.GameObjectName)
                {
                    GameObject ins = Instantiate(go, coGo.transform, true); //实例化游戏物体

                    AllCreatedList.Add(ins);

                    col.CreatMethod(ts, ins);
                }
            }

            ObjectControl.SetTarget(AllCreatedList[0].transform);

            coGo.SetActive(i == 0);
        }

        //创建练习
        CreatPractice(tm);
    }

    /// <summary> 
    /// 创建练习 </summary>
    public override void CreatPractice(ObjectData od)
    {
        var tm = (TypeMethod) od;
        if (tm == null)
        {
            Debug.LogError("类型转换失败");
            return;
        }

        //创建练习
        if (tm.m_MethodPracticeData != null)
        {
            GameObject ccGo = Instantiate(m_MethodPractice.gameObject, transform, false);

            //------加载通用功能
            UIPanelCommon upc = transform.GetComponentInChildren<UIPanelCommon>();
            if (!ReferenceEquals(upc, null))
            {
                upc.SetMianBtnTitle(tm.Name);

                upc.OnPracClick += ObjectControl.Pause;
                upc.OnMainClick += ObjectControl.Continue;

                upc.OnPracClick += OnPracClick;
                upc.OnMainClick += OnMainClick;
            }

            UIPracBottom uib = ccGo.GetComponent<UIPracBottom>();
            if (!ReferenceEquals(uib, null))
            {
                //uib.ShowAnswer(tm.m_MethodPracticeData.IsShowAnswer);
                uib.AddButtonFunc(upc);
            }

            //------通用功能加载完毕
            MethodPractice cp = ccGo.GetComponent<MethodPractice>();

            cp.CreatPractice(tm.m_MethodPracticeData);

            //异步加载图片
            StartCoroutine(DownLoadSprites(tm.m_MethodPracticeData, cp, uib));
        }
    }

    /// <summary> 
    /// 加载练习的图 </summary>
    IEnumerator DownLoadSprites(MethodPracticeData mpd, MethodPractice mp, UIPracBottom uib)
    {
        List<string> urls = new List<string>();
        foreach (var u in mpd.PracticeUrls)
        {
            urls.Add(u.Value);
        }

        //var urls = mpd.practiceUrls;
        urls.Add(mpd.AnswerPicUrl);
        urls.Add(mpd.TaskPicUrl);

        List<Sprite> sps = new List<Sprite>();

        yield return StartCoroutine(StaticClass.DownloadImages(urls.ToArray(), v => sps = v));

        Sprite answer = null, task = null;

        foreach (var s in sps)
        {
            if (s.name == mpd.TaskPicName)
                answer = s;

            if (s.name == mpd.AnswerPicName)
                task = s;
        }

        sps.Remove(answer);
        sps.Remove(task);

        //加载选择题
        mp.LoadSprites(sps);
        //加载答案等图
        uib.LoadSprites(task, answer);
    }
}