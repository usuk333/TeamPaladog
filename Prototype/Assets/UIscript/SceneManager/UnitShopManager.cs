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

    [Header("����� UI ��ҵ�")]
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

    private string[] unitInfoArray = { "�Ŵ��� ������ �ٶ��� ������.\n�ϰ�.\n�� �������� ������ ����� ������ û����,\n��� �� ���� �տ��� �¾��� ���� ���ν� �̼Ҹ� ���� �־���.",
        "����� �Ҹ���ġ�� ������ �긮�� �ٰ��Դ�.\n������ �ҳ�� ������ ������ ���� �̼����� ��.\n�ҳฦ ����ƺ� �����, ���� �����̸� �ֵѷ���.\n�ڽ��� �Ӹ��� �ٴڿ��� ������ �ִ� �͵� ���� ���� ä.",
        "����� ��� �ӿ��� ������ �Ҳ��� �Ͼ���.\n��ġ ��Ȥ�ϵ� ������ ���� �Ҳ��� ����, ���ڴ� ���� ����´�.\n�ұ��� ���� �����Ű�� ���� �Ű浵 ���� �ʰ�,\n�� ����. �� ����.",
        "��ũ �ε�� ���� ������ �ڰ��ִ� ������ ������ ���� �ʾҴ�.\n�ڽ��� ���� �ȿ��� �������� ��ȭ�ο� �������� �Ͼ���.\n��! ��ũ �ε��� �Ÿ��� �ٸ��� ������ �����ߴ�\n�� �ڿ� ���� ����, ������ ���ٶ��� ���� ������ ���̾���." };

    private string[] unitNameArray = { "���Ʈ", "�ϳ�", "�϶�", "��������" };
    private string[] unitSkillNameArray = { "Ư�� ���� : ����� �ϰ�", "Ư�� ���� : �Ϸ��̴� ��������", "Ư������ : �Ͼ�� ��ȭ", "Ư������ : �Ǿ" };
    private string[] unitPathArray = { "Warrior", "Assassin" , "Magician", "Archor" };
    private string[] statusPathArray = { "ATK", "EXP", "HP", "Level" };

    private string currentUnit;

    private void Start()
    {
        goldText.text = GetInfoDataToString("Gold") + " ���";
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
            atkText.text = "���ݷ� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]
                + " / " + DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        }
        else
        {
            GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[2]}", value);
            hpText.text = "ü�� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] 
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
            warningText.text = "��� ��ȭ�� �Ϸ��ؾ� �մϴ�!";
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
        atkText.text = "���ݷ� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        expText.text = GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / " + DataEquation.UnitMaxEXPToLevel(currentUnit);
        hpText.text = "ü�� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + DataEquation.UnitMaxHpEquationToLevel(currentUnit);
        levelText.text = "LV : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
        expSlider.value = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
        expSlider.maxValue = DataEquation.UnitMaxEXPToLevel(currentUnit);
        unitAtkGoldText.text = $"���ݷ� ��ȭ\n{DataEquation.UnitUpgradeGoldToLeve(currentUnit)}���";
        unitHpGoldText.text = $"ü�� ��ȭ\n{DataEquation.UnitUpgradeGoldToLeve(currentUnit)}���";
        ChangeSkillInfo(i);
    }
    private void ChangeSkillInfo(int i)
    {
        switch (i)
        {
            case 0:
                skillInfoText.text = $"������ ���� '�Ķ�'�� ������ ��븦 ������({DataEquation.UnitSkillConditionToLevel(currentUnit)})�� ° ���ݸ��� ���ݷ�(+150%) ��ŭ�� ���ظ� ������ ���ݷ�(+200%) ��ŭ�� ü���� ȸ���մϴ�";
                break;
            case 1:
                skillInfoText.text = $"�������̰� �ϸ�ŭ ������ ��븦 ��� ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) Ȯ���� ���ݷ�(+200%) ��ŭ�� ���ظ� �����ϴ�.";
                break;
            case 2:
                skillInfoText.text = $"������ �������� ��븦 �һ�� ({DataEquation.UnitSkillConditionToLevel(currentUnit)})�� ° ���ݸ��� ���ݷ�(+400%) ��ŭ�� ���ظ� �����ϴ�.";
                break;
            case 3:
                skillInfoText.text = $"������ ���÷� ��븦 ��վ� ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) Ȯ���� ���ݷ�(+180%) ��ŭ�� ���ظ� �����ϴ�.";
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
                warningText.text = "�̹� �ִ�ġ �Դϴ�!";
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
                warningText.text = "�̹� �ִ�ġ �Դϴ�!";
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
            warningText.text = "��尡 �����մϴ�!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        else
        {
            GameManager.Instance.FirebaseData.SaveData("Info", "Gold", gold);
            goldText.text = GetInfoDataToString("Gold") + " ���";

            return true;
        }
    }
    private bool CheckHaveUnitPoint()
    {
        int point = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary[$"{currentUnit}Points"]);
        if (point <= 0)
        {
            warningText.text = "���� ����Ʈ�� �����մϴ�!";
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
