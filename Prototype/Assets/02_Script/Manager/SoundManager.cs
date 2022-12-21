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
    /// 0 = ��ŸƮ��, 1 = ���ξ�, 2 = �����������Ǿ�, 3 = ����, 4 = ����, 5 = ����, 6 = ������
    /// </summary>
    /// <param name="index"></param>
    public void SetBGM(int index)
    {
        bgmAudio.Pause();
        bgmAudio.clip = bgmClipArray[index];
        bgmAudio.Play();
    }
}
