using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Text[] nicknameText;
    [SerializeField] private Slider[] expSlider;
    [SerializeField] private Text[] levelText;
    [SerializeField] private Text[] expText;
    private void Start()
    {
        ChangePlayerInfo();
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
