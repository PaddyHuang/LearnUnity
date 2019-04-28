using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : CreatorBase, ICreatorHandler<GameObject>
{
    //还可以继续结合
    private event Action<bool> OnSwitchHandler;
    public event Action<bool> OnSwitch
    {
        add
        {
            OnSwitchHandler -= value;
            OnSwitchHandler += value;
        }
        remove { OnSwitchHandler -= value; }
    }

    public GameObject PublicGameObject; //用来装一些共有物体

    [SerializeField] private ItemIntroduce m_IntroduceComponent;
    [SerializeField] private ItemStruct m_StructComponent;
    [SerializeField] private ItemSizes m_SizesComponent;
    [SerializeField] private ItemMovies m_MoviesComponent;
    [SerializeField] private ItemPractice m_ItemPractice;

    private GameObject gameObjectRef;
    private ObjectControl m_ObjectControl;   

    /// <summary> 
    /// 全局显示 </summary>
    protected override void OnMainClick()
    {
        base.OnMainClick();
        OnSwitchHandler?.Invoke(true);        
    }

    /// <summary> 
    /// 全局隐藏 </summary>
    protected override void OnPracClick()
    {
        base.OnPracClick();
        OnSwitchHandler?.Invoke(false);
    }

    public void CreatComponent(ObjectData od, GameObject mainGo)
    {
        //类型转换
        TypeItem ti = od as TypeItem;
        if (ti == null)
            return;

        //实例化游戏物体
        GameObject ins = Instantiate(mainGo, transform.parent);
        gameObjectRef = ins;

        //------加载通用功能
        UIPanelCommon uip = GetComponentInChildren<UIPanelCommon>();

        if (ReferenceEquals(uip, null))
        {
            Debug.LogError("Not Found UIPanelCommon");
            return;
        }
        
        uip.OnPracClick += OnPracClick;
        uip.OnMainClick += OnMainClick;

        //注册激活时的方法，防止再次激活时无法再控制物体
        MainGameObject mgo = transform.root.GetComponent<MainGameObject>();
        if (mgo == null)
        {
            Debug.LogError("找不到根MainGameObject类");
            return;
        }

        mgo.OnFound += () => ObjectControl.SetTarget(ins.transform);

        //激活结束
        ObjectControl.SetTarget(ins.transform);

        //toggle方法添加
        OnSwitch += toggleGroup.gameObject.SetActive;       

        ItemIntroduce ii;
        //介绍
        if (true)
        {
            GameObject introGo = Instantiate(m_IntroduceComponent.gameObject, transform, false);

            ii = introGo.GetComponent<ItemIntroduce>();

            ApplyForOneToggle(ii.Controller, ti.CompomentsTitle[TypeData.Item.Introduce], ii.ToggleFunc, true);

            OnSwitch += introGo.SetActive;
            
            OnSwitch += ii.SwitchPractice;
            //构建介绍
            ii.CreatIntroduceUI(ti.m_Introduce);

            OnSwichPanel += ii.OnSwichPanel;

            mgo.OnFound += ii.OnReFound;
        }

        //结构
        if (ti.StructsList.Count != 0)
        {
            GameObject structGo = Instantiate(m_StructComponent.gameObject, transform, false);

            ItemStruct iss = structGo.GetComponent<ItemStruct>();

            ApplyForOneToggle(iss.Controller, ti.CompomentsTitle[TypeData.Item.Structs], iss.ToggleFunc);

            OnSwitch += structGo.SetActive;

            OnSwitch += iss.SwitchPractice;

            //构建结构
            iss.CreatStructUI(ti.StructsList.ToArray(), PublicGameObject.transform, ins);
            ii.SetStruct(iss);                       
        }

        //尺寸
        if (ti.SizesList.Count != 0)
        {
            GameObject sizeGo = Instantiate(m_SizesComponent.gameObject, transform, false);

            ItemSizes iss = sizeGo.GetComponent<ItemSizes>();

            //申请一个toggle
            ApplyForOneToggle(iss.Controller, ti.CompomentsTitle[TypeData.Item.Sizes], iss.ToggleFunc);

            OnSwitch += sizeGo.SetActive;

            OnSwitch += iss.SwitchPractice;

            //构建尺寸
            iss.CreatSizesUI(ti.SizesList.ToArray(), ins);
            sizeGo.SetActive(false);
            ii.SetSizes(iss);
        }

        //视频
        if (ti.VideoList.Count != 0)
        {
            GameObject movieGo = Instantiate(m_MoviesComponent.gameObject, transform, false);

            ItemMovies im = movieGo.GetComponent<ItemMovies>();
            //申请一个toggle,这里是用特殊的物体来控制显示
            ApplyForOneToggle(im.Controller, ti.CompomentsTitle[TypeData.Item.Video], im.ToggleFunc);

            OnSwitch += im.SwitchPractice;
            //构建电影
            im.CreatMovie(ti.VideoList[0]);
        }
        
        //练习
        CreatPractice(ti);
    }

    /// <summary> 
    /// 创建练习题 </summary>
    public override void CreatPractice(ObjectData od)
    {
        //类型转换
        var ti = od as TypeItem;

        if (ti?.m_ItemPracticeData != null)
        {
            GameObject ccGo = Instantiate(m_ItemPractice.gameObject, transform, false);

            //------加载通用功能
            UIPanelCommon upc = transform.GetComponentInChildren<UIPanelCommon>();
            if (ReferenceEquals(upc, null))
            {
                Debug.LogError("Not Found UIPanelCommon");
                return;
            }

            upc.SetMianBtnTitle(ti.Name);

            UIPracBottom pbp = ccGo.GetComponent<UIPracBottom>();
            if (!ReferenceEquals(pbp, null))
            {
                //pbp.ShowAnswer(ti.m_ItemPracticeData.IsShowAnswer);
                pbp.AddButtonFunc(upc);
            }
            //------通用功能加载完毕

            ItemPractice ip = ccGo.GetComponent<ItemPractice>();

            ip.CreatPractice(ti.m_ItemPracticeData, gameObjectRef /*, sp*/);
            //异步加载图片
            StartCoroutine(DownLoadSprites(new[] {ti.m_ItemPracticeData.AnswerImageUrl}, pbp));
        }
    }

    /// <summary> 
    /// 加载练习的图 </summary>
    IEnumerator DownLoadSprites(string[] urls, UIPracBottom pbp)
    {
        List<Sprite> sps = new List<Sprite>();

        yield return StartCoroutine(StaticClass.DownloadImages(urls, v => sps = v));
        //todo:错误码
        if (sps.Count < 1)
            yield break;

        //只有一张图
        pbp.LoadSprites(sps[0]);
    }
}