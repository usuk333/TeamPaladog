using UnityEngine;
using UnityEngine.UI;
using System;
using ETC;

namespace UnitShopScene
{
    public class UnitInfo : MonoBehaviour
    {
        [SerializeField] private Sprite[] unitIconArray;
        [SerializeField] private Sprite[] unitIllustArray;
        [SerializeField] private Sprite[] unitTypeIconArray;

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
        [SerializeField] private Image unitTypeIconImage;
        [SerializeField] private Text unitAtkGoldText;
        [SerializeField] private Text unitHpGoldText;

        [SerializeField] private StatusBar[] statusBarArray;
        [SerializeField] private WarningMessage warningMessage;

        private string[] unitInfoArray = { "�Ŵ��� ������ �ٶ��� ������.\n�ϰ�.\n�� �������� ������ ����� ������ û����,\n��� �� ���� �տ��� �¾��� ���� ���ν� �̼Ҹ� ���� �־���.",
        "����� �Ҹ���ġ�� ������ �긮�� �ٰ��Դ�.\n������ �ҳ�� ������ ������ ���� �̼����� ��.\n�ҳฦ ����ƺ� �����, ���� �����̸� �ֵѷ���.\n�ڽ��� �Ӹ��� �ٴڿ��� ������ �ִ� �͵� ���� ���� ä.",
        "����� ��� �ӿ��� ������ �Ҳ��� �Ͼ���.\n��ġ ��Ȥ�ϵ� ������ ���� �Ҳ��� ����, ���ڴ� ���� ����´�.\n�ұ��� ���� �����Ű�� ���� �Ű浵 ���� �ʰ�,\n�� ����. �� ����.",
        "��ũ �ε�� ���� ������ �ڰ��ִ� ������ ������ ���� �ʾҴ�.\n�ڽ��� ���� �ȿ��� �������� ��ȭ�ο� �������� �Ͼ���.\n��! ��ũ �ε��� �Ÿ��� �ٸ��� ������ �����ߴ�\n�� �ڿ� ���� ����, ������ ���ٶ��� ���� ������ ���̾���." };

        private string[] unitNameArray = { "���Ʈ", "�ϳ�", "�϶�", "��������" };
        private string[] unitSkillNameArray = { "Ư�� ���� : ����� �ϰ�", "Ư�� ���� : �Ϸ��̴� ��������", "Ư������ : �Ͼ�� ��ȭ", "Ư������ : �Ǿ" };
        private string[] unitPathArray = { "Warrior", "Assassin", "Magician", "Archor" };
        private string[] statusPathArray = { "ATK", "EXP", "HP", "Level" };

        private float[] illustPosX = { -197, -216, -216, -214 };

        private string currentUnit;
        public void ChangeUnitInfo(int i)
        {
            currentUnit = unitPathArray[i];
            unitTypeIconImage.sprite = unitTypeIconArray[i];
            profileImage.sprite = unitIconArray[i];
            infoText.text = unitInfoArray[i];
            nameText.text = unitNameArray[i];
            skillNameText.text = unitSkillNameArray[i];
            illustImage.sprite = unitIllustArray[i];
            illustImage.transform.localPosition = new Vector2(illustPosX[i], illustImage.transform.localPosition.y);
            ChangeUnitData(i);
        }
        public bool CheckHaveGold()
        {
            int gold = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"]) - GameManager.Instance.FirebaseData.GetUnitUpgradeGoldByLeve(currentUnit);
            if (gold < 0)
            {
                warningMessage.SetWarningText("��尡 �����մϴ�!");
                return false;
            }
            else
            {
                GameManager.Instance.FirebaseData.SaveData("Info", "Gold", gold);
                statusBarArray[0].UpdateValueText();
                return true;
            }
        }
        public bool CheckHaveUnitPoint()
        {
            int point = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary[$"{currentUnit}Points"]);
            if (point <= 0)
            {
                warningMessage.SetWarningText("�迭 ����Ʈ�� �����մϴ�!");
                return false;
            }
            else
            {
                point--;
                GameManager.Instance.FirebaseData.SaveData("Info", $"{currentUnit}Points", point);
                statusBarArray[Array.IndexOf(unitPathArray, currentUnit) + 1].UpdateValueText();
                return true;
            }
        }
        public void UpgradeUnitStatus(bool isAtk)
        {
            int value = isAtk ? Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]) + CheckCurrentUnit(isAtk)
                : Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"]) + CheckCurrentUnit(isAtk);

            if (!CheckCanUpgradeAtk(isAtk, value)) return;

            if (isAtk)
            {
                GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[0]}", value);
                atkText.text = "���ݷ� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]
                    + " / " + GameManager.Instance.FirebaseData.GetUnitMaxAtkByLevel(currentUnit);
            }
            else
            {
                GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[2]}", value);
                hpText.text = "ü�� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"]
                    + " / " + GameManager.Instance.FirebaseData.GetUnitMaxHpByLevel(currentUnit);
            }
        }
        public void UpdateUnitExp()
        {
            int level = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"]);
            int exp = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]) + 1000;

            if (exp >= GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit))
            {
                if (level % 5 == 0)
                {
                    if (!CheckUnitStatus()) return;
                }
                exp = exp - GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit);
                if (exp < 0) exp = exp * -1;
                level += 1;
                GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[3]}", level);
                GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[1]}", exp);
                int index = Array.IndexOf(unitPathArray, currentUnit);
                ChangeUnitData(index);
            }
            else
            {
                expText.text = $"{exp} / {GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit)}";
                expSlider.maxValue = GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit);
                expSlider.value = exp;
            }
            GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[1]}", exp);
        }
        private void ChangeUnitData(int i)
        {
            atkText.text = "���ݷ� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + GameManager.Instance.FirebaseData.GetUnitMaxAtkByLevel(currentUnit);
            expText.text = GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / " + GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit);
            hpText.text = "ü�� : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + GameManager.Instance.FirebaseData.GetUnitMaxHpByLevel(currentUnit);
            levelText.text = "Lv. " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
            expSlider.value = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
            expSlider.maxValue = GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit);
            unitAtkGoldText.text = $"���ݷ� ��ȭ\n{GameManager.Instance.FirebaseData.GetUnitUpgradeGoldByLeve(currentUnit)}���";
            unitHpGoldText.text = $"ü�� ��ȭ\n{GameManager.Instance.FirebaseData.GetUnitUpgradeGoldByLeve(currentUnit)}���";
            ChangeSkillInfo(i);
        }
        private void ChangeSkillInfo(int i)
        {
            switch (i)
            {
                case 0:
                    skillInfoText.text = $"������ ���� '�Ķ�'�� ������ ��븦 ������({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)})�� ° ���ݸ��� ���ݷ�(+150%) ��ŭ�� ���ظ� ������ ���ݷ�(+200%) ��ŭ�� ü���� ȸ���մϴ�";
                    break;
                case 1:
                    skillInfoText.text = $"�������̰� �ϸ�ŭ ������ ��븦 ��� ({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)}%) Ȯ���� ���ݷ�(+200%) ��ŭ�� ���ظ� �����ϴ�.";
                    break;
                case 2:
                    skillInfoText.text = $"������ �������� ��븦 �һ�� ({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)})�� ° ���ݸ��� ���ݷ�(+400%) ��ŭ�� ���ظ� �����ϴ�.";
                    break;
                case 3:
                    skillInfoText.text = $"������ ���÷� ��븦 ��վ� ({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)}%) Ȯ���� ���ݷ�(+180%) ��ŭ�� ���ظ� �����ϴ�.";
                    break;
                default:
                    break;
            }
        }
        private bool CheckUnitStatus()
        {
            int atk = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]);
            int hp = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"]);
            int maxAtk = GameManager.Instance.FirebaseData.GetUnitMaxAtkByLevel(currentUnit);
            int maxHp = GameManager.Instance.FirebaseData.GetUnitMaxHpByLevel(currentUnit);
            if (atk < maxAtk || hp < maxHp)
            {
                warningMessage.SetWarningText("��� ��ȭ�� �Ϸ��ؾ� �մϴ�!");
                return false;
            }
            return true;
        }
        private bool CheckCanUpgradeAtk(bool isAtk, int value)
        {
            if (isAtk)
            {
                if (value > GameManager.Instance.FirebaseData.GetUnitMaxAtkByLevel(currentUnit))
                {
                    warningMessage.SetWarningText("�̹� �ִ�ġ �Դϴ�!");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (value > GameManager.Instance.FirebaseData.GetUnitMaxHpByLevel(currentUnit))
                {
                    warningMessage.SetWarningText("�̹� �ִ�ġ �Դϴ�!");
                    return false;
                }
                else
                {
                    return true;
                }
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
    }
}
