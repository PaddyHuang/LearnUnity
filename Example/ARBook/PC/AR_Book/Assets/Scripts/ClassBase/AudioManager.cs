using UnityEngine;

//控制动画播放的状态
public enum AnimState
{
    Await,
    Pause,
    Play
}

public class AudioManager : MonoBehaviour
{
    #region Mono-Singleton

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<AudioManager>();
            return instance;
        }
    }

    #endregion

    [SerializeField] private AudioSource m_AudioSource;

    private float saveCurtTime;

    public void PlayAudio(AudioClip ac)
    {
        if (!ReferenceEquals(ac, null))
        {
            m_AudioSource.clip = null;
            m_AudioSource.clip = ac;
            m_AudioSource.Play();
        }
    }

    public void PauseAudio()
    {
        saveCurtTime = m_AudioSource.time;
        m_AudioSource.Pause();
    }

    public void ContinueAudio()
    {
        if (m_AudioSource.clip != null)
            m_AudioSource.PlayScheduled(saveCurtTime);
    }
}