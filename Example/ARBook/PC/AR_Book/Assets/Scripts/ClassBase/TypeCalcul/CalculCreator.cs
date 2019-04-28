using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculCreator : CreatorBase, ICreatorHandler<GameObject>
{
    [SerializeField] private CalculOverall m_OverallComponent;
    [SerializeField] private Calculation m_Calculation;
    [SerializeField] private CalculPractice m_CalculPractice;

    //[SerializeField] private Transform togglesParent;
    //[SerializeField] private Transform togglePrefab;
    //[SerializeField] private ToggleGroup toggleGroup;

    ///// <summary> 
    ///// 申请一个toggle </summary>
    //private void ApplyForOneToggle(GameObject fo, string toggleName, Action<bool> actFunc = null, bool isOn = false)
    //{
    //    var to = Instantiate(togglePrefab.gameObject);
    //    to.transform.SetParent(togglesParent);
    //    to.transform.localPosition = to.transform.localEulerAngles = Vector3.zero;
    //    to.transform.localScale = Vector3.one;

    //    Toggle tg = to.GetComponent<Toggle>();
    //    if (!ReferenceEquals(tg, null))
    //    {
    //        //加入事件
    //        tg.group = toggleGroup;

    //        tg.onValueChanged.AddListener(ison =>
    //        {
    //            fo.SetActive(ison);
    //            actFunc?.Invoke(ison);
    //        });

    //        tg.isOn = isOn;

    //        //更改名字
    //        Text tgt = tg.GetComponentInChildren<Text>();
    //        if (!ReferenceEquals(tgt, null))
    //        {
    //            tgt.text = toggleName;
    //        }
    //    }

    //    to.SetActive(false);
    //    to.SetActive(true);
    //}

    /// <summary> 
    /// 加载所需要的组件 </summary>
    public void CreatComponent(ObjectData od, GameObject go)
    {
        var tcl = (TypeCalculation) od;
        if (tcl == null)
        {
            Debug.LogError("类型转换失败");
            return;
        }

        //实例化游戏物体
        var ing = Instantiate(go, transform.parent);

        ObjectControl.SetTarget(ing.transform);

        //注册激活时的方法，防止再次激活时无法再控制物体
        MainGameObject mgo = transform.root.GetComponent<MainGameObject>(); //Main 用以处理再次识别时的各种方法
        if (mgo == null)
        {
            Debug.LogError("找不到根MainGameObject类");
            return;
        }

        mgo.OnFound += () => ObjectControl.SetTarget(ing.transform);
        //激活结束

        if (true) //创建概览
        {
            GameObject coGo = Instantiate(m_OverallComponent.gameObject, transform, false);

            CalculOverall col = coGo.GetComponent<CalculOverall>();

            ApplyForOneToggle(coGo, tcl.CompomentsTitle[TypeData.Calcul.OverallInfo], col.ToggleFunc, true);

            col.CreatOverallUI(tcl.CalculOverallInfo);

            mgo.OnFound += col.OnReFound;
        }

        if (tcl.CalcultionsList.Count != 0)
        {
            GameObject ccGo = Instantiate(m_Calculation.gameObject, transform, false);

            Calculation ccl = ccGo.GetComponent<Calculation>();

            ApplyForOneToggle(ccGo, tcl.CompomentsTitle[TypeData.Calcul.Calculations], ccl.ToggleFunc);

            //构建单个计算类型
            ccl.CreatCalculUI(tcl, ing);
            ccGo.SetActive(false);

            OnSwichPanel += ccl.OnSwichPanel;
            mgo.OnFound += ccl.OnReFound;
        }

        //------加载通用功能
        UIPanelCommon upc = transform.GetComponentInChildren<UIPanelCommon>();
        if (ReferenceEquals(upc, null))
        {
            Debug.LogError("Not Found UIPanelCommon");
            return;
        }

        upc.OnPracClick += ObjectControl.Pause;
        upc.OnMainClick += ObjectControl.Continue;

        upc.OnPracClick += OnPracClick;
        upc.OnMainClick += OnMainClick;

        //创建练习
        CreatPractice(tcl);
    }

    /// <summary> 
    /// 创建练习题 </summary>
    public override void CreatPractice(ObjectData od)
    {
        var tcl = (TypeCalculation) od;
        if (tcl == null)
        {
            Debug.LogError("类型转换失败");
            return;
        }

        //创建练习
        if (tcl.m_CalculPracticeData != null)
        {
            GameObject ccGo = Instantiate(m_CalculPractice.gameObject, transform, false);

            //------加载通用功能
            UIPanelCommon upc = transform.GetComponentInChildren<UIPanelCommon>();
            upc?.SetMianBtnTitle(tcl.Name);

            UIPracBottom pbp = ccGo.GetComponent<UIPracBottom>();
            if (!ReferenceEquals(pbp, null))
            {
                //pbp.ShowAnswer(tcl.m_CalculPracticeData.IsShowAnswer);
                pbp.AddButtonFunc(upc);
            }
            //------通用功能加载完毕

            CalculPractice cp = ccGo.GetComponent<CalculPractice>();

            List<string> urls = new List<string> {tcl.m_CalculPracticeData.AnswerPicUrl, tcl.m_CalculPracticeData.TaskPicUrl};

            cp.CreatPractice(tcl.m_CalculPracticeData);

            //异步加载图片
            UIPracBottom pb = ccGo.GetComponent<UIPracBottom>();
            if (!ReferenceEquals(pb, null))
                StartCoroutine(DownLoadSprites(urls.ToArray(), tcl, pb));
        }
    }

    /// <summary> 
    /// 加载练习的图 </summary>
    IEnumerator DownLoadSprites(string[] urls, TypeCalculation tcl, UIPracBottom pbp)
    {
        List<Sprite> sps = new List<Sprite>();

        yield return StartCoroutine(StaticClass.DownloadImages(urls, v => sps = v));

        Sprite task = null;
        Sprite ans = null;
        foreach (var sp in sps)
        {
            if (sp.name == tcl.m_CalculPracticeData.TaskPicName)
                task = sp;
            if (sp.name == tcl.m_CalculPracticeData.AnswerPicName)
                ans = sp;
        }

        pbp.LoadSprites(ans, task);
    }
}