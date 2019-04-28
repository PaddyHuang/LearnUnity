using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ItemMovies : MonoBehaviour
{
    public GameObject Controller;

    [SerializeField] private RawImage movieImage;
    [SerializeField] private Button playBtn, pauseBtn;

    [SerializeField] private Text showInfoText;

    private bool isReady, isDownloading;

    private VideoSigle saveVideoData;

    private VideoPlayer m_VideoPlayer;
    private AudioSource m_AudioSource;

    private bool isActiveThis; //切换练习题时需要判断

    void Start()
    {
        m_VideoPlayer = GetComponent<VideoPlayer>();

        playBtn.interactable = false;
        playBtn.onClick.AddListener(PlayMovie);
        pauseBtn.onClick.AddListener(PauseMovie);
    }

    public void CreatMovie(VideoSigle vs)
    {
        Controller.SetActive(false);

        m_VideoPlayer = GetComponent<VideoPlayer>();
        m_AudioSource = GetComponent<AudioSource>();
        saveVideoData = vs;

        StartCoroutine(PlayVideo(vs));
    }

    /// <summary> 
    /// 显示和隐藏时的事件 </summary>
    public void ToggleFunc(bool aoh)
    {
        isActiveThis = aoh;

        if (aoh)
        {
            if (m_VideoPlayer == null)
                return;

            if (!m_VideoPlayer.isPrepared && !isDownloading)
            {
                StopAllCoroutines();
                StartCoroutine(PlayVideo(saveVideoData));
            }
        }
        else
        {
            if (!isReady)
                return;

            m_VideoPlayer.Pause();

            playBtn.gameObject.SetActive(true);
            pauseBtn.gameObject.SetActive(false);
        }
    }

    public void SwitchPractice(bool isthis)
    {
        if (isActiveThis)
        {
            ToggleFunc(isthis);

            Controller.SetActive(isthis);

            isActiveThis = true;
        }
    }

    private void PlayMovie()
    {
        if (!isReady)
            return;
        m_VideoPlayer.Play();
        //m_AudioSource.Play();
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(IsPlayDone());
    }

    private void PauseMovie()
    {
        if (!isReady)
            return;
        m_VideoPlayer.Pause();

        playBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        isDownloading = false;
    }

    IEnumerator IsPlayDone()
    {
        while (m_VideoPlayer.isPlaying)
        {
            yield return null;
        }

        playBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// PC测试不出来
    /// </summary>
    /// <param name="vs"></param>
    /// <returns></returns>
    IEnumerator PlayVideo(VideoSigle vs)
    {
        m_VideoPlayer.playOnAwake = false;
        //可以将这里改为WWW，然后下载完毕时加载本地url
        m_VideoPlayer.url = vs.Url;

        m_VideoPlayer.SetDirectAudioVolume(0, 1);
        m_VideoPlayer.controlledAudioTrackCount = 1;
        m_VideoPlayer.SetTargetAudioSource(0, m_AudioSource);

        m_VideoPlayer.Prepare();

        movieImage.uvRect = new Rect(0, 0, 1, 1);

        while (!m_VideoPlayer.isPrepared)
        {
            isDownloading = true;

            isReady = false;

            yield return null;
        }

        showInfoText.text = vs.Info;
        movieImage.color = Color.white;
        playBtn.interactable = true;
        movieImage.texture = m_VideoPlayer.texture;

        isDownloading = false;
        isReady = true;
    }
}

//public class ItemMovies2 : MonoBehaviour
//{
//    public GameObject Controller;
//    [SerializeField] private RawImage movieImage;
//    [SerializeField] private Button playBtn;
//    [SerializeField] private Button pauseBtn;

//    [SerializeField] private Text showInfoText, progressText;

//    private MediaPlayerCtrl m_MediaPlayerCtrl;

//    private bool isReady;

//    private WWW downWww;

//    //如果下载被强制中断，则重新下载
//    private VideoSigle saveVideoData;

//    private bool isDownloading;

//    void Start()
//    {
//        playBtn.interactable = false;
//        playBtn.onClick.AddListener(PlayMovie);
//        pauseBtn.onClick.AddListener(PauseMovie);
//        //m_MediaPlayerCtrl.DownloadStreamingVideoAndLoad()
//    }

//    /// <summary> 
//    /// 显示和隐藏时的事件 </summary>
//    public void ToggleFunc(bool aoh)
//    {
//        if (aoh)
//        {
//            if (downWww == null)
//                return;

//            if (!downWww.isDone && !isDownloading)
//            {
//                StopAllCoroutines();
//                StartCoroutine(DownLoadMovie(saveVideoData));
//            }
//        }
//        else
//        {
//            if (!isReady)
//                return;

//            m_MediaPlayerCtrl.Pause();

//            //if (movie == null)
//            //    return;

//            //movie.Pause();
//            playBtn.gameObject.SetActive(true);
//            pauseBtn.gameObject.SetActive(false);
//        }
//    }

//    public void CreatMovie(VideoSigle vs)
//    {
//        Controller.SetActive(false);

//        //isInit = true;
//        saveVideoData = vs;

//        m_MediaPlayerCtrl = GetComponent<MediaPlayerCtrl>();

//        m_MediaPlayerCtrl.OnReady += () => { isReady = true; };
//        //CreatFolder(vs);

//        StartCoroutine(DownLoadMovie(vs));
//        //StartCoroutine(m_MediaPlayerCtrl.DownloadStreamingVideoAndLoad(vs.Url));
//        // isInit = false;
//    }

//    private void PlayMovie()
//    {
//        if (!isReady)
//            return;

//        m_MediaPlayerCtrl.Play();

//        //if (movie == null)
//        //    return;

//        //movie.Play();

//        //audio.Play();

//        playBtn.gameObject.SetActive(false);
//        pauseBtn.gameObject.SetActive(true);
//    }

//    private void PauseMovie()
//    {
//        if (!isReady)
//            return;

//        m_MediaPlayerCtrl.Pause();

//        //if (movie == null)
//        //    return;

//        //movie.Pause();
//        //audio.Pause();

//        playBtn.gameObject.SetActive(true);
//        pauseBtn.gameObject.SetActive(false);
//    }

//    private void OnDisable()
//    {
//        isDownloading = false;
//    }

//    /// <summary> 
//    /// 下载视频 </summary>
//    private IEnumerator DownLoadMovie(VideoSigle vs)
//    {
//        downWww = new WWW(vs.Url);

//        string path = CreatFolder(vs);
//        //进度提示
//        while (!downWww.isDone)
//        {
//            isDownloading = true;
//            progressText.text = (int)(downWww.progress * 100) % 100 + "%";
//            yield return null;
//        }

//        progressText.text = String.Empty;

//        File.WriteAllBytes(path, downWww.bytes);

//        m_MediaPlayerCtrl.Load("file://" + path);
//        //movie = downWww.GetMovieTexture();
//        //audio = movieImage.transform.GetComponentInChildren<AudioSource>();
//        //audio.clip = movie.audioClip;
//        //movieImage.texture = movie;

//        yield return downWww;
//        m_MediaPlayerCtrl.Stop();

//        showInfoText.text = vs.Info;
//        movieImage.color = Color.white;
//        playBtn.interactable = true;
//    }

//    private string CreatFolder(VideoSigle vs)
//    {
//        string local = Application.persistentDataPath + "/MoviesData";

//        if (Directory.Exists(local) == false)
//            Directory.CreateDirectory(local);

//        string writePath = local + vs.Url.Substring(vs.Url.LastIndexOf("/", StringComparison.Ordinal));
//        //string writePath = local + vs.Url.Substring(vs.Url.LastIndexOf("/"));

//        return writePath;
//    }
//}