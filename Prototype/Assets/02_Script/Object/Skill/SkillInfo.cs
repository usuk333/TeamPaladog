using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using ETC;

namespace SkillScene
{
    public class SkillInfo : MonoBehaviour
    {
        [SerializeField] private Sprite[] skillIconArray;
        [SerializeField] private WarningMessage warningMessage;
        [SerializeField] private StatusBar skillBar;

        [Header("변경될 UI 요소들")]
        [SerializeField] private Text skillNameText;
        [SerializeField] private Text skillLevelText;
        [SerializeField] private Image skillIconImage;
        [SerializeField] private Text skillInfoText;

        private string[] skillNameArray = { "체력 단련", "마나 단련", "아무거나 맞아라!", "보호의 물약", "치유의 물약", "사기진작" };
        private string[] skillPathArray = { "HP", "MP", "Attack", "Barrior", "Heal", "PowerUp" };

        private int currentSkill = -1;

        public int CurrentSkill { get => currentSkill; set => currentSkill = value; }

        public void ChangeSkillUI(int i)
        {
            skillNameText.text = skillNameArray[i];
            skillLevelText.text = $"Lv. {GameManager.Instance.FirebaseData.SkillDictionary[skillPathArray[i]]}";
            skillIconImage.sprite = skillIconArray[i];
            ChangeSkillInfo(i);
        }
        private void ChangeSkillInfo(int i)
        {
            float[] skillValue = GameManager.Instance.FirebaseData.SkillArray[i].GetSkillValueArray();
            switch (i)
            {
                case 0:
                    skillInfoText.text = $"꾸준한 운동으로 체력을 단련합니다.\n체력 최대량 = ({skillValue[0]})\n체력 회복량 = ({skillValue[1]})";
                    break;
                case 1:
                    skillInfoText.text = $"지식을 쌓아 마나를 효율적으로 사용할 수 있게 됩니다.\n마나 최대량 = ({skillValue[0]})\n마나 회복량 = ({skillValue[1]})";
                    break;
                case 2:
                    skillInfoText.text = $"손에 잡히는 물건을 적에게 던집니다.\n사과, 돌맹이, 폭탄은 각각 ({skillValue[0]}), ({skillValue[1]}), ({skillValue[2]}) 만큼의 데미지를 가집니다.\n마나 소모량 = (5)\n쿨타임 = (3) 초";
                    break;
                case 3:
                    skillInfoText.text = $"보호막을 생성하는 물약을 사용해 아군을 보호합니다.\n({skillValue[1]}) 초 간 유지되는 아군 최대 체력 ({skillValue[0]}%) 만큼의 보호막을 제공합니다.\n마나 소모량 = ({skillValue[2]})\n쿨타임 = ({skillValue[3]}) 초";
                    break;
                case 4:
                    skillInfoText.text = $"회복 물약을 사용해 아군을 치유합니다.\n아군 최대 체력의 ({skillValue[0]}%) 만큼 회복시킵니다.\n마나 소모량 = ({skillValue[1]})\n쿨타임 = ({skillValue[2]}) 초";
                    break;
                case 5:
                    skillInfoText.text = $"사기를 북돋아 아군을 강하게 만듭니다.\n아군 공격력을 ({skillValue[0]}%) 만큼 증가시킵니다.\n마나 소모량 = 초당 ({skillValue[1]})\n쿨타임 = (1)초";
                    break;
                default:
                    break;
            }
        }
        public bool CheckHaveSkillPoint()
        {
            int point = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"]);
            if (point <= 0)
            {
                warningMessage.SetWarningText("스킬 돌파석이 부족합니다!");
                return false;
            }
            else
            {
                return true;
            }
        }
        public void UpgradeSkill()
        {
            int level = GameManager.Instance.FirebaseData.SkillArray[currentSkill].GetLevel();
            if (CheckSkillLevel(level))
            {
                int point = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"]);
                point--;
                GameManager.Instance.FirebaseData.SaveData("Skill", "SkillPoints", point);
                skillBar.UpdateValueText();
                level += 1;
                GameManager.Instance.FirebaseData.SaveData("Skill", skillPathArray[currentSkill], level);
                GameManager.Instance.FirebaseData.SkillArray[currentSkill].LevelUp();            
                ChangeSkillUI(currentSkill);
            }
        }
        private bool CheckSkillLevel(int level)
        {
            if (currentSkill < 2) return true;
            if (level >= 5)
            {
                warningMessage.SetWarningText("이미 최대치 입니다!");
                return false;
            }
            return true;
        }
    }
}
