using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

using DG.Tweening;


public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject unitInfoObj;
    [SerializeField] private Sprite[] unitIconArray;
    [SerializeField] private string[] unitInfoArray;
    [SerializeField] private Sprite[] unitIllustArray;
    [SerializeField] private GameObject settingObj;
    [SerializeField] private GameObject warningMessageObj;

    [Header("변경될 UI 요소들")]
    [SerializeField] private Image profileImage;
    [SerializeField] private Text levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Text expText;
    [SerializeField] private Text atkText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text infoText;
    [SerializeField] private Image illustImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillInfoText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text[] unitPointTextArray;
    [SerializeField] private Text warningText;

    private string[] unitNameArray = { "탱커 - 길버트", "암살자 - 하나", "마법사 - 하람", "궁수 - 로젤리아" };
    private string[] unitSkillNameArray = { "특수 공격 : 흡수의 일격", "특수 공격 : 일렁이는 아지랑이", "특수공격 : 일어나는 업화", "특수공격 : 피어스" };
    private string[] unitPathArray = { "Warrior", "Assassin" , "Magician", "Archor" };
    private string[] statusPathArray = { "ATK", "EXP", "HP", "Level" };

    private string currentUnit;

    private void Start()
    {
        goldText.text = GetInfoDataToString("Gold") + " 골드";
        UpdateUnitPointText();
    }
    public void BtnEvt_UseUnitPoint()
    {
        if (!CheckHaveUnitPoint()) return;
        UpdateUnitExp();
    }
    public void BtnEvt_UpgradeAtk()
    {
        if (!CheckHaveGold()) return;

        UpgradeUnitStatus(true);
    }
    public void BtnEvt_UpgradeHp()
    {
        if (!CheckHaveGold()) return;

        UpgradeUnitStatus(false);
    }
    public void BtnEvt_GoMain()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
    public void BtnEvt_ActiveSettingObj()
    {
        settingObj.SetActive(!settingObj.activeSelf);
    }
    public void BtnEvt_ActiveUnitInfoObj(int i)
    {
        if (!unitInfoObj.activeSelf)
        {
            unitInfoObj.SetActive(!unitInfoObj.activeSelf);
        }
        if (unitInfoObj.activeSelf)
        {
            ChangeUnitInfo(i);
        }
    }

    private void UpgradeUnitStatus(bool isAtk)
    {
        int value = isAtk ? Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]) + CheckCurrentUnit(isAtk)
            : Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"]) + CheckCurrentUnit(isAtk);

        if (!CheckCanUpgradeAtk(isAtk, value)) return;

        if (isAtk)
        {
            StartManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[0]}", value);
            atkText.text = "공격력 : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]
                + " / " + DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        }
        else
        {
            StartManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[2]}", value);
            hpText.text = "체력 : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] 
                + " / " + DataEquation.UnitMaxHpEquationToLevel(currentUnit);
        }
    }
    private void UpdateUnitPointText()
    {
        for (int i = 0; i < unitPointTextArray.Length; i++)
        {
            unitPointTextArray[i].text = GetInfoDataToString($"{unitPathArray[i]}Points");
        }
    }
    private void UpdateUnitExp()
    {
        int exp = Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]) + 100;

        if(exp >= 500)
        {
            exp = 0;
            int level = Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"]) + 1;
            StartManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[3]}", level);
            int index = Array.IndexOf(unitPathArray, currentUnit);
            ChangeUnitData(index);
        }
        Debug.Log(exp);
        StartManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[1]}", exp);
        expText.text = $"{exp} / 500";
        expSlider.value = exp;
    }
    private void ChangeUnitInfo(int i)
    {
        currentUnit = unitPathArray[i];
        profileImage.sprite = unitIconArray[i];
        infoText.text = unitInfoArray[i];
        nameText.text = unitNameArray[i];
        skillNameText.text = unitSkillNameArray[i];
        //illustImage.sprite = unitIllustArray[i];
        ChangeUnitData(i);
    }
    private void ChangeUnitData(int i)
    {
        atkText.text = "공격력 : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        expText.text = StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / 500";
        hpText.text = "체력 : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + DataEquation.UnitMaxHpEquationToLevel(currentUnit);
        levelText.text = "LV : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
        expSlider.value = Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
        ChangeSkillInfo(i);
    }
    private void ChangeSkillInfo(int i)
    {
        switch (i)
        {
            case 0:
                skillInfoText.text = $"전설의 도끼 '파라슈'의 힘으로 상대를 내려쳐({DataEquation.UnitSkillConditionToLevel(currentUnit)})번 째 공격마다 공격력(+150%) 만큼의 피해를 입히고 공격력(+200%) 만큼의 체력을 회복합니다";
                break;
            case 1:
                skillInfoText.text = $"아지랑이가 일만큼 빠르게 상대를 베어내 ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) 확률로 공격력(+200%) 만큼의 피해를 입힙니다.";
                break;
            case 2:
                skillInfoText.text = $"강력한 마법으로 상대를 불살라 ({DataEquation.UnitSkillConditionToLevel(currentUnit)})번 째 공격마다 공격력(+400%) 만큼의 피해를 입힙니다.";
                break;
            case 3:
                skillInfoText.text = $"강력한 가시로 상대를 꿰뚫어 ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) 확률로 공격력(+180%) 만큼의 피해를 입힙니다.";
                break;
            default:
                break;
        }
    }
    private bool CheckCanUpgradeAtk(bool isAtk, int value)
    {
        if (isAtk)
        {
            if (value > DataEquation.UnitMaxAtkEquationToLevel(currentUnit))
            {
                warningText.text = "이미 최대치 입니다!";
                StartCoroutine(Co_WarningMessageAnim());
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (value > DataEquation.UnitMaxHpEquationToLevel(currentUnit))
            {
                warningText.text = "이미 최대치 입니다!";
                StartCoroutine(Co_WarningMessageAnim());
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    private bool CheckHaveGold()
    {
        int gold = Convert.ToInt32(StartManager.Instance.FirebaseData.InfoDictionary["Gold"]) - 200;
        if (gold < 0)
        {
            warningText.text = "골드가 부족합니다!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        else
        {
            StartManager.Instance.FirebaseData.SaveData("Info", "Gold", gold);
            goldText.text = GetInfoDataToString("Gold") + " 골드";

            return true;
        }
    }
    private bool CheckHaveUnitPoint()
    {
        int point = Convert.ToInt32(StartManager.Instance.FirebaseData.InfoDictionary[$"{currentUnit}Points"]);
        if (point <= 0)
        {
            warningText.text = "돌파석이 부족합니다!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        else
        {
            point--;
            StartManager.Instance.FirebaseData.SaveData("Info", $"{currentUnit}Points", point);
            UpdateUnitPointText();

            return true;
        }
    }
    private int CheckCurrentUnit(bool isAtk)
    {
        int value;
        switch (currentUnit)
        {
            case "Warrior":
                value = isAtk ? 8 : 75;
                break;
            case "Assassin":
                value = isAtk ? 8 : 50;
                break;
            case "Magician":
                value = isAtk ? 12 : 40;
                break;
            case "Archor":
                value = isAtk ? 10 : 40;
                break;
            default:
                value = 0;
                break;
        }
        return value;
    }
    private string GetInfoDataToString(string path)
    {
        string str;

        if(path == "Gold")
        {
            str = DataEquation.GetUnit(Convert.ToInt32(StartManager.Instance.FirebaseData.InfoDictionary[path]));
        }
        else
        {
            str = StartManager.Instance.FirebaseData.InfoDictionary[path].ToString();
        }
        return str;
    }
    private IEnumerator Co_WarningMessageAnim()
    {
        warningMessageObj.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        warningMessageObj.SetActive(false);
    }
}
