using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETC
{
    public class SettingObject : MonoBehaviour
    {
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle bgmToggle;
        [SerializeField] private Toggle sfxToggle;
        [SerializeField] private Text bgmValueText;
        [SerializeField] private Text sfxValueText;

        private void Awake()
        {
            bgmSlider.value = SoundManager.Instance.BgmAudio.volume * 10;
            sfxSlider.value = SoundManager.Instance.SfxAudio.volume * 10;
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
                SoundManager.Instance.SetVolume(isBGM, bgmSlider.value / 10);
                if(bgmSlider.value <= 0)
                {
                    bgmToggle.isOn = false;
                }
                else
                {
                    bgmToggle.isOn = true;
                }
                SoundManager.Instance.SetMute(true, !bgmToggle.isOn);
            }
            else
            {
                SoundManager.Instance.SetVolume(isBGM, sfxSlider.value / 10);
                if (sfxSlider.value <= 0)
                {
                    sfxToggle.isOn = false;
                }
                else
                {
                    sfxToggle.isOn = true;
                }
                SoundManager.Instance.SetMute(false, !sfxToggle.isOn);
            }
            //StartCoroutine(Co_ChangeSoundValueText(isBGM));
        }
        public void ToggleEvt_SetVolume(Toggle toggle)
        {
            Debug.Log("토글 이벤트 호출");
            if (toggle.transform.parent.name.Contains("BGM"))
            {
                SoundManager.Instance.SetMute(true, !toggle.isOn);
                bgmSlider.value = toggle.isOn? 1 : 0;
                SoundManager.Instance.SetVolume(true, bgmSlider.value / 10);
            }
            else
            {
                SoundManager.Instance.SetMute(false, !toggle.isOn);
                sfxSlider.value = toggle.isOn ? 1 : 0;
                SoundManager.Instance.SetVolume(false, sfxSlider.value / 10);
            }
        }
        public void PointerUp_ChangeSoundValueText(bool isBGM)
        {
            StartCoroutine(Co_ChangeSoundValueText(isBGM));
        }
        private IEnumerator Co_ChangeSoundValueText(bool isBGM)
        {
            if (isBGM)
            {
                bgmValueText.text = bgmSlider.value.ToString();
                bgmValueText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                bgmValueText.gameObject.SetActive(false);
            }
            else
            {
                sfxValueText.text = sfxSlider.value.ToString();
                sfxValueText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                sfxValueText.gameObject.SetActive(false);
            }
        }
    }
}
