using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCreator : CreatorBase, ICreatorHandler<TableUnityData>
{
    private enum ImageType
    {
        Background,
        Dynamic
    }

    [SerializeField] private TablePractice m_TablePractice;
    [SerializeField] private TableTemplate m_TableTemplate;

    //存储创建的游戏物体，以找到再次激活时，到底控制哪个物体
    private List<GameObject> AllCreatedList = new List<GameObject>();

    /// <summary>
    /// 创建图表类型
    /// </summary>
    /// <param name="od">objdata 数据</param>
    /// <param name="tud">ab打包出来的精灵组</param>
    public void CreatComponent(ObjectData od, TableUnityData tud)
    {
        TypeTable tt = (TypeTable) od;

        //注册激活时的方法，防止再次激活时无法再控制物体
        MainGameObject mgo = transform.root.GetComponent<MainGameObject>();
        if (mgo == null)
            throw new NullReferenceException();
        //用来存放模型
        var modelContainer = new GameObject("ModelContainer");
        modelContainer.transform.SetParent(mgo.transform);

        if (tt.CompomentsTitle.Count != 0)
        {
            int listIndex = 0;
            foreach (var t in tt.CompomentsTitle)
            {
                GameObject coGo = Instantiate(m_TableTemplate.gameObject, transform, false);
                TableTemplate col = coGo.GetComponent<TableTemplate>();
                OnSwichPanel += col.OnSwichPanel;
                mgo.OnFound += col.OnReFound;

                //查找sprite
                TableSingle ts = tt.TablesList[listIndex];

                bool isOn = ts.IsOn;
                //申请一个toggle
                ApplyForOneToggle(coGo, t.Value, col.ToggleFunc, isOn);

                StartCoroutine(DownLoadSlefSprites(ts.BackgroundPicUrl, col, ImageType.Background));
                StartCoroutine(DownLoadSlefSprites(ts.DynamicPicUrl, col, ImageType.Dynamic));

                foreach (var go in tud.AnimGameObjects)
                    if (go.name == ts.GameObjectName)
                    {
                        GameObject goo = Instantiate(go);
                        goo.transform.SetParent(modelContainer.transform);

                        AllCreatedList.Add(goo);
                        col.SetBindGameObject(goo, isOn); //绑定游戏物体
                        col.SetAudio(ts.AudioUrl); //绑定音乐
                    }

                //正常情况下，退出foreach时并不会值溢出
                listIndex++;

                coGo.SetActive(isOn);
            }

            mgo.OnFound += () =>
            {
                foreach (var o in AllCreatedList)
                    if (o.activeInHierarchy)
                        ObjectControl.SetTarget(o.transform);
            };

            foreach (var o in AllCreatedList)
                if (o.activeInHierarchy)
                {
                    //默认需要绑定一个旋转物体
                    ObjectControl.SetTarget(o.transform);
                }
        }

        //创建练习
        CreatPractice(tt);
    }

    public override void CreatPractice(ObjectData od)
    {
        var tt = (TypeTable) od;
        if (tt == null)
        {
            Debug.LogError("类型转换失败");
            return;
        }

        //创建练习
        if (tt.m_TablePracticeData != null)
        {
            GameObject ccGo = Instantiate(m_TablePractice.gameObject, transform, false);
            //------加载通用功能
            UIPanelCommon upc = transform.GetComponentInChildren<UIPanelCommon>();
            if (!ReferenceEquals(upc, null))
            {
                upc.SetMianBtnTitle(tt.Name);
                upc.OnPracClick += ObjectControl.Pause;
                upc.OnMainClick += ObjectControl.Continue;

                upc.OnPracClick += OnPracClick;
                upc.OnMainClick += OnMainClick;
            }

            UIPracBottom pbp = ccGo.GetComponent<UIPracBottom>();
            if (!ReferenceEquals(pbp, null))
            {
                //pbp.ShowAnswer(tt.m_TablePracticeData.IsShowAnswer);
                pbp.AddButtonFunc(upc);
            }

            //------通用功能加载完毕

            TablePractice cp = ccGo.GetComponent<TablePractice>();

            List<string> urls = new List<string> {tt.m_TablePracticeData.AnswerPicUrl, tt.m_TablePracticeData.TaskPicUrl};

            cp.CreatPractice(tt.m_TablePracticeData);

            //异步加载图片
            StartCoroutine(DownLoadSprites(urls.ToArray(), tt, pbp));
        }
    }

    /// <summary> 
    /// 加载练习的图 </summary>
    IEnumerator DownLoadSprites(string[] urls, TypeTable tt, UIPracBottom pbp)
    {
        List<Sprite> sps = new List<Sprite>();

        yield return StartCoroutine(StaticClass.DownloadImages(urls, v => sps = v));

        Sprite task = null;
        Sprite ans = null;
        foreach (var sp in sps)
        {
            if (sp.name == tt.m_TablePracticeData.TaskPicName)
                task = sp;
            if (sp.name == tt.m_TablePracticeData.AnswerPicName)
                ans = sp;
        }

        pbp.LoadSprites(ans, task);
    }

    /// <summary> 
    /// 加载动态的图 </summary>
    IEnumerator DownLoadSlefSprites(string url, TableTemplate tt, ImageType it)
    {
        if (string.IsNullOrEmpty(url))
            yield break;

        List<Sprite> sps = new List<Sprite>();
        string[] urls = {url};

        yield return StartCoroutine(StaticClass.DownloadImages(urls, v => sps = v));

        if (it == ImageType.Background && sps != null)
            tt.CreatBgImage(sps[0]);

        if (it == ImageType.Dynamic && sps != null)
            tt.CreatDyImage(sps[0]);
    }
}