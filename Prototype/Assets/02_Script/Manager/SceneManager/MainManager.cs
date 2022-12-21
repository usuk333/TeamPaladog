using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class MainManager : MonoBehaviour
{
    [Header("변경되는 UI 요소")]
    [SerializeField] private Text[] nicknameText;
    [SerializeField] private Slider[] expSlider;
    [SerializeField] private Text[] levelText;
    [SerializeField] private Text[] expText;
    [SerializeField] private GameObject exitPopUpObj;

    private void Start()
    {
        ChangePlayerInfo();
    }
    public void BtnEvt_ExitGame()
    {
        exitPopUpObj.SetActive(!exitPopUpObj.activeSelf);
    }
    public void BtnEvt_Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void BtnEvt_ActiveObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public void BtnEvt_LoadUnitScene()
    {
        LoadingSceneController.LoadScene("UnitShop");
    }
    public void BtnEvt_LoadSkillScene()
    {
        LoadingSceneController.LoadScene("Skill");
    }
    public void BtnEvt_LoadStageSectionScene()
    {
        LoadingSceneController.LoadScene("StageSection");
        SoundManager.Instance.SetBGM(2);
    }
    private void ChangePlayerInfo()
    {
        for (int i = 0; i < nicknameText.Length; i++)
        {
            nicknameText[i].text = GameManager.Instance.FirebaseData.InfoDictionary["Nickname"].ToString();
            levelText[i].text = GameManager.Instance.FirebaseData.InfoDictionary["Level"].ToString();
            expSlider[i].maxValue = DataEquation.PlayerMaxExpToLevel();
            expSlider[i].value = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["EXP"]);
            expText[i].text = $"{expSlider[i].value} / {expSlider[i].maxValue}";
        }
    }
}
