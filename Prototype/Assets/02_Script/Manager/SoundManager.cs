using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioClip[] bgmClipArray;
    public static SoundManager Instance { get => instance; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
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
}
