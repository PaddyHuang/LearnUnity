using UnityEngine;
using UnityEngine.UI;

public class TableTemplate : MonoBehaviour
{
    [SerializeField] private Image bgImage, dyImage;
    [SerializeField] private Button playBtn, pauseBtn;

    private GameObject bindObject;
    private Animator bindAnim;

    private AudioClip m_Audio;
    private AudioManager m_AudioManager;

    private float elapsed, animLength;

    private AnimState animState = AnimState.Await;

    private string playAnimName;

    void Start()
    {
        m_AudioManager = AudioManager.Instance;

        playBtn.onClick.AddListener(StartAnim);
        playBtn.onClick.AddListener(ContinueAnim);
        pauseBtn.onClick.AddListener(PauseAnim);
    }

    private void Update()
    {
        if (animState == AnimState.Play)
        {
            elapsed += Time.deltaTime;
            float c = Mathf.Lerp(0, 1, elapsed / animLength);

            dyImage.fillAmount = c;

            if (c >= 1f)
                animState = AnimState.Await;
        }
    }

    private void StartAnim()
    {
        if (animState != AnimState.Await)
            return;

        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);

        if (bindAnim.enabled == false)
            bindAnim.enabled = true;
        bindAnim.Play(playAnimName, -1, 0f);
        bindAnim.Update(0f);
        dyImage.fillAmount = 0;

        animState = AnimState.Play;

        m_AudioManager.PlayAudio(m_Audio);
    }

    private void ContinueAnim()
    {
        if (animState != AnimState.Pause)
            return;

        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);

        animState = AnimState.Play;

        m_AudioManager.ContinueAudio();
        bindAnim.speed = 1;
    }

    private void PauseAnim()
    {
        if (animState != AnimState.Play)
            return;

        animState = AnimState.Pause;
        bindAnim.speed = 0;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        m_AudioManager.PauseAudio();
    }

    public void CreatBgImage(Sprite sp)
    {
        if (!ReferenceEquals(bgImage, null) && !ReferenceEquals(sp, null))
        {
            bgImage.sprite = sp;
            bgImage.SetNativeSize();
        }
    }

    /// <summary> 
    /// 设置动态图像的状态 </summary>
    public void CreatDyImage(Sprite sp)
    {
        if (!ReferenceEquals(dyImage, null) && !ReferenceEquals(sp, null))
        {
            dyImage.sprite = sp;
            dyImage.SetNativeSize();
            dyImage.type = Image.Type.Filled;
            dyImage.fillMethod = Image.FillMethod.Horizontal;
            dyImage.fillOrigin = 0;
            dyImage.fillAmount = 0;
        }
    }

    /// <summary> 
    /// 设置绑定物体 </summary>
    public void SetBindGameObject(GameObject target)
    {
        if (!ReferenceEquals(target, null))
        {
            GameObject goo = target;

            goo.transform.position = Vector3.zero;

            bindObject = goo;
            bindAnim = bindObject.GetComponent<Animator>();
            if (bindAnim == null)
            {
                Debug.LogError("anim is null");
                return;
            }

            playAnimName = bindAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

            animLength = bindAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }
    }

    /// <summary> 
    /// 设置绑定物体 </summary>
    public void SetBindGameObject(GameObject target, bool isShow)
    {
        if (!ReferenceEquals(target, null))
        {
            GameObject goo = target;

            bindObject = goo;
            bindAnim = bindObject.GetComponent<Animator>();
            if (bindAnim == null)
            {
                Debug.LogError("anim is null");
                return;
            }

            playAnimName = bindAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

            animLength = bindAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length;

            goo.SetActive(isShow);
        }
    }

    /// <summary> 
    /// 绑定音乐 </summary>
    public void SetAudio(string url)
    {
        DownLoaderMono.Instance.DownLoadAudio(url, a => m_Audio = a);
    }

    public void ToggleFunc(bool b)
    {
        bindObject.SetActive(b);
        if (!b)
        {
            ObjectControl.SetDefault();

            m_AudioManager.PauseAudio();
        }
        else
        {
            if (bindAnim != null)
            {
                bindAnim.enabled = false;
                bindAnim.Update(0f);
                ResetAnim();
            }

            ObjectControl.SetTarget(bindObject.transform);
        }

        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }

    /// <summary> 
    /// 切换暂停按钮 </summary>
    public void OnSwichPanel(PracMainSwitch pom)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (pom == PracMainSwitch.Practice)
            PauseAnim();
    }

    private void ResetAnim()
    {
        if (bindAnim != null)
        {
            bindAnim.Play(playAnimName, -1, 0f);
            bindAnim.Update(0f);
        }

        animState = AnimState.Await;
        dyImage.fillAmount = 0;
        elapsed = 0;
    }

    /// <summary> 
    /// 当重新识别时 </summary>
    public void OnReFound()
    {
        ResetAnim();
    }
}