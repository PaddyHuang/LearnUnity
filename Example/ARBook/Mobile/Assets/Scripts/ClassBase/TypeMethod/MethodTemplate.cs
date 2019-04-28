using UnityEngine;
using UnityEngine.UI;

public class MethodTemplate : MonoBehaviour
{
    [SerializeField] private Button btnAnimPlay, btnAnimPause; //开始播放动画
    [SerializeField] private Text showAllInfo; //下方展示文字区

    private Animator anim;
    private string firstAnimName; //获取动画名字

    private GameObject bandinGameObject;

    private AnimState animState = AnimState.Await;

    private AudioClip audioClip;
    private AudioManager m_AudioManager;

    private float elasped, targetTime;

    void Start()
    {
        btnAnimPlay.onClick.AddListener(PlayAnim);
        btnAnimPlay.onClick.AddListener(ContinueAnim);
        btnAnimPause.onClick.AddListener(PauseAnim);
    }

    void Update()
    {
        if (animState == AnimState.Play)
        {
            elasped += Time.deltaTime;
            if (elasped > targetTime)
            {
                EndAnim();
                elasped = targetTime = 0;
            }
        }
    }

    /// <summary> 
    /// 构建方法 </summary>
    public void CreatMethod(MethodSigle ms, GameObject go)
    {
        if (!ReferenceEquals(go, null))
        {
            bandinGameObject = go;
            anim = go.GetComponent<Animator>();
            if (anim == null)
                return;

            firstAnimName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

            showAllInfo.text = ms.Info;
        }

        m_AudioManager = AudioManager.Instance;
        DownLoaderMono.Instance.DownLoadAudio(ms.AudioUrl, a => audioClip = a);
    }

    public void ToggleFunc(bool at)
    {
        if (at)
            Active();
        else
            InActive();
    }

    public void OnSwichPanel(PracMainSwitch pom)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (pom == PracMainSwitch.Practice)
            PauseAnim();
    }

    /// <summary> 
    /// 激活时 </summary>
    private void Active()
    {
        if (ReferenceEquals(anim, null))
            return;
        anim.enabled = false;

        anim.Update(0f);
        ObjectControl.SetTarget(bandinGameObject.transform);
    }

    /// <summary> 
    /// 不激活时 </summary>
    private void InActive()
    {
        ObjectControl.SetDefault();
        EndAnim();
    }

    private void ResetAnim()
    {
        if (ReferenceEquals(anim, null))
            return;

        anim.Play(firstAnimName, -1, 0f);
        anim.Update(0f);
    }

    /// <summary> 
    /// 播放动画 </summary>
    private void PlayAnim()
    {
        if (animState != AnimState.Await)
            return;

        animState = AnimState.Play;

        ShowButton(false);

        ResetAnim();

        if (ReferenceEquals(anim, null))
            return;
        anim.enabled = true;
        anim.Play(firstAnimName);
        targetTime = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        m_AudioManager.PlayAudio(audioClip);
    }

    private void ContinueAnim()
    {
        if (animState != AnimState.Pause)
            return;
        animState = AnimState.Play;
        ShowButton(false);

        anim.enabled = true;
        m_AudioManager.ContinueAudio();
    }

    private void PauseAnim()
    {
        if (animState != AnimState.Play)
            return;

        anim.enabled = false;
        animState = AnimState.Pause;

        ShowButton(true);
        m_AudioManager.PauseAudio();
    }

    /// <summary>
    /// 点击切换按钮
    /// </summary>
    /// <param name="isPause">是否是暂停</param>
    private void ShowButton(bool isPause)
    {
        btnAnimPause.gameObject.SetActive(!isPause);
        btnAnimPlay.gameObject.SetActive(isPause);
    }

    private void EndAnim()
    {
        anim.enabled = false;
        animState = AnimState.Await;
        elasped = targetTime = 0;

        ShowButton(true);
    }

    public void OnReFound()
    {
        EndAnim();
    }
}