using System;
using UnityEngine;

public class DownLoaderMono : MonoBehaviour
{
    #region Mono-Singleton

    private static DownLoaderMono instance;
    public static DownLoaderMono Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<DownLoaderMono>();
            return instance;
        }
    }

    #endregion

    public void DownLoadAudio(string url, Action<AudioClip> ac)
    {
        StartCoroutine(StaticClass.DownloadAudio(url, ac));
    }
}