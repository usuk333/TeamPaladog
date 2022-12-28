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

        private string[] unitInfoArray = { "거대한 도끼가 바람을 갈랐다.\n일격.\n제 몸집만한 도끼를 어깨에 짊어진 청년은,\n양단 된 마물 앞에서 태양을 닮은 눈부신 미소를 짓고 있었다.",
        "고블린이 소름끼치는 웃음을 흘리며 다가왔다.\n하지만 소녀는 꼬리를 살랑살랑 흔들며 미소지을 뿐.\n소녀를 얕잡아본 고블린은, 나무 몽둥이를 휘둘렀다.\n자신의 머리가 바닥에서 구르고 있는 것도 알지 못한 채.",
        "찐득한 어둠 속에서 새빨간 불꽃이 일었다.\n마치 유혹하듯 꼬리를 흔드는 불꽃을 보고, 망자는 발을 내딛는다.\n불길이 몸을 집어삼키는 것은 신경도 쓰지 않고,\n한 걸음. 한 걸음.",
        "오크 로드는 나무 위에서 자고있는 여인이 마음에 들지 않았다.\n자신의 영역 안에서 느껴지는 평화로움에 구역질이 일었다.\n쾅! 오크 로드의 거목같은 다리가 나무에 직격했다\n그 뒤에 남은 것은, 잔잔한 산들바람과 새의 지저귐 뿐이었다." };

        private string[] unitNameArray = { "길버트", "하나", "하람", "로젤리아" };
        private string[] unitSkillNameArray = { "특수 공격 : 흡수의 일격", "특수 공격 : 일렁이는 아지랑이", "특수공격 : 일어나는 업화", "특수공격 : 피어스" };
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
                warningMessage.SetWarningText("골드가 부족합니다!");
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
                warningMessage.SetWarningText("계열 포인트가 부족합니다!");
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
                atkText.text = "공격력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"]
                    + " / " + GameManager.Instance.FirebaseData.GetUnitMaxAtkByLevel(currentUnit);
            }
            else
            {
                GameManager.Instance.FirebaseData.SaveData("Unit", $"{currentUnit}{statusPathArray[2]}", value);
                hpText.text = "체력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"]
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
            atkText.text = "공격력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + GameManager.Instance.FirebaseData.GetUnitMaxAtkByLevel(currentUnit);
            expText.text = GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / " + GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit);
            hpText.text = "체력 : " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + GameManager.Instance.FirebaseData.GetUnitMaxHpByLevel(currentUnit);
            levelText.text = "Lv. " + GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
            expSlider.value = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
            expSlider.maxValue = GameManager.Instance.FirebaseData.GetUnitMaxExpByLevel(currentUnit);
            unitAtkGoldText.text = $"공격력 강화\n{GameManager.Instance.FirebaseData.GetUnitUpgradeGoldByLeve(currentUnit)}골드";
            unitHpGoldText.text = $"체력 강화\n{GameManager.Instance.FirebaseData.GetUnitUpgradeGoldByLeve(currentUnit)}골드";
            ChangeSkillInfo(i);
        }
        private void ChangeSkillInfo(int i)
        {
            switch (i)
            {
                case 0:
                    skillInfoText.text = $"전설의 도끼 '파라슈'의 힘으로 상대를 내려쳐({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)})번 째 공격마다 공격력(+150%) 만큼의 피해를 입히고 공격력(+200%) 만큼의 체력을 회복합니다";
                    break;
                case 1:
                    skillInfoText.text = $"아지랑이가 일만큼 빠르게 상대를 베어내 ({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)}%) 확률로 공격력(+200%) 만큼의 피해를 입힙니다.";
                    break;
                case 2:
                    skillInfoText.text = $"강력한 마법으로 상대를 불살라 ({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)})번 째 공격마다 공격력(+400%) 만큼의 피해를 입힙니다.";
                    break;
                case 3:
                    skillInfoText.text = $"강력한 가시로 상대를 꿰뚫어 ({GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(currentUnit)}%) 확률로 공격력(+180%) 만큼의 피해를 입힙니다.";
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
                warningMessage.SetWarningText("골드 강화를 완료해야 합니다!");
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
                    warningMessage.SetWarningText("이미 최대치 입니다!");
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
                    warningMessage.SetWarningText("이미 최대치 입니다!");
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
