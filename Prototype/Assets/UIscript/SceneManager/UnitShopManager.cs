using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;



public class UnitShopManager : MonoBehaviour
{
    [SerializeField] private GameObject unitInfoObj;
    [SerializeField] private Sprite[] unitIconArray;
    [SerializeField] private Sprite[] unitIllustArray;
    [SerializeField] private GameObject settingObj;
    [SerializeField] private GameObject warningMessageObj;
    [SerializeField] private Sprite[] unitTypeIconArray;

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
    [SerializeField] private Image unitTypeIconImage;
    [SerializeField] private Text unitAtkGoldText;
    [SerializeField] private Text unitHpGoldText;

    private string[] unitInfoArray = { "거대한 도끼가 바람을 갈랐다.\n일격.\n제 몸집만한 도끼를 어깨에 짊어진 청년은,\n양단 된 마물 앞에서 태양을 닮은 눈부신 미소를 짓고 있었다.",
        "고블린이 소름끼치는 웃음을 흘리며 다가왔다.\n하지만 소녀는 꼬리를 살랑살랑 흔들며 미소지을 뿐.\n소녀를 얕잡아본 고블린은, 나무 몽둥이를 휘둘렀다.\n자신의 머리가 바닥에서 구르고 있는 것도 알지 못한 채.",
        "찐득한 어둠 속에서 새빨간 불꽃이 일었다.\n마치 유혹하듯 꼬리를 흔드는 불꽃을 보고, 망자는 발을 내딛는다.\n불길이 몸을 집어삼키는 것은 신경도 쓰지 않고,\n한 걸음. 한 걸음.",
        "오크 로드는 나무 위에서 자고있는 여인이 마음에 들지 않았다.\n자신의 영역 안에서 느껴지는 평화로움에 구역질이 일었다.\n쾅! 오크 로드의 거목같은 다리가 나무에 직격했다\n그 뒤에 남은 것은, 잔잔한 산들바람과 새의 지저귐 뿐이었다." };

    private string[] unitNameArray = { "길버트", "하나", "하람", "로젤리아" };
    private string[] unitSkillNameArray = { "특수 공격 : 흡수의 일격", "특수 공격 : 일렁이는 아지랑이", "특수공격 : 일어나는 업화", "특수공격 : 피어스" };
    private string[] unitPathArray = { "Warrior", "Assassin" , "Magician", "Archor" };
    private string[] statusPathArray = { "ATK", "EXP", "HP", "Level" };

    private string currentUnit;

    private void Start()
    {
        goldText.text = GetInfoDataToString("Gold") + " 골드";
        UpdateUnitPointText();
        ChangeUnitInfo(0);
        unitInfoObj.SetActive(true);
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
    public void BtnEvt_LoadMainScene()
    {
        LoadingSceneController.LoadScene("Main");
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
            if (currentUnit == unitPathArray[i]) return;
            ChangeUnitInfo(i);
        }
    }

    private void UpgradeUnitStatus(bool isAtk)
    {
        int value = isAtk ? Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]) + CheckCurrentUnit(isAtk)
            : Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"]) + CheckCurrentUnit(isAtk);

        if (!CheckCanUpgradeAtk(isAtk, value)) return;

        if (isAtk)
        {
            GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[0]}", value);
            atkText.text = "공격력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]
                + " / " + DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        }
        else
        {
            GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[2]}", value);
            hpText.text = "체력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] 
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
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"]);
        int exp = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]) + 1000;

        if(exp >= DataEquation.UnitMaxEXPToLevel(currentUnit))
        {
            if (level % 5 == 0)
            {
                if (!CheckUnitStatus()) return;
            }
            exp = exp - DataEquation.UnitMaxEXPToLevel(currentUnit);
            if (exp < 0) exp = exp * -1;
            level += 1;
            GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[3]}", level);
            GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[1]}", exp);
            int index = Array.IndexOf(unitPathArray, currentUnit);
            ChangeUnitData(index);
        }
        else
        {
            expText.text = $"{exp} / {DataEquation.UnitMaxEXPToLevel(currentUnit)}";
            expSlider.maxValue = DataEquation.UnitMaxEXPToLevel(currentUnit);
            expSlider.value = exp;
        }
        GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[1]}", exp);
    }
    private bool CheckUnitStatus()
    {
        int atk = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]);
        int hp = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"]);
        int maxAtk = DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        int maxHp = DataEquation.UnitMaxHpEquationToLevel(currentUnit);
        if (atk < maxAtk || hp < maxHp)
        {
            warningText.text = "골드 강화를 완료해야 합니다!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        return true;
    }
    private void ChangeUnitInfo(int i)
    {
        currentUnit = unitPathArray[i];
        unitTypeIconImage.sprite = unitTypeIconArray[i];
        profileImage.sprite = unitIconArray[i];
        infoText.text = unitInfoArray[i];
        nameText.text = unitNameArray[i];
        skillNameText.text = unitSkillNameArray[i];
        //illustImage.sprite = unitIllustArray[i];
        ChangeUnitData(i);
    }
    private void ChangeUnitData(int i)
    {
        atkText.text = "공격력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        expText.text = GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / " + DataEquation.UnitMaxEXPToLevel(currentUnit);
        hpText.text = "체력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + DataEquation.UnitMaxHpEquationToLevel(currentUnit);
        levelText.text = "LV : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
        expSlider.value = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
        expSlider.maxValue = DataEquation.UnitMaxEXPToLevel(currentUnit);
        unitAtkGoldText.text = $"공격력 강화\n{DataEquation.UnitUpgradeGoldToLeve(currentUnit)}골드";
        unitHpGoldText.text = $"체력 강화\n{DataEquation.UnitUpgradeGoldToLeve(currentUnit)}골드";
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
        int gold = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"]) - DataEquation.UnitUpgradeGoldToLeve(currentUnit);
        if (gold < 0)
        {
            warningText.text = "골드가 부족합니다!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        else
        {
            GameManager.Instance.FirebaseData.SaveData("Info", "Gold", gold);
            goldText.text = GetInfoDataToString("Gold") + " 골드";

            return true;
        }
    }
    private bool CheckHaveUnitPoint()
    {
        int point = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary[$"{currentUnit}Points"]);
        if (point <= 0)
        {
            warningText.text = "유닛 포인트가 부족합니다!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        else
        {
            point--;
            GameManager.Instance.FirebaseData.SaveData("Info", $"{currentUnit}Points", point);
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
            str = DataEquation.GetUnit(Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary[path]));
        }
        else
        {
            str = GameManager.Instance.FirebaseData.InfoDictionary[path].ToString();
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
