using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;

public class LocalAssetManager : MonoBehaviour
{
    #region Mono-Singleton

    private static LocalAssetManager instance;
    public static LocalAssetManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<LocalAssetManager>();
            return instance;
        }
    }

    #endregion

    private Dictionary<string, Package> PackagesColl = new Dictionary<string, Package>();

    private Queue<Package> PackagesQueue = new Queue<Package>();

    private string tempMD5, tempKey;

    void Awake()
    {
        InitLocalData();
    }

    /// <summary> 
    /// 进行本地和服务器MD5的校验 </summary>
    public string GetLocalAsset()
    {
        Package p;
        //如果校验通过，与服务器相同
        if (PackagesColl.TryGetValue(tempKey, out p))
            if (string.Equals(p.MD5, tempMD5))
            {
                string comb;
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
                comb = "file://" + p.FilePath;
#endif
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                comb = p.FilePath;
#endif
                return comb;
            }

        return null;
    }

    private void InitLocalData()
    {
        string lf = StaticData.AssetBundlePath + "/" + StaticData.LocalJsonName;

        if (!Directory.Exists(StaticData.AssetBundlePath))
            Directory.CreateDirectory(StaticData.AssetBundlePath);

        if (!File.Exists(lf))
        {
            File.Create(lf);
            return;
        }

        var files = Directory.GetFiles(StaticData.AssetBundlePath);

        for (var index = 0; index < files.Length; index++)
        {
            var file = files[index];
            files[index] = file.Replace('\\', '/');
        }

        //获取文件后读取成json格式
        string jsdata = File.ReadAllText(lf, Encoding.UTF8);
        if (string.IsNullOrEmpty(jsdata))
            return;

        JsonData jd = JsonMapper.ToObject(jsdata);

        Package[] pg = new Package[jd["AssetData"].Count];

        for (int i = 0; i < jd["AssetData"].Count; i++)
        {
            pg[i] = new Package();
            pg[i].Name = jd["AssetData"][i]["Name"].ToString();
            pg[i].MD5 = jd["AssetData"][i]["MD5"].ToString();
            pg[i].Update = false;

            for (int j = 0; j < files.Length; j++)
            {
                string name = files[j].Split('/').Last();

                if (string.Equals(name, pg[i].Name))
                {
                    pg[i].FilePath = files[j];
                    break;
                }
            }
        }

        CreateData(pg);
    }

    public void CreateData(Package[] pg)
    {
        foreach (var p in pg)
        {
            //PackagesQueueStr.Enqueue(p.Name);
            PackagesColl.Add(p.Name, p);
            PackagesQueue.Enqueue(p);
        }
    }

    /// <summary> 
    /// 服务器验证通过后，先临时存储数据 </summary>
    public void SaveTemp(string tk, string tm)
    {
        // Debug.Log(tk);
        tempKey = tk.Replace("图", "p");
        tempMD5 = tm;
    }

    /// <summary> 
    /// 下载完Ab包后，正式保存数据 </summary>
    public void WriteTempData(byte[] abBytes)
    {
        if (!String.IsNullOrEmpty(tempKey) && !String.IsNullOrEmpty(tempMD5))
        {
            string filePath = StaticData.AssetBundlePath + "/" + tempKey;
            Debug.Log(filePath);
            File.WriteAllBytes(filePath, abBytes);

            AddNewFileData(tempKey, tempMD5, filePath);
        }
    }

    private void AddNewFileData(string key, string md5, string filePath)
    {
        //如果超过了五个资源包
        if (PackagesQueue.Count >= 5)
        {
            var pp = PackagesQueue.Dequeue();
            File.Delete(pp.FilePath);
            PackagesColl.Remove(pp.Name);
        }

        Package p;
        if (PackagesColl.TryGetValue(key, out p))
        {
            p.MD5 = md5;
            p.Update = false;
        }
        else
        {
            Package np = new Package
            {
                MD5 = md5,
                Name = key,
                Update = false,
                //FilePath = filePath,
                FilePath = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(filePath)) 
                
            };
            PackagesColl.Add(np.Name, np);
        }

        JsonFormat jf = new JsonFormat();

        foreach (var pc in PackagesColl)
        {
            jf.AssetData.Add(pc.Value);
        }

        string jsonStr = JsonMapper.ToJson(jf);

        string lf = StaticData.AssetBundlePath + "/" + StaticData.LocalJsonName;

        File.WriteAllText(lf, jsonStr);
    }

    //private Package[] GetLocalJsonData()
    //{
    //    string lf = StaticData.AssetBundlePath + "/" + StaticData.LocalJsonName;

    //    if (!File.Exists(lf))
    //    {
    //        File.WriteAllText(lf, "");
    //        return null;
    //    }

    //    //获取文件后读取成json格式
    //    string jsdata = File.ReadAllText(lf, Encoding.UTF8);

    //    JsonData jd = JsonMapper.ToObject(jsdata);

    //    Package[] pg = new Package[jd["AssetData"].Count];

    //    for (int i = 0; i < jd["AssetData"].Count; i++)
    //    {
    //        pg[i].Name = jd["AssetData"][i]["Name"].ToString();
    //        pg[i].MD5 = jd["AssetData"][i]["MD5"].ToString();
    //        pg[i].Update = false;
    //    }

    //    CreateData(pg);
    //    return pg;
    //}
}

public class JsonFormat
{
    public List<Package> AssetData = new List<Package>();
}

//public string Name;
//public string MD5;
//public string Url;
//public bool Update;
//public string FilePath;