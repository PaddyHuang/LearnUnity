using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class Record : MonoBehaviour {

    // Baidu Info
    private const string APP_ID = "11801011";
    private const string API_KEY = "bbWf4ACn6thWn1XrvqQr7GES";
    private const string SECRECT_KEY = "7ps962ZvMdmql8qEMrMIhhAGgMBr1oY5";

    // Record Info
    private Button btn;    
    // 按键按下时长
    private double downTime = 0.0f;             
        
    // 存储录音
    private AudioSource audioSource;
    //// 录音文件保存位置
    //private string filePath = string.Empty;
    // 标记是否有麦克风
    private bool isHaveMic = false;
    // 当前录音设备名称
    private string currentDeviceName = string.Empty;
    // 采样率
    public const int SampleRate = 16000;
    // 录音时间长度(最长录音时间)
    private int MAX_RECORD_TIME = 100;
    private int TRUE_RECORD_TIME = 0;
        
    // 百度Token
    private string TOKEN = string.Empty;
    // 按键文本
    public Text btnText;
    // 结果显示文本
    public Text asrResult;

    private void Awake()
    {
        btn = this.GetComponent<Button>();
        audioSource = this.GetComponent<AudioSource>();
        //filePath = Application.dataPath + "/Audio";   
        btnText.text = "按住录音,松开识别";
    }

    // Use this for initialization
    void Start()
    {
        // 按键监听
        EventTriggerListener.Get(btn.gameObject).onDown = OnPointerDown;
        EventTriggerListener.Get(btn.gameObject).onUp = OnPointerUp;
        // 判断是否有录音设备
        if (Microphone.devices.Length > 0)
        {
            isHaveMic = true;
            currentDeviceName = Microphone.devices[0];            
        }
    }
    
    // Record
    public bool StartRecording()
    {
        if (isHaveMic == false || Microphone.IsRecording(currentDeviceName))
        {
            Debug.LogWarning("No microphone found to record audio clip sample with.");
            return false;
        }
        // 停止前一次录音
        Microphone.End(currentDeviceName);
        // 记录按下的时刻
        downTime = GetTimestampOfNowWithMillisecond();
        // 开始本次录音
        audioSource.clip = Microphone.Start(currentDeviceName, false, MAX_RECORD_TIME, SampleRate);
        return true;
    }
    
    public int EndRecording()
    {
        if (isHaveMic == false || !Microphone.IsRecording(currentDeviceName))
        {
            return 0;
        }
        //结束录音
        Microphone.End(currentDeviceName);
        //向上取整,避免遗漏录音末尾
        return Mathf.CeilToInt((float)(GetTimestampOfNowWithMillisecond() - downTime) / 1000f);
    }

    // 获取毫秒时间戳，用来计算录音时长
    public double GetTimestampOfNowWithMillisecond()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }

    // Mouse Event
    public void OnPointerDown(GameObject btn)
    {                
        if (StartRecording())
        {
            btnText.text = "录音成功";
        }
        else
        {
            btnText.text = "录音失败";
        }
    }

    public void OnPointerUp(GameObject btn)
    {
        TRUE_RECORD_TIME = EndRecording();
        if(TRUE_RECORD_TIME > 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
            StartCoroutine(AsrData());
        }
        else
        {
            asrResult.text = "录音时长过短";
        }
    }


    // Baidu Request
    IEnumerator AsrData()
    {
        if (string.IsNullOrEmpty(TOKEN))
        {
            yield return GetAccessToken();
        }

        //var data = File.ReadAllBytes(@"F:\UnityProjects\New Unity Project\Assets\Audio\16k.wav");
        //var data = File.ReadAllBytes(@"F:\UnityProjects\New Unity Project\Assets\Audio\16k.pcm");
        //var data = WavToBytes(audioSource.clip);
        //var client = new Baidu.Aip.Speech.Asr(user.API_KEY, user.SECRECT_KEY);
        //client.Timeout = 120000;

        //处理当前录音数据为PCM16
        float[] samples = new float[SampleRate * TRUE_RECORD_TIME * audioSource.clip.channels];
        audioSource.clip.GetData(samples, 0);
        var samplesShort = new short[samples.Length];
        for (var index = 0; index < samples.Length; index++)
        {
            samplesShort[index] = (short)(samples[index] * short.MaxValue);
        }
        byte[] datas = new byte[samplesShort.Length * 2];
        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

        //var result = client.Recognize(data, "wav", 16000);

        string url = string.Format("http://vop.baidu.com/server_api?dev_pid=1536&cuid={0}&token={1}", SystemInfo.deviceUniqueIdentifier, TOKEN);
        WWWForm data = new WWWForm();
        data.AddBinaryData("audio", datas);

        using (var request = UnityWebRequest.Post(url, data))
        {
            request.SetRequestHeader("Content-Type", "audio/wav;rate=" + SampleRate);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Recognized!");
                btnText.text = "Recognized!";
                asrResult.text = GetJsonValue(request.downloadHandler.text, "result");
                //Debug.Log(asrResult.text);
            }

            if (asrResult.text == string.Empty)
            {
                asrResult.text = "未识别出文字";
                //Debug.Log("未识别出文字");
            }
        }        
    }

    IEnumerator GetAccessToken()
    {
        string url = string.Format("https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}&", API_KEY, SECRECT_KEY);
        Debug.Log(url);

        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
            Debug.Log(request.downloadHandler.text);
            TOKEN = GetJsonValue(request.downloadHandler.text, "access_token");
            Debug.Log("TKOEN: " + TOKEN);
            //btnText.text = "TKOEN: " + TOKEN;
        }
    }

    // 获取Json字符串某节点的值
    public static string GetJsonValue(string jsonStr, string key)
    {
        string result = string.Empty;
        if (!string.IsNullOrEmpty(jsonStr))
        {
            key = "\"" + key.Trim('"') + "\"";
            int index = jsonStr.IndexOf(key) + key.Length + 1;
            if (index > key.Length + 1)
            {
                //先截逗号，若是最后一个，截"｝"号，取最小值
                int end = jsonStr.IndexOf(',', index);
                if (end == -1)
                {
                    end = jsonStr.IndexOf('}', index);
                }
                result = jsonStr.Substring(index, end - index);
                result = result.Trim(new[] { '"', ' ', '\"', '[', ']' }); //过滤引号或空格
            }
        }
        return result;
    }

}


