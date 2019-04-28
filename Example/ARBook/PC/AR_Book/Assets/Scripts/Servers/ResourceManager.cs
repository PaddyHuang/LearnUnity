using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

public class ResourceManager : MonoBehaviour
{
    private const string Geturls = "http://www.73cloud.top:8080/ArAppResource/LoadData";
    

    void Awake()
    {
        //读取本地资源
        //StartCoroutine(Check());
        //StartCoroutine(LoadLocalVersion()); 


        //加载本地的json数据
        StartCoroutine(LoadLocalJsonData());

        //Loading.Instance.DownloadCompleted = true;
    }

    IEnumerator Check()
    {
        int platform = -1;
        if (Application.platform == RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor)
            platform = 0;
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            //if (Application.platform == RuntimePlatform.WindowsPlayer)
            platform = 1;

        // Debug.Log(platform);

        UnityWebRequest web = UnityWebRequest.Get(Geturls + "?clienttype=" + platform);

        yield return web.SendWebRequest();

        if (!string.IsNullOrEmpty(web.error))
        {
            Debug.Log("失败");
            yield return new WaitForSeconds(1);
            yield return Check();
            yield break;
        }

        string packs = web.downloadHandler.text;

        JsonData data = JsonMapper.ToObject(packs);

        int count = 0;

        for (int i = 0; i < data["data"].Count; i++)
            if (int.Parse(data["data"][i]["ClientType"].ToString()) == platform)
                count++;
        //Debug.Log(count);
        Package[] serverPgs = new Package[count];

        for (int i = 0; i < data["data"].Count; i++)
        {
            if (int.Parse(data["data"][i]["ClientType"].ToString()) != platform)
                continue;

            Package pack = new Package();
            pack.Name = data["data"][i]["Name"].ToString();
            pack.MD5 = data["data"][i]["MD5"].ToString();
            pack.Url = data["data"][i]["Url"].ToString();
            serverPgs[i] = pack;
        }

        Package[] localPgs = GetLocalFiles();

        List<Package> dlGroup = new List<Package>();

        if (localPgs != null)
        {
            foreach (var sp in serverPgs)
            {
                bool isContain = false; //判断是否包含

                foreach (var lp in localPgs)
                {
                    int i = sp.CompareTo(lp);

                    if (i != -1 && isContain == false)
                        isContain = true;

                    if (i == 1)
                        dlGroup.Add(sp);
                }

                if (!isContain)
                    dlGroup.Add(sp);
            }
        }

        //UICheckPackages uicp = UICheckPackages.Instance;

        ////如果尚有文件需要下载
        //if (dlGroup.Count != 0)
        //{
        //    uicp.Active(dlGroup.Count);

        //    foreach (var dl in dlGroup)
        //    {
        //        // Debug.Log(dl.Url);
        //        UnityWebRequest dlw = UnityWebRequest.Get(dl.Url);
        //        //Debug.Log(dl.Name);
        //        uicp.StartNewDownload();
        //        dlw.SendWebRequest();

        //        while (!dlw.isDone)
        //        {
        //            uicp.SetCurrent(dlw.downloadProgress);
        //            yield return new WaitForEndOfFrame();
        //        }

        //        byte[] aBytes = dlw.downloadHandler.data;

        //        File.WriteAllBytes(AssetBundlePath + "/" + dl.Name, aBytes);
        //    }
        //}

        //uicp.DownLoadCompleted();

        //Loading.Instance.DownloadCompleted = true;
    } 

    private IEnumerator LoadLocalVersion()
    {
        string locVerPath = Application.streamingAssetsPath + "/version.txt";

        using (UnityWebRequest loc = UnityWebRequest.Get(locVerPath))
        {
            yield return loc.SendWebRequest();
            //获取本地版本号
            if (loc.isNetworkError || loc.isHttpError)
                Debug.Log(loc.error);
            else 
            {
                string locVer = loc.downloadHandler.text;
                //将版本号post至服务器
                using (UnityWebRequest web = UnityWebRequest.Post("", locVer))
                {
                    //获取服务器返回信息
                    yield return web.SendWebRequest();

                    if (web.isNetworkError || web.isHttpError)
                        Debug.Log(web.error);
                    else
                    {
                        string serverData = web.downloadHandler.text;
                    }
                }
            }
        }
    }

