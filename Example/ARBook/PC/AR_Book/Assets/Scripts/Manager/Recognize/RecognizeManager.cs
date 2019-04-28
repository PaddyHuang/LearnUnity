using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RecognizeManager : MonoBehaviour
{
    #region Mono-Singleton

    private static RecognizeManager instance;
    public static RecognizeManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<RecognizeManager>();
            return instance;
        }
    }

    #endregion

    //存储已经创建过的类型
    private Dictionary<string, GameObject> CreatedObjectColl = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> CreatedObjectCollProp { get { return CreatedObjectColl ?? (CreatedObjectColl = new Dictionary<string, GameObject>()); } private set { CreatedObjectColl = value; } }

    private Queue<GameObject> PreGameObjectQueue = new Queue<GameObject>();
    private Queue<GameObject> PreGameObjectQueueProp { get { return PreGameObjectQueue ?? (PreGameObjectQueue = new Queue<GameObject>()); } }

    //全局静态变量结束
    public GameObject CreatedObject => createdObject;

    private GameObject createdObject;

    public Camera JieTuCam;

    private UIMainCanvas m_UIMainCanvas;

    private bool isRecing;

    private Gyroscope go;

    private TaskRecognize m_TaskRecognize;
    private TypeFactory m_TypeFactory; //创建工厂

    private int takeWidth, takeHeight;
    private float rectX, rectY, rectWidth, rectHeight;

    private int successedTimes;
    private float elapsedTime;
    
    //public Text ttt;

    void Awake()
    {
        m_UIMainCanvas = UIMainCanvas.Instance;

        m_TaskRecognize = TaskRecognize.Instance;
        m_TaskRecognize.Init();

        m_TypeFactory = TypeFactory.Instance;
    }

    void Start()
    {
        SizeInit();

        GyroscopeInit();

        isRecing = true;
    }

    /// <summary> 
    /// 触发识别按钮,仅PC测试使用，外部拖动调用 </summary>
    public void StartRecognize()
    {
        isRecing = true;
    }

    void Update()
    {
        if (!isRecing)
            return;

        //每0.6s检索一次
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 0.6f)
        {
            if (!m_TaskRecognize.IsRecognizing)
                m_UIMainCanvas.RecognizingEffect(false);

            if (!m_TaskRecognize.HasResult)
            {
                elapsedTime = 0;

                float curtRate = go.rotationRate.magnitude;

                if (curtRate > 0.35f)
                {
                    successedTimes = 0;
                    return;
                }

                successedTimes++;

                if (successedTimes > 2)
                {
                    //如果没有在识别
                    if (!m_TaskRecognize.IsRecognizing)
                    {                       
                        StartCoroutine(UploadPngCurrent());
                        successedTimes = 0;
                    }
                    else
                        successedTimes = 1; //少识别一次,防止识别太久                    
                }
            }

            if (m_TaskRecognize.HasResult)
            {
                string result = m_TaskRecognize.GetResult();
                //Debug.Log(string.Format("result: {0}", result));
                               
                isRecing = false;

                GameObject isCreated;

                elapsedTime = successedTimes = 0;

                //检测是否已经创建过，若已经创建过 则不再需要创建一次
                if (GameDataMono.Instance.CreatedObjectCollProp.TryGetValue(result, out isCreated))
                {
                    MainGameObject mgo = isCreated.GetComponent<MainGameObject>();
                    mgo.OnFoundFunc();
                    GameDataMono.Instance.SetCurrentGameObject(isCreated);

                    isCreated.gameObject.SetActive(true);

                    m_UIMainCanvas.RecPanelHide();
                }
                else                    
                    StartCoroutine(PostToServer(result));                               
            }           
        }
    }

    /// <summary> 
    /// post 到服务器 </summary>
    IEnumerator PostToServer(string result)
    {
        WWWForm form = new WWWForm();

        form.AddField("BookID", StaticData.BookInfo.currentBookID);
        form.AddField("keywords", result);

        //foreach (var item in form.data)
        //    Debug.Log(Convert.ToChar(item));

        using (var www = new WWW(StaticData.RecPostUrl, form))
        {
            yield return www;

            if (www.error != null)
            {
                Debug.Log("失败");

                isRecing = true;
                yield break;
            }
            else
            {
                string serverResult = www.text;
                //Debug.Log(serverResult);
                JsonData data = JsonMapper.ToObject(serverResult);

                int code = int.Parse(data["code"].ToString());
                
                //识别数据错误
                if (code != 0)
                {
                    isRecing = true;
                    Debug.Log(data["msg"]);
                    yield break;
                }               

                string md5 = null;
                //var md5 = data["data"]["phonemd5"].ToString();

                if (Application.platform == RuntimePlatform.Android)
                    md5 = data["data"]["phonemd5"].ToString();
                else if (Application.platform == RuntimePlatform.WindowsPlayer ||
                         (Application.platform == RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.Android))
                    md5 = data["data"]["pcmd5"].ToString();

                //临时保存本地的json表，等待ab包下载完后再进行存储
                LocalAssetManager.Instance.SaveTemp(result, md5);

                //进行到此处时 已经正确解析出结果
                GameDataMono.Instance.CreatedSuccess();

                m_TypeFactory?.Post(data["data"].ToJson());
                m_UIMainCanvas.RecPanelHide();
                m_UIMainCanvas.SetLoadingPercentage(0);
            }
        }
    }

    void GyroscopeInit()
    {
        go = Input.gyro;
        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true    
        Input.gyro.enabled = true;
        //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次    
        Input.gyro.updateInterval = 0.2f;
    }

    /// <summary> 
    /// 截图框初始化 </summary>
    void SizeInit()
    {
        var rect = m_UIMainCanvas.FrameImage;

        int width = (int) rect.rect.width;
        int height = (int) rect.rect.height;

        float facX = Screen.width / 1920f;
        float facY = Screen.height / 1080f;

        takeWidth = (int) (width * facX);
        takeHeight = (int) (height * facY);

        rectX = rect.anchoredPosition.x * facX;
        rectY = rect.anchoredPosition.y * facY;

        rectWidth = width * facX;
        rectHeight = height * facY;
    }
    
    /// <summary>
    /// 上传到百度识别
    /// </summary>
    /// <returns></returns>
    IEnumerator UploadPngCurrent()
    {
        //m_UIMainCanvas.FrameHide();

        // 用协程等待屏幕渲染完成后再截图
        yield return new WaitForEndOfFrame();
        // m_UIMainCanvas.FrameShow();

        Texture2D tex1 = new Texture2D(takeWidth, takeHeight, TextureFormat.ARGB32, false);

        // 将屏幕像素保存到新建的Texture2D(截图原理)
        tex1.ReadPixels(new Rect(rectX, rectY, rectWidth, rectHeight), 0, 0);

        tex1.Apply();
        
        byte[] bg1 = tex1.EncodeToJPG();
        
        // 第二张截图
        Texture2D tex2 = new Texture2D(takeWidth, takeHeight, TextureFormat.ARGB32, false);

        // 将屏幕像素保存到新建的Texture2D(截图原理)
        tex2.ReadPixels(new Rect(rectX, rectY, rectWidth, rectHeight), 0, 0);

        tex2.Apply();

        byte[] bg2 = tex1.EncodeToJPG();

        m_TaskRecognize?.SendToRecognize(bg1, bg2);
        
        m_UIMainCanvas.RecognizingEffect(true);

        // 手动销毁截图
        //Destroy(tex);
    }
    
    //IEnumerator UploadPngCurrent()
    //{
    //    // 用协程等待屏幕渲染完成后再截图
    //    yield return new WaitForEndOfFrame();

    //    // 创建一个RenderTexture对象    
    //    RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);

    //    // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机    
    //    JieTuCam.targetTexture = rt;
    //    JieTuCam.Render();

    //    // 激活这个rt, 并从中中读取像素。    
    //    RenderTexture.active = rt;
    //    Texture2D screenShot = new Texture2D(takeWidth, takeHeight, TextureFormat.ARGB32, false);
    //    screenShot.ReadPixels(new Rect(rectX, rectY, rectWidth, rectHeight), 0, 0); // 注：这个时候，它是从RenderTexture.active中读取像素    
    //    screenShot.Apply();

    //    // 重置相关参数，以使用camera继续在屏幕上显示    
    //    JieTuCam.targetTexture = null;

    //    RenderTexture.active = null; // JC: added to avoid errors   

    //    //Destroy(rt);
    //    // 最后将这些纹理数据，成一个png图片文件    
    //    byte[] bytes = screenShot.EncodeToPNG();

    //    m_TaskRecognize?.SendToRecognize(bytes);
    //}

    private void OnDestroy()
    {
        //GameData.Instance.OnDestroy();

        if (m_TaskRecognize != null)
            m_TaskRecognize.AbortTask();
    }
}