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

        [Header("����� UI ��ҵ�")]
        [SerializeField] private Text skillNameText;
        [SerializeField] private Text skillLevelText;
        [SerializeField] private Image skillIconImage;
        [SerializeField] private Text skillInfoText;

        private string[] skillNameArray = { "ü�� �ܷ�", "���� �ܷ�", "�ƹ��ų� �¾ƶ�!", "��ȣ�� ����", "ġ���� ����", "�������" };
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
                    skillInfoText.text = $"������ ����� ü���� �ܷ��մϴ�.\nü�� �ִ뷮 = ({skillValue[0]})\nü�� ȸ���� = ({skillValue[1]})";
                    break;
                case 1:
                    skillInfoText.text = $"������ �׾� ������ ȿ�������� ����� �� �ְ� �˴ϴ�.\n���� �ִ뷮 = ({skillValue[0]})\n���� ȸ���� = ({skillValue[1]})";
                    break;
                case 2:
                    skillInfoText.text = $"�տ� ������ ������ ������ �����ϴ�.\n���, ������, ��ź�� ���� ({skillValue[0]}), ({skillValue[1]}), ({skillValue[2]}) ��ŭ�� �������� �����ϴ�.\n���� �Ҹ� = (5)\n��Ÿ�� = (3) ��";
                    break;
                case 3:
                    skillInfoText.text = $"��ȣ���� �����ϴ� ������ ����� �Ʊ��� ��ȣ�մϴ�.\n({skillValue[1]}) �� �� �����Ǵ� �Ʊ� �ִ� ü�� ({skillValue[0]}%) ��ŭ�� ��ȣ���� �����մϴ�.\n���� �Ҹ� = ({skillValue[2]})\n��Ÿ�� = ({skillValue[3]}) ��";
                    break;
                case 4:
                    skillInfoText.text = $"ȸ�� ������ ����� �Ʊ��� ġ���մϴ�.\n�Ʊ� �ִ� ü���� ({skillValue[0]}%) ��ŭ ȸ����ŵ�ϴ�.\n���� �Ҹ� = ({skillValue[1]})\n��Ÿ�� = ({skillValue[2]}) ��";
                    break;
                case 5:
                    skillInfoText.text = $"��⸦ �ϵ��� �Ʊ��� ���ϰ� ����ϴ�.\n�Ʊ� ���ݷ��� ({skillValue[0]}%) ��ŭ ������ŵ�ϴ�.\n���� �Ҹ� = �ʴ� ({skillValue[1]})\n��Ÿ�� = (1)��";
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
                warningMessage.SetWarningText("��ų ���ļ��� �����մϴ�!");
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
                warningMessage.SetWarningText("�̹� �ִ�ġ �Դϴ�!");
                return false;
            }
            return true;
        }
    }
}
