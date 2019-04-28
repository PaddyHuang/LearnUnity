using System;
using System.Collections;
using EasyAR;
using UnityEngine;

/// <summary> 
/// 所有类型的创建入口 </summary>
public class TypeFactory : MonoBehaviour
{
    #region Mono-Singleton

    private static TypeFactory instance;
    public static TypeFactory Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<TypeFactory>();
            return instance;
        }
    }

    #endregion

    private string commonUrl;

    public ImageTrackerBehaviour Tracker;

    public GameObject CanvasItem, CanvasCul, CanvasMethod, CanvasTable, CanvasProcess;

    private UIMainCanvas m_UIMainCanvas;

    void Awake()
    {
        TestLoad();
    }

    void Start()
    {
        m_UIMainCanvas = UIMainCanvas.Instance;
    }

    private void TestLoad()
    {
        StartCoroutine(LoadCommonAssets());

#if UNITY_EDITOR

        //string path = Application.streamingAssetsPath + "/content_Calcul.json";
        //string path = Application.streamingAssetsPath + "/content_Item.json";
        //string path = Application.streamingAssetsPath + "/content_Method.json";
        //string path = Application.streamingAssetsPath + "/content_Table.json";
        //string path = Application.streamingAssetsPath + "/content_Process.json";

        //StartCoroutine(AndriodLoadLocal(path));
#endif
    }

    public void Post(string str)
    {
        //StartCoroutine(AndriodPostServer(str));
        StartCoroutine(AndriodPostServerNew(str));
    }

    private GameObject CreatMainGameObject(ObjectData od, GameObject canvaPre)
    {
        //创建一个总物体
        GameObject topPa = new GameObject(od.Name);
        topPa.AddComponent<MainGameObject>();

        //设置全局静态物体
        SaveCreatedObject(od.CNID, topPa);

        //设置创建出来的主物体的父物体
        GameObject co = Instantiate(canvaPre);
        co.transform.SetParent(topPa.transform);

        return co;
    }

    private void SaveCreatedObject(string str, GameObject topPa)
    {
        var sg = str.Split(',');

        foreach (var s in sg)
        {
            GameDataMono.Instance.SaveCreatedObject(s, topPa);
        }
    }

    /// <summary>
    /// 创建主类型总入口
    /// </summary>
    /// <typeparam name="TC">Creator类型</typeparam>
    /// <typeparam name="TT">Ab包资源出来的类型</typeparam>
    /// <param name="od">objectdata 参数</param>
    /// <param name="objType">Ab包资源 参数</param>
    /// <param name="co">Canvas 参数</param>
    private void CreatTypes<TC, TT>(ObjectData od, TT objType, GameObject co) where TC : CreatorBase, ICreatorHandler<TT>
    {
        //实例化Canvas
        TC cr = co.GetComponent<TC>();

        if (cr == null)
            return;
        cr.CreatComponent(od, objType);
    }

    /// <summary> 
    /// 加载通用材质等 </summary>
    IEnumerator LoadCommonAssets()
    {
        string platform;
#if UNITY_ANDROID
        platform = "MB/";
#endif

#if UNITY_STANDALONE_WIN
        platform = "PC/";
#endif
        commonUrl = Application.streamingAssetsPath + "/AssetBundles/" + platform + "common";
        //commonUrl = Application.streamingAssetsPath + "/AssetBundles/common";
        // WWW getCommon = WWW.LoadFromCacheOrDownload(commonUrl, 1);
        WWW getCommon = new WWW(commonUrl); /* WWW.LoadFromCacheOrDownload(, 1);*/

        yield return getCommon;

        AssetBundle ass = getCommon.assetBundle;
        ass.LoadAllAssets();
        ass.Unload(false);

        getCommon.Dispose();
    }

    /// <summary> 
    /// 安卓解析 </summary>
    IEnumerator AndriodLoadLocal(string path)
    {
        WWW getData = new WWW(path);
        yield return getData;

        string jsdata = System.Text.Encoding.UTF8.GetString(getData.bytes, 3, getData.bytes.Length - 3);
        //暂时测出解析耗时16ms
        ObjectData od = AssetsLoader.LoadServerJsonData(jsdata);

        string abPath = null;

        if (Application.platform == RuntimePlatform.Android)
        {
            abPath = Application.dataPath + od.aburl;
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            abPath = Application.dataPath + od.aburlPC;
        }

        //ab包的地址,等待下载完成ab包

        //WWW abWww = WWW.LoadFromCacheOrDownload(abPath, 4);
        WWW abWww = new WWW(abPath);
        yield return abWww;

        AssetBundle ass = abWww.assetBundle;

        GameObject createdGo;

        TypeEnum t = od.Type;

        switch (t)
        {
            case TypeEnum.JiSuan:
                createdGo = CreatMainGameObject(od, CanvasCul);
                //主体
                GameObject abGo1 = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<CalculCreator, GameObject>(od, abGo1, createdGo);

                //CreatPractice<CalculCreator>(od, cg1);
                break;

            case TypeEnum.WuJian:
                createdGo = CreatMainGameObject(od, CanvasItem);
                //主体
                GameObject abGo = ass.LoadAllAssets<GameObject>()[0];

                CreatTypes<ItemCreator, GameObject>(od, abGo, createdGo);
                break;

            case TypeEnum.FangShi:
                createdGo = CreatMainGameObject(od, CanvasMethod);

                GameObject[] gGroups = ass.LoadAllAssets<GameObject>();
                CreatTypes<MethodCreator, GameObject[]>(od, gGroups, createdGo);

                break;

            case TypeEnum.TuBiao:
                createdGo = CreatMainGameObject(od, CanvasTable);

                //Sprite[] sGroups = ass.LoadAllAssets<Sprite>();
                GameObject[] gog = ass.LoadAllAssets<GameObject>();
                TableUnityData tud = new TableUnityData();
                tud.AnimGameObjects = gog;
                //tud.AnimSprite = sGroups;

                CreatTypes<TableCreator, TableUnityData>(od, tud, createdGo);

                break;

            case TypeEnum.LiuCheng:
                createdGo = CreatMainGameObject(od, CanvasProcess);

                GameObject abGo2 = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<ProcessCreator, GameObject>(od, abGo2, createdGo);
                break;
        }

        //这里是EasyAR的内容,暂时注释掉了
        //ImageTargetBehaviour arScript = abGo.AddComponent<ImageTargetBehaviour>();
        //arScript.Bind(Tracker);
        //arScript.SetupWithImage(dfj.image, StorageType.Assets, dfj.name, new Vector2(dfj.size[0], dfj.size[1]));

        //开始根据json创建物体
        ass.Unload(false);
        abWww.Dispose();
        getData.Dispose();
    }

    /// <summary> 
    /// 安卓解析 </summary>
    IEnumerator AndriodPostServer(string jsonData)
    {
        //暂时测出解析耗时16ms
        ObjectData od = AssetsLoader.LoadServerJsonData(jsonData);

        //ab包的地址,等待下载完成ab包
        //string abPath = od.aburl;

        string abPath = null;

        if (Application.platform == RuntimePlatform.Android)
            abPath = od.aburl;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            abPath = od.aburlPC;

        WWW abWww = new WWW(abPath);

        if (abWww.error != null)
        {
            Debug.Log("AB包下载失败");
            yield break;
        }

        //-----------开始下载ab包
        while (!abWww.isDone)
        {
            m_UIMainCanvas.SetLoadingPercentage(abWww.progress);

            yield return new WaitForEndOfFrame();
        }

        m_UIMainCanvas.SetLoadingPercentage(1);

        //-----------至此Ab包下载完成

        AssetBundle ass = abWww.assetBundle;

        TypeEnum t = od.Type;

        switch (t)
        {
            case TypeEnum.JiSuan:
                GameObject cg1 = CreatMainGameObject(od, CanvasCul);

                //主体
                GameObject abGo1 = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<CalculCreator, GameObject>(od, abGo1, cg1);
                break;

            case TypeEnum.WuJian:
                GameObject cg2 = CreatMainGameObject(od, CanvasItem);

                //主体
                GameObject abGo = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<ItemCreator, GameObject>(od, abGo, cg2);
                break;

            case TypeEnum.FangShi:
                GameObject cg3 = CreatMainGameObject(od, CanvasMethod);

                //主体
                GameObject[] gGroups = ass.LoadAllAssets<GameObject>();
                CreatTypes<MethodCreator, GameObject[]>(od, gGroups, cg3);
                break;

            case TypeEnum.TuBiao:
                GameObject cg4 = CreatMainGameObject(od, CanvasTable);

                //主体
                GameObject[] gog = ass.LoadAllAssets<GameObject>();
                TableUnityData tud = new TableUnityData();
                tud.AnimGameObjects = gog;

                CreatTypes<TableCreator, TableUnityData>(od, tud, cg4);

                break;

            case TypeEnum.LiuCheng:
                GameObject cg5 = CreatMainGameObject(od, CanvasProcess);
                //主体
                GameObject abGo2 = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<ProcessCreator, GameObject>(od, abGo2, cg5);

                break;
        }

        yield return abWww;
        //开始根据json创建物体
        ass.Unload(false);
        abWww.Dispose();
        m_UIMainCanvas.DisableProgressImage();
    }

    IEnumerator AndriodPostServerNew(string jsonData)
    {
        //暂时测出解析耗时16ms
        ObjectData od = AssetsLoader.LoadServerJsonData(jsonData);

        bool isLocal;

        string abPath = null;

        var hasLocalAsset = LocalAssetManager.Instance.GetLocalAsset();
        if (hasLocalAsset != null)
        {
            abPath = hasLocalAsset;
            isLocal = true;
        }
        else
        {
            //string abPath = od.aburl;

            if (Application.platform == RuntimePlatform.Android)
                abPath = od.aburl;

            else if (Application.platform == RuntimePlatform.WindowsPlayer ||
                     (Application.platform == RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.Android))
                abPath = od.aburlPC;

            isLocal = false;
        }

        WWW abWww = new WWW(abPath);

        if (abWww.error != null)
        {
            Debug.Log("AB包下载失败");
            yield break;
        }

        //-----------开始下载ab包
        while (!abWww.isDone)
        {
            m_UIMainCanvas.SetLoadingPercentage(abWww.progress);

            yield return new WaitForEndOfFrame();
        }

        m_UIMainCanvas.SetLoadingPercentage(1);

        //-----------至此Ab包下载完成
        //正式把数据写入本地的json和保存ab包
        if (!isLocal)
            LocalAssetManager.Instance.WriteTempData(abWww.bytes);

        AssetBundle ass = abWww.assetBundle;

        TypeEnum t = od.Type;

        switch (t)
        {
            case TypeEnum.JiSuan:
                GameObject cg1 = CreatMainGameObject(od, CanvasCul);

                //主体
                GameObject abGo1 = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<CalculCreator, GameObject>(od, abGo1, cg1);
                break;

            case TypeEnum.WuJian:
                GameObject cg2 = CreatMainGameObject(od, CanvasItem);

                //主体
                GameObject abGo = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<ItemCreator, GameObject>(od, abGo, cg2);
                break;

            case TypeEnum.FangShi:
                GameObject cg3 = CreatMainGameObject(od, CanvasMethod);

                //主体
                GameObject[] gGroups = ass.LoadAllAssets<GameObject>();
                CreatTypes<MethodCreator, GameObject[]>(od, gGroups, cg3);
                break;

            case TypeEnum.TuBiao:
                GameObject cg4 = CreatMainGameObject(od, CanvasTable);

                //主体
                GameObject[] gog = ass.LoadAllAssets<GameObject>();
                TableUnityData tud = new TableUnityData();
                tud.AnimGameObjects = gog;

                CreatTypes<TableCreator, TableUnityData>(od, tud, cg4);

                break;

            case TypeEnum.LiuCheng:
                GameObject cg5 = CreatMainGameObject(od, CanvasProcess);
                //主体
                GameObject abGo2 = ass.LoadAllAssets<GameObject>()[0];
                CreatTypes<ProcessCreator, GameObject>(od, abGo2, cg5);

                break;
        }

        yield return abWww;
        //开始根据json创建物体
        ass.Unload(false);
        abWww.Dispose();
        m_UIMainCanvas.DisableProgressImage();
    }
}

/// <summary> 
/// 激活物体时调用方法 </summary>
public class MainGameObject : MonoBehaviour
{
    private event Action OnFoundHandler;
    public event Action OnFound
    {
        add
        {
            OnFoundHandler -= value;
            OnFoundHandler += value;
        }
        remove { OnFoundHandler -= value; }
    }

    //private event Action OnLostHandler;
    //public event Action OnLost
    //{
    //    add
    //    {
    //        OnLostHandler -= value;
    //        OnLostHandler += value;
    //    }
    //    remove { OnLostHandler -= value; }
    //}

    public void OnFoundFunc()
    {
        OnFoundHandler?.Invoke();
    }

    //public void OnLostFunc()
    //{
    //    OnLostHandler?.Invoke();
    //}
}