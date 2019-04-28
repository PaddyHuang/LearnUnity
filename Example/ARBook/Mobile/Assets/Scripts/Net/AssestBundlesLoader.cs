using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EasyAR;
using UnityEngine.UI;

[Serializable]
public class JsonDataPa
{
    public List<DataFromJson> ImagesList = new List<DataFromJson>();
}

[Serializable]
public class DataFromJson
{
    public string qsId;
    public string ocrId;
    public string image;
    public string name;
    public float[] size;
    public string abName;
    public string pfName;
}

public class AssestBundlesLoader : MonoBehaviour
{
    public Text TTTTTTTT;
    public Text TTTTTTTT2;

    public ImageTrackerBehaviour Tracker;

    private Dictionary<string, DataFromJson> JsonDictionary = new Dictionary<string, DataFromJson>();

    private string testOcrId = "pic2-14";

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        StartCoroutine(AndroidLoad());
#endif
#if UNITY_EDITOR_WIN
        StartCoroutine(WinEditorLoad());
#endif
    }

    private DataFromJson LoadDataWithOcrId(string ocrId)
    {
        DataFromJson jf;
        if (JsonDictionary.TryGetValue(ocrId, out jf))
            return jf;

        return null;
    }

    IEnumerator WinEditorLoad()
    {
        //读取文件  
        string path = Application.streamingAssetsPath + "/targets.json";

        StreamReader sr = new StreamReader(path);
        var jsonStr = sr.ReadToEnd();
        sr.Close();
        yield return null;

        JsonDataPa dd = JsonUtility.FromJson<JsonDataPa>(jsonStr);

        foreach (var js in dd.ImagesList)
            JsonDictionary.Add(js.ocrId, js);

        DataFromJson dfj = LoadDataWithOcrId(testOcrId);
        if (dfj == null)
        {
            TTTTTTTT.text = "dfj is null" + JsonDictionary.Count;
            yield break;
        }
        //至此预读取完成
        //开始加载
        string abPath;

        abPath = "file://" + Application.dataPath + "/AssetsBundles/" + dfj.abName;
        TTTTTTTT.text = "WINDOWS";

        WWW cubewww = new WWW(abPath);
        yield return cubewww;

        GameObject getGo = Instantiate(cubewww.assetBundle.LoadAsset<GameObject>(dfj.pfName));

        ImageTargetBehaviour arScript = getGo.AddComponent<ImageTargetBehaviour>();

        arScript.Bind(Tracker);

        arScript.SetupWithImage(dfj.image, StorageType.Assets, dfj.name, new Vector2(dfj.size[0], dfj.size[1]));

        getGo.gameObject.SetActive(true);
    }

    /// <summary> 
    /// 安卓读取 </summary>
    IEnumerator AndroidLoad()
    {
        //读取文件  
        string path = Application.streamingAssetsPath + "/targets.json";

        WWW awww = new WWW(path);
        yield return awww;

        //string jsonStr = awww.text;

        JsonDataPa dd = JsonUtility.FromJson<JsonDataPa>(awww.text);

        foreach (var js in dd.ImagesList)
            JsonDictionary.Add(js.ocrId, js);

        //string t1 = "";

        //foreach (var d in JsonDictionary)
        //    t1 = t1 + d.Key;

        //TTTTTTTT2.text = JsonDictionary.Count + t1;

        DataFromJson dfj = LoadDataWithOcrId(testOcrId);
        if (dfj == null)
        {
            //TTTTTTTT.text = "dfj is null" + JsonDictionary.Count;
            yield break;
        }

        //至此预读取完成
        //开始加载
        string abPath = Application.streamingAssetsPath + "/AssetBundles/" + dfj.abName;

        WWW cubewww = new WWW(abPath);
        yield return cubewww;

        GameObject getGo = Instantiate(cubewww.assetBundle.LoadAsset<GameObject>(dfj.pfName));
        //TTTTTTTT.text = getGo.name;

        ImageTargetBehaviour arScript = getGo.AddComponent<ImageTargetBehaviour>();

        arScript.Bind(Tracker);

        arScript.SetupWithImage(dfj.image, StorageType.Assets, dfj.name, new Vector2(dfj.size[0], dfj.size[1]));

        getGo.gameObject.SetActive(true);
    }
}

/// <summary> 
/// 整体备份 </summary>
//public class AssestBundlesLoader : MonoBehaviour
//{
//    public Text TTTTTTTT;
//    public Text TTTTTTTT2;

//    public ImageTrackerBehaviour Tracker;

//    private Dictionary<string, DataFromJson> JsonDictionary = new Dictionary<string, DataFromJson>();

//    private string testOcrId = "pic2-14";

//    void Start()
//    {
//#if UNITY_ANDROID && !UNITY_EDITOR_WIN
//        StartCoroutine(AndroidLoad());
//#endif
//#if UNITY_EDITOR_WIN
//        StartCoroutine(WinEditorLoad());
//#endif
//    }

//    private DataFromJson LoadDataWithOcrId(string ocrId)
//    {
//        DataFromJson jf;
//        if (JsonDictionary.TryGetValue(ocrId, out jf))
//            return jf;

//        return null;
//    }

//    #region 弃用
//    IEnumerator GetAbPackageFromJson()
//    {
//        DataFromJson dfj = LoadDataWithOcrId(testOcrId);
//        if (dfj == null)
//        {
//            TTTTTTTT.text = "dfj is null" + JsonDictionary.Count;
//            yield break;
//        }

//        //WWW windowswww = new WWW("file://" + Application.dataPath + "/AssetBundles"); //Application.dataPath 当前工程目录
//        //yield return windowswww;
//        string abPath;

//#if UNITY_ANDROID && !UNITY_EDITOR_WIN
//        abPath = Application.streamingAssetsPath + "/AssetBundles/" + dfj.abName;
//        TTTTTTTT.text = "ANDR";
//#endif

//#if UNITY_EDITOR_WIN
//        abPath = "file://" + Application.dataPath + "/AssetBundles/" + dfj.abName;
//        TTTTTTTT.text = "WINDOWS";
//#endif

//        WWW cubewww = new WWW(abPath);
//        yield return cubewww;

//        GameObject getGo = Instantiate(cubewww.assetBundle.LoadAsset<GameObject>(dfj.pfName));

//        ImageTargetBehaviour arScript = getGo.AddComponent<ImageTargetBehaviour>();

//        //ImageTargetBaseBehaviour arbsScript = getGo.AddComponent<ImageTargetBaseBehaviour>();
//        //arScript.Bind(Tracker);
//        arScript.Bind(Tracker);

//        arScript.SetupWithImage(dfj.image, StorageType.Assets, dfj.name, new Vector2(dfj.size[0], dfj.size[1]));

//        getGo.gameObject.SetActive(true);
//    }

//    //IEnumerator GetAbPackage()
//    //{
//    //    WWW windowswww = new WWW("file://" + Application.dataPath + "/AssetBundles"); //Application.dataPath 当前工程目录
//    //    yield return windowswww;
//    //    //AssetBundleManifest windowsasset = windowswww.assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest; //这里对于新手来说特别重要！
//    //    /*我们去www请求东西的时候是不是会看到上图我断点的地方，mainasset为null
//    //    *我们加载的东西不再这里，我们的资源包里是在断点里看不到资源的
//    //    *需要通过这样的方式去加载windowswww.assetBundle.LoadAsset("AssetBundleManifest")就是这个AssetBundleManifest
//    //    *当分清这个的时候就发现网上其他的教程就完全行的通了，windowsasset 不在为null
//    //    */
//    //    //加载完总的依赖后，就可以进一步加载我们需要的资源包了
//    //    // WWW cubewww = new WWW("file://" + Application.dataPath + "/AssetBundles/abtest1");
//    //    WWW cubewww = new WWW("file://" + Application.dataPath + "/AssetBundles/idcard-1");
//    //    yield return cubewww;
//    //    //然后在加载完资源包后，就可以通过loadasset的方法去load出我们的资源
//    //    //GameObject obj = cubewww.assetBundle.LoadAsset("vvvv@.prefab") as GameObject;

//    //    GameObject getGo = cubewww.assetBundle.LoadAsset<GameObject>("qs0002-idback.prefab");

//    //    ImageTargetBehaviour arScript = getGo.AddComponent<ImageTargetBehaviour>();
//    //    arScript.Path = "targets.json";
//    //    arScript.Name = "idback";
//    //    arScript.Size = new Vector2(8.56f, 5.4f);
//    //    arScript.ActiveTargetOnStart = true;
//    //    arScript.GameObjectActiveControl = true;
//    //    arScript.Storage = StorageType.Assets;
//    //    Instantiate(getGo);
//    //    getGo.gameObject.SetActive(true);

//    //    //Instantiate(obj);
//    //}

//    private void LoadJsonData()
//    {
//        //读取文件  
//        string path = Application.streamingAssetsPath + "/targets.json";

//        StreamReader sr = new StreamReader(path);
//        string json = sr.ReadToEnd();
//        sr.Close();

//        JsonDataPa dd = JsonUtility.FromJson<JsonDataPa>(json);

//        foreach (var js in dd.ImagesList)
//            JsonDictionary.Add(js.ocrId, js);
//    }

//    IEnumerator AndLoadJsonData()
//    {
//        //读取文件  
//        string path = Application.streamingAssetsPath + "/targets.json";

//        WWW awww = new WWW(path);
//        yield return awww;

//        string jsonString = awww.text;
//        JsonDataPa dd = JsonUtility.FromJson<JsonDataPa>(jsonString);

//        foreach (var js in dd.ImagesList)
//            JsonDictionary.Add(js.ocrId, js);

//        string t1 = "";

//        foreach (var d in JsonDictionary)
//        {
//            t1 = t1 + d.Key;
//        }

//        TTTTTTTT2.text = JsonDictionary.Count.ToString() + t1;
//    }

//    IEnumerator LoadJsonFromData()
//    {
//        //读取文件  
//        string path = Application.streamingAssetsPath + "/targets.json";

//        string jsonStr = null;

//#if UNITY_EDITOR_WIN
//        StreamReader sr = new StreamReader(path);
//        jsonStr = sr.ReadToEnd();
//        sr.Close();
//        yield return null;

//#elif UNITY_ANDROID && !UNITY_EDITOR_WIN
//        WWW awww = new WWW(path);
//        yield return awww;
//        jsonStr = awww.text;

//#endif

//        JsonDataPa dd = JsonUtility.FromJson<JsonDataPa>(jsonStr);

//        foreach (var js in dd.ImagesList)
//            JsonDictionary.Add(js.ocrId, js);

//        DataFromJson dfj = LoadDataWithOcrId(testOcrId);
//        if (dfj == null)
//        {
//            TTTTTTTT.text = "dfj is null" + JsonDictionary.Count;
//            yield break;
//        }
//        //至此预读取完成
//        //开始加载
//        string abPath;

//#if UNITY_ANDROID && !UNITY_EDITOR_WIN
//        abPath = Application.streamingAssetsPath + "/AssetBundles/" + dfj.abName;
//        TTTTTTTT.text = "ANDR";
//#endif

//#if UNITY_EDITOR_WIN
//        abPath = "file://" + Application.dataPath + "/AssetBundles/" + dfj.abName;
//        TTTTTTTT.text = "WINDOWS";
//#endif

//        WWW cubewww = new WWW(abPath);
//        yield return cubewww;

//        GameObject getGo = Instantiate(cubewww.assetBundle.LoadAsset<GameObject>(dfj.pfName));

//        ImageTargetBehaviour arScript = getGo.AddComponent<ImageTargetBehaviour>();

//        //ImageTargetBaseBehaviour arbsScript = getGo.AddComponent<ImageTargetBaseBehaviour>();
//        //arScript.Bind(Tracker);
//        arScript.Bind(Tracker);

//        arScript.SetupWithImage(dfj.image, StorageType.Assets, dfj.name, new Vector2(dfj.size[0], dfj.size[1]));

//        getGo.gameObject.SetActive(true);
//    }

//    #endregion

//    IEnumerator WinEditorLoad()
//    {
//        //读取文件  
//        string path = Application.streamingAssetsPath + "/targets.json";

//        StreamReader sr = new StreamReader(path);
//        var jsonStr = sr.ReadToEnd();
//        sr.Close();
//        yield return null;

//        JsonDataPa dd = JsonUtility.FromJson<JsonDataPa>(jsonStr);

//        foreach (var js in dd.ImagesList)
//            JsonDictionary.Add(js.ocrId, js);

//        DataFromJson dfj = LoadDataWithOcrId(testOcrId);
//        if (dfj == null)
//        {
//            TTTTTTTT.text = "dfj is null" + JsonDictionary.Count;
//            yield break;
//        }
//        //至此预读取完成
//        //开始加载
//        string abPath;

//        abPath = "file://" + Application.dataPath + "/AssetsBundles/" + dfj.abName;
//        TTTTTTTT.text = "WINDOWS";

//        WWW cubewww = new WWW(abPath);
//        yield return cubewww;

//        GameObject getGo = Instantiate(cubewww.assetBundle.LoadAsset<GameObject>(dfj.pfName));

//        ImageTargetBehaviour arScript = getGo.AddComponent<ImageTargetBehaviour>();

//        arScript.Bind(Tracker);

//        arScript.SetupWithImage(dfj.image, StorageType.Assets, dfj.name, new Vector2(dfj.size[0], dfj.size[1]));

//        getGo.gameObject.SetActive(true);
//    }

//    /// <summary> 
//    /// 安卓读取 </summary>
//    IEnumerator AndroidLoad()
//    {
//        //读取文件  
//        string path = Application.streamingAssetsPath + "/targets.json";

//        WWW awww = new WWW(path);
//        yield return awww;

//        //string jsonStr = awww.text;

//        JsonDataPa dd = JsonUtility.FromJson<JsonDataPa>(awww.text);

//        foreach (var js in dd.ImagesList)
//            JsonDictionary.Add(js.ocrId, js);

//        //string t1 = "";

//        //foreach (var d in JsonDictionary)
//        //    t1 = t1 + d.Key;

//        //TTTTTTTT2.text = JsonDictionary.Count + t1;

//        DataFromJson dfj = LoadDataWithOcrId(testOcrId);
//        if (dfj == null)
//        {
//            //TTTTTTTT.text = "dfj is null" + JsonDictionary.Count;
//            yield break;
//        }

//        //至此预读取完成
//        //开始加载
//        string abPath = Application.streamingAssetsPath + "/AssetBundles/" + dfj.abName;

//        WWW cubewww = new WWW(abPath);
//        yield return cubewww;

//        GameObject getGo = Instantiate(cubewww.assetBundle.LoadAsset<GameObject>(dfj.pfName));
//        //TTTTTTTT.text = getGo.name;

//        ImageTargetBehaviour arScript = getGo.AddComponent<ImageTargetBehaviour>();

//        arScript.Bind(Tracker);

//        arScript.SetupWithImage(dfj.image, StorageType.Assets, dfj.name, new Vector2(dfj.size[0], dfj.size[1]));

//        getGo.gameObject.SetActive(true);

//    }
//}