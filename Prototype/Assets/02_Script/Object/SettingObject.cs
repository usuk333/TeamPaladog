using UnityEngine;
using UnityEngine.UI;

public class SettingObject : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSpriteArray;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Image soundButtonImage;
    [SerializeField] private Image accountButtonImage;

    [SerializeField] private GameObject currentFrameObj;
    private void Awake()
    {
        bgmSlider.value = SoundManager.Instance.BgmAudio.volume;
        sfxSlider.value = SoundManager.Instance.SfxAudio.volume;
        bgmToggle.isOn = !SoundManager.Instance.BgmAudio.mute;
        sfxToggle.isOn = !SoundManager.Instance.SfxAudio.mute;
    }
    public void BtnEvt_ActiveObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public void BtnEvt_Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void SliderEvt_SetVolume(bool isBGM)
    {
        if (isBGM)
        {
            SoundManager.Instance.SetVolume(isBGM, bgmSlider.value);
        }
        else
        {
            SoundManager.Instance.SetVolume(isBGM, sfxSlider.value);
        }
    }
    public void ToggleEvt_SetVolume(Toggle toggle)
    {
        if (toggle.transform.parent.name.Contains("BGM"))
        {
            SoundManager.Instance.SetMute(true, !toggle.isOn);
        }
        else
        {
            SoundManager.Instance.SetMute(false, !toggle.isOn);
        }
    }
    public void BtnEvt_ActiveFrame(GameObject obj)
    {
        if (currentFrameObj == obj) return;

        currentFrameObj.SetActive(false);
        obj.SetActive(true);
        currentFrameObj = obj;
        ChangeButtonSprite();
    }



    private void ChangeButtonSprite()
    {
        if (currentFrameObj.name.Contains("Sound"))
        {
            soundButtonImage.sprite = buttonSpriteArray[0];
            accountButtonImage.sprite = buttonSpriteArray[1];
        }
        else
        {
            soundButtonImage.sprite = buttonSpriteArray[1];
            accountButtonImage.sprite = buttonSpriteArray[0];
        }
    }
}
