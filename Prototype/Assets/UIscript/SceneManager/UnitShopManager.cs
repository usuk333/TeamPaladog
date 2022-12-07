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

    private string[] unitInfoArray = { "�Ŵ��� ������ �ٶ��� ������.\n�ϰ�.\n�� �������� ������ ����� ������ û����,\n��� �� ���� �տ��� �¾��� ���� ���ν� �̼Ҹ� ���� �־���.",
        "����� �Ҹ���ġ�� ������ �긮�� �ٰ��Դ�.\n������ �ҳ�� ������ ������ ���� �̼����� ��.\n�ҳฦ ����ƺ� �����, ���� �����̸� �ֵѷ���.\n�ڽ��� �Ӹ��� �ٴڿ��� ������ �ִ� �͵� ���� ���� ä.",
        "����� ��� �ӿ��� ������ �Ҳ��� �Ͼ���.\n��ġ ��Ȥ�ϵ� ������ ���� �Ҳ��� ����, ���ڴ� ���� ����´�.\n�ұ��� ���� �����Ű�� ���� �Ű浵 ���� �ʰ�,\n�� ����. �� ����.",
        "��ũ �ε�� ���� ������ �ڰ��ִ� ������ ������ ���� �ʾҴ�.\n�ڽ��� ���� �ȿ��� �������� ��ȭ�ο� �������� �Ͼ���.\n��! ��ũ �ε��� �Ÿ��� �ٸ��� ������ �����ߴ�\n�� �ڿ� ���� ����, ������ ���ٶ��� ���� ������ ���̾���." };

    private string[] unitNameArray = { "��Ŀ - ���Ʈ", "�ϻ��� - �ϳ�", "������ - �϶�", "�ü� - ��������" };
    private string[] unitSkillNameArray = { "Ư�� ���� : ����� �ϰ�", "Ư�� ���� : �Ϸ��̴� ��������", "Ư������ : �Ͼ�� ��ȭ", "Ư������ : �Ǿ" };
    private string[] unitPathArray = { "Warrior", "Assassin" , "Magician", "Archor" };
    private string[] statusPathArray = { "ATK", "EXP", "HP", "Level" };

    private string currentUnit;

    private void Start()
    {
        goldText.text = GetInfoDataToString("Gold") + " ���";
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
        int exp = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]) + 100;

        if(exp >= 500)
        {
            exp = 0;
            int level = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"]) + 1;
            GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[3]}", level);
            int index = Array.IndexOf(unitPathArray, currentUnit);
            ChangeUnitData(index);
        }
        Debug.Log(exp);
        GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[1]}", exp);
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
        atkText.text = "���ݷ� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + DataEquation.UnitMaxAtkEquationToLevel(currentUnit);
        expText.text = GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / 500";
        hpText.text = "ü�� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + DataEquation.UnitMaxHpEquationToLevel(currentUnit);
        levelText.text = "LV : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
        expSlider.value = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
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
        int gold = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"]) - 200;
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
            warningText.text = "���ļ��� �����մϴ�!";
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
