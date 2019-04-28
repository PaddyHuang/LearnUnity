using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Baidu.Aip.Ocr;
using Newtonsoft.Json.Linq;
using Debug = UnityEngine.Debug;

public class TaskRecognize
{
    #region Instance

    private static readonly TaskRecognize _instance = new TaskRecognize();

    protected TaskRecognize() { }

    public static TaskRecognize Instance
    {
        get { return _instance; }
    }

    #endregion

    private readonly List<string> MessageList = new List<string>();    

    Regex regexText = new Regex("图[1-9]\\d{0,1}-[1-9]\\d{0,1}");

    string APP_ID = "11387450"; 
    string API_KEY = "mzZLWOVIczeOdGad4BGugYLH";
    string SECRET_KEY = "onTkS5Gt4nNTgO1ZSKpbdgTBeuLnesmY";

    private Ocr ocrClient;
    
    private CancellationTokenSource recognizeTask;

    public bool IsRecognizing { get; private set; }

    public bool HasResult { get; private set; } //是否已经识别出结果

    private byte[] bytesGroup1, bytesGroup2;
    
    private bool hasTask; //是否有线程在运行，如果识别出了，线程已经被dispose了
        
    public void Init()
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        // 修改超时时间
        ocrClient = new Ocr(API_KEY, SECRET_KEY) {Timeout = 60000};
        
        recognizeTask = new CancellationTokenSource();
    }

    public void AbortTask()
    {
        IsRecognizing = HasResult = hasTask = false;

        bytesGroup1 = null;
        bytesGroup2 = null;

        recognizeTask.Cancel();

        recognizeTask.Dispose();
    }

    //public void GeneralBasicDemo()
    //{
    //    byte[] image = File.ReadAllBytes(path);

    //    //// 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
    //    //try 
    //    //{
    //    //    var result = client.GeneralBasic(image);
    //    //    Debug.Log(result);
    //    //}
    //    //catch (Exception e)
    //    //{
    //    //    Debug.Log(e);
    //    //    throw;
    //    //}

    //    // 如果有可选参数
    //    var options = new Dictionary<string, object>
    //    {
    //        {"language_type", "CHN_ENG"},
    //        {"detect_direction", "true"},
    //        {"detect_language", "true"},
    //        {"probability", "true"}
    //    };

    //    var result = ocrClient.GeneralBasic(image, options);
    //    Debug.Log(result);
    //}

    /// <summary> 
    /// 其他类进入识别流程的入口 </summary>
    public void SendToRecognize(byte[] bg1, byte[] bg2)
    {
        bytesGroup1 = bg1;
        bytesGroup2 = bg2;
        
        IsRecognizing = true;
                
        if (!hasTask)
        {
            recognizeTask = new CancellationTokenSource();
            Task.Factory.StartNew(RecFunc, recognizeTask.Token);
            hasTask = true;
        }
    }
       
    private void RecFunc()
    {
        while (!recognizeTask.IsCancellationRequested)
        {
            hasTask = true;            

            if (IsRecognizing)
            {
                MessageList.Clear();

                var curtBg1 = bytesGroup1;
                var curtBg2 = bytesGroup2;

                if (curtBg1 != null && curtBg2 != null)
                {
                    HasResult = false;

                    JObject result1, result2;                    
                    try
                    {
                        // 通用版百度文字识别
                        result1 = ocrClient.GeneralBasic(curtBg1);
                        result2 = ocrClient.GeneralBasic(curtBg2);
                        // 高精度版百度文字识别   -- 有每日使用限制
                        //result = ocrClient.AccurateBasic(curtBg);

                        if (result1["error_code"] != null && result2["error_code"] != null)
                        {
                            Debug.Log(result1["error_msg"]);
                            Debug.Log(result2["error_msg"]);
                            IsRecognizing = false;
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                        IsRecognizing = false;
                        continue;
                    }

                    IsRecognizing = false;
                    //Debug.Log(result);
                    //识别并进行rex出来的结果
                    string r1 = RexKeyWord(result1);
                    string r2 = RexKeyWord(result2);
                    //Debug.Log(r);
                    if (string.IsNullOrEmpty(r1) && string.IsNullOrEmpty(r2))
                        continue;

                    //MessageList.Clear();
                    MessageList.Add(r1);
                    MessageList.Add(r2);

                    HasResult = true;
                    
                    recognizeTask.Cancel();
                    hasTask = false;

                    return;
                }            
            }
        }
    }

    private string RexKeyWord(JObject jo)
    {
        string result = string.Empty;

        JToken group = jo["words_result"];

        List<string> strGroup = new List<string>();

        foreach (var j in group)
            strGroup.Add(j["words"].ToString());

        //如果出现多匹配 则返回第一个

        foreach (var sg in strGroup)
        {
            MatchCollection r = regexText.Matches(sg);
            if (r.Count != 0)
                result = r[0].Value;
        }

        return result;
    }

    public string GetResult()
    {
        HasResult = false;
        if (string.Equals(MessageList[0], MessageList[1]))
            return MessageList[0];
        return string.Empty;
        //Debug.Log(string.Format("MessageList[0]: {0}, MessageList[1]: {1}", MessageList[0], MessageList[1]));
        //return MessageList[0];
    }
    
    /// <summary> 
    /// Init </summary>
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