using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class StaticClass
{
    /// <summary>
    /// 按名字递归查找所有子物体
    /// </summary>
    /// <param name="paTarget">查找的父物体</param>
    /// <param name="goName">需要查找的名字</param>
    /// <returns></returns>
    public static GameObject RecursiveFindChild(Transform paTarget, string goName)
    {
        var go = paTarget.Find(goName);
        if (ReferenceEquals(go, null))
        {
            GameObject g;
            foreach (Transform child in paTarget)
            {
                g = RecursiveFindChild(child, goName);
                if (!ReferenceEquals(g, null))
                    return g.gameObject;
            }
        }

        return go?.gameObject;
    }

    public static T FindInChild<T>(GameObject go) where T : Component
    {
        if (go == null)
            return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        foreach (Transform c in go.transform)
        {
            comp = c.GetComponent<T>();
            if (comp != null)
                break;

            var hasChild = c.childCount;

            if (hasChild != 0)
                comp = FindInChild<T>(c.gameObject);
        }

        return comp;
    }

    /// <summary> 
    /// 下载多张图片 </summary>
    public static IEnumerator DownloadImages(string[] urls, Action<List<Sprite>> result)
    {
        //todo:错误码
        if (urls.Length < 1)
            yield break;

        if (urls.Contains(String.Empty))
            yield break;

        List<Sprite> sps = new List<Sprite>();

        //System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();

        for (int i = 0; i < urls.Length; i++)
        {
            string noUniStr = urls[i];

            WWW www = new WWW(noUniStr);

            while (!www.isDone)
            {
                //elapsedTime += Time.deltaTime;
                //if (elapsedTime >= 10.0f)
                //    break;
                yield return null;
            }

            if (!www.isDone || !String.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Load Failed");
                result(null); // Pass null result.
                yield break;
            }

            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);

            // assign the downloaded image to sprite
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

            //split name
            string[] spg = noUniStr.Split('/');

            spriteToUse.name = spg[spg.Length - 1];

            sps.Add(spriteToUse);

            www.Dispose();
        }

        result(sps);
        // Pass retrieved result.
        //stopwatch.Stop(); //  停止监视
        //TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间

        //Debug.Log(timespan.Milliseconds);
    }

    public static IEnumerator Post(string url, string jsonData, Action<bool> isSuccess)
    {
        StaticData.UserName = "这里是名字";
        string userName = StaticData.UserName;
        WWWForm form = new WWWForm();

        form.AddField("UserName", userName);
        form.AddField("answer", jsonData);

        WWW www = new WWW(url, form);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("失败");
            isSuccess(false);
            yield return null;
        }
        else
        {
            Debug.Log("成功");
            isSuccess(true);
            yield return null;
        }
    }

    /// <summary> 
    /// 下载音频 </summary>
    public static IEnumerator DownloadAudio(string url, Action<AudioClip> audio)
    {
        WWW www = new WWW(url);

        if (www.error != null)
        {
            Debug.Log("失败");
            yield break;
        }

        while (!www.isDone)
        {
            yield return null;
        }

        var ad = www.GetAudioClip();

        audio(ad);

        www.Dispose();
    }

    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null)
            return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        var t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }

        return comp;
    }
}