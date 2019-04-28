using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CalculOverall : MonoBehaviour
{
    [SerializeField] private Text showAllInfo;
    [SerializeField] private Button playBtn, pauseBtn;
    private string saveMainStr;

    private AudioClip saveClip;
    private AudioManager m_AudioManager;

    private AnimState animState;

    private void Start()
    {
        playBtn.onClick.AddListener(PlayAnim);
        playBtn.onClick.AddListener(ContinueAnim);
        pauseBtn.onClick.AddListener(PauseAnim);
    }

    /// <summary> 
    /// 创建计算类型概览 </summary>
    public void CreatOverallUI(CalculOverallInfo coi)
    {
        if (coi.Info != null)
        {
            saveMainStr = coi.Info;
            showAllInfo.text = saveMainStr;
            DownLoaderMono.Instance.DownLoadAudio(coi.Audio, s => saveClip = s);
            m_AudioManager = AudioManager.Instance;
        }
    }

    void PlayAnim()
    {
        if (animState != AnimState.Await)
            return;

        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);

        animState = AnimState.Play;

        m_AudioManager.PlayAudio(saveClip);
        AutoReset();
    }

    void ContinueAnim()
    {
        if (animState != AnimState.Pause)
            return;
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
        animState = AnimState.Play;

        m_AudioManager.ContinueAudio();
    }

    void PauseAnim()
    {
        if (animState != AnimState.Play)
            return;

        animState = AnimState.Pause;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);

        m_AudioManager.PauseAudio();
    }

    /// <summary> 
    /// 当重新识别该物体时 </summary>
    public void OnReFound()
    {
        animState = AnimState.Await;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);

        m_AudioManager.PauseAudio();
    }

    public void ToggleFunc(bool b) { }

    /// <summary> 
    /// 不使用Update </summary>
    private void AutoReset()
    {
        StopAllCoroutines();
        StartCoroutine(AutoResetDelay(saveClip.length));
    }

    IEnumerator AutoResetDelay(float time)
    {
        yield return new WaitForSeconds(time);

        animState = AnimState.Await;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }
}