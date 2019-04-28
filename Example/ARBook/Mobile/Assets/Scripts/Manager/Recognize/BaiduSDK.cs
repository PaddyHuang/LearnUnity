using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Baidu.Aip.Ocr;
using UnityEngine;

public class BaiduSDK : MonoBehaviour
{
    string APP_ID = "11153197";
    string API_KEY = "rYA56lrrrqMdg9W0EujA6pPn";
    string SECRET_KEY = "YfVE8Kjus0nvA2SWZ2DF1IzbAAEA9nZx";

    private Ocr client;

    private string path;

    void Awake()
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    }

    private void Start()
    {
        path = Application.dataPath + "/111.jpg";
        client = new Ocr(API_KEY, SECRET_KEY);
        client.Timeout = 60000; // 修改超时时间
        GeneralBasicDemo();
    }

    public void GeneralBasicDemo()
    {
        byte[] image = File.ReadAllBytes(path);

        //// 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
        //try
        //{
        //    var result = client.GeneralBasic(image);
        //    Debug.Log(result);
        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e);
        //    throw;
        //}

        // 如果有可选参数
        var options = new Dictionary<string, object>
        {
            {"language_type", "CHN_ENG"},
            {"detect_direction", "true"},
            {"detect_language", "true"},
            {"probability", "true"}
        };

        var result = client.GeneralBasic(image, options);
        Debug.Log(result);
    }

    public void GeneralBasicUrlDemo()
    {
        string url = "https//www.x.com/sample.jpg";

        // 调用通用文字识别, 图片参数为远程url图片，可能会抛出网络等异常，请使用try/catch捕获
        var result = client.GeneralBasicUrl(url);
        Debug.Log(result);
        //Console.WriteLine(result);
        // 如果有可选参数
        var options = new Dictionary<string, object>
        {
            {"language_type", "CHN_ENG"},
            {"detect_direction", "true"},
            {"detect_language", "true"},
            {"probability", "true"}
        };

        // 带参数调用通用文字识别, 图片参数为远程url图片
        result = client.GeneralBasicUrl(url, options);
        Debug.Log(result);
    }

    /// <summary> 
    /// init </summary>
    private bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2) certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }
}