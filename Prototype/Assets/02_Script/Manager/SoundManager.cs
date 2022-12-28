using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource sfxAudio;

    [SerializeField] private AudioClip[] bgmClipArray;
    [SerializeField] private AudioClip[] sfxClipArray;
    public static SoundManager Instance { get => instance; }
    public AudioSource BgmAudio { get => bgmAudio; }
    public AudioSource SfxAudio { get => sfxAudio; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            LoadAudioData();
        }
        else
        {
            Destroy(this);
        }
    }
    /// <summary>
    /// 0 = 스타트씬, 1 = 메인씬, 2 = 스테이지섹션씬, 3 = 수인, 4 = 버섯, 5 = 스님, 6 = 가고일
    /// </summary>
    /// <param name="index"></param>
    public void SetBGM(int index)
    {
        bgmAudio.Pause();
        bgmAudio.clip = bgmClipArray[index];
        bgmAudio.Play();
    }
    /// <summary>
    /// 0 = 던전 입장 효과음
    /// </summary>
    /// <param name="index"></param>
    public void SetSFX(int index)
    {
        sfxAudio.clip = sfxClipArray[index];
        sfxAudio.Play();
    }
    public void SetVolume(bool isBGM, float value)
    {
        if (isBGM)
        {
            bgmAudio.volume = value;
        }
        else
        {
            sfxAudio.volume = value;
        }
        SaveAudioData(isBGM);
    }
    public void SetMute(bool isBGM, bool isOn)
    {
        if (isBGM)
        {
            bgmAudio.mute = isOn;
        }
        else
        {
            sfxAudio.mute = isOn;
        }
        SaveMuteData(isBGM);
    }
    private void SaveAudioData(bool isBGM)
    {
        if (isBGM)
        {
            PlayerPrefs.SetFloat("BGM", bgmAudio.volume);
        }
        else
        {
            PlayerPrefs.SetFloat("SFX", sfxAudio.volume);
        }
    }
    private void SaveMuteData(bool isBGM)
    {
        if (isBGM)
        {
            PlayerPrefs.SetString("BGM_Mute", bgmAudio.mute.ToString());
        }
        else
        {
            PlayerPrefs.SetString("SFX_Mute", sfxAudio.mute.ToString());
        }
    }
    private void LoadAudioData()
    {
        bgmAudio.volume = PlayerPrefs.GetFloat("BGM", 0.5f);
        bgmAudio.mute = System.Convert.ToBoolean(PlayerPrefs.GetString("BGM_Mute", "false"));
        sfxAudio.volume = PlayerPrefs.GetFloat("SFX", 0.5f);
        sfxAudio.mute = System.Convert.ToBoolean(PlayerPrefs.GetString("SFX_Mute", "false"));
    }
}
