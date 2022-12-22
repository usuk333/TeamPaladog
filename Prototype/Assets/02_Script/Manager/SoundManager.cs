using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource sfxAudio;

    [SerializeField] private AudioClip[] bgmClipArray;
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
    /// 0 = ½ºÅ¸Æ®¾À, 1 = ¸ÞÀÎ¾À, 2 = ½ºÅ×ÀÌÁö¼½¼Ç¾À, 3 = ¼öÀÎ, 4 = ¹ö¼¸, 5 = ½º´Ô, 6 = °¡°íÀÏ
    /// </summary>
    /// <param name="index"></param>
    public void SetBGM(int index)
    {
        bgmAudio.Pause();
        bgmAudio.clip = bgmClipArray[index];
        bgmAudio.Play();
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