    IEnumerator Check2()
    {
        int platform = -1;
        if (Application.platform == RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor)
            platform = 0;
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            platform = 1;

        UnityWebRequest web = UnityWebRequest.Get(Geturls + "?clienttype=" + platform);

        yield return web.SendWebRequest();

        if (!string.IsNullOrEmpty(web.error))
        {
            Debug.Log("失败");
            //如果失败，则反复尝试获取服务器信息
            yield return new WaitForSeconds(1);
            yield return Check();
            yield break;
        }

        string packs = web.downloadHandler.text;

        JsonData data = JsonMapper.ToObject(packs);

        int count = 0;

        for (int i = 0; i < data["data"].Count; i++)
            if (int.Parse(data["data"][i]["ClientType"].ToString()) == platform)
                count++;
        //Debug.Log(count);
        Package[] serverPgs = new Package[count];

        for (int i = 0; i < data["data"].Count; i++)
        {
            if (int.Parse(data["data"][i]["ClientType"].ToString()) != platform)
                continue;

            Package pack = new Package();
            pack.Name = data["data"][i]["Name"].ToString();
            pack.MD5 = data["data"][i]["MD5"].ToString();
            //pack.Url = data["data"][i]["Url"].ToString();
            serverPgs[i] = pack;
        }

        Package[] localPgs = GetLocalJsonData();

        if (localPgs != null)
        {
            foreach (var sp in serverPgs)
            {
                for (int k = 0; k < localPgs.Length; k++)
                {
                    int ik = sp.CompareTo(localPgs[k]);
                    if (ik != 0)
                        localPgs[k].Update = true;
                }
            }
        }

        //创建本地的数据结构
        LocalAssetManager.Instance.CreateData(localPgs);

        //uicp.DownLoadCompleted();

        //Loading.Instance.DownloadCompleted = true;
    }

    /// <summary> 
    /// 获取本地文件 </summary>
    private Package[] GetLocalFiles()
    {
        string path = StaticData.AssetBundlePath;

        //文件夹是否存在
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        //获取文件路径
        string[] nameg = Directory.GetFiles(path);
        if (nameg.Length == 0)
            return new Package[0];

        string[] nng = new string[nameg.Length];
        //获取文件名
        for (int i = 0; i < nng.Length; i++)
            nng[i] = Path.GetFileName(nameg[i]);

        Package[] pg = new Package[nng.Length];

        for (var i = 0; i < nng.Length; i++)
        {
            var s = nng[i];

            Package p = new Package();
            p.Name = s;
            p.MD5 = UtilsFiles.GetFileHash(path + "/" + s);
            pg[i] = p;

            // Debug.Log(p.Name + "," + p.MD5);
            //if (!p.Name.Contains(".meta"))
            //cbn += p.Name + "," + p.MD5.ToUpper() + ";" + '\n';
        }

        //Debug.Log(cbn);

        return pg;
    }

    private Package[] GetLocalJsonData()
    {
        string lf = StaticData.AssetBundlePath + "/" + StaticData.LocalJsonName;

        if (!File.Exists(lf))
        {
            File.WriteAllText(lf, "");
            return null;
        }

        //获取文件后读取成json格式
        string jsdata = File.ReadAllText(lf, Encoding.UTF8);

        JsonData jd = JsonMapper.ToObject(jsdata);

        Package[] pg = new Package[jd["AssetData"].Count];

        for (int i = 0; i < jd["AssetData"].Count; i++)
        {
            pg[i].Name = jd["AssetData"][i]["Name"].ToString();
            pg[i].MD5 = jd["AssetData"][i]["MD5"].ToString();
            pg[i].Update = false;
        }

        LocalAssetManager.Instance.CreateData(pg);
        return pg;
    }

    /// <summary> 
    /// 解析本地的json数据 </summary>
    IEnumerator LoadLocalJsonData()
    {
        string path = Application.streamingAssetsPath + "/" + "CreateData.json";

        WWW getData = new WWW(path);
        yield return getData;

        string jsdata = System.Text.Encoding.UTF8.GetString(getData.bytes, 3, getData.bytes.Length - 3);

        //GameJsonData.Instance.SetJson(jsdata);

        getData.Dispose();
    }
}

public class Package : IComparable<Package>
{
    public string Name;
    public string MD5;
    public string Url;
    public bool Update;
    public string FilePath;

    /// <summary>
    /// 比较
    /// </summary>
    /// <param name="other"></param>
    /// <returns>0 = pass，1 = 重新下载</returns>
    public int CompareTo(Package other)
    {
        if (string.Equals(Name, other.Name))
        {
            if (string.Equals(MD5, other.MD5, StringComparison.CurrentCultureIgnoreCase))
                return 0;

            return 1;
        }

        return -1;
    }
}