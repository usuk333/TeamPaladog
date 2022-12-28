using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject skillInfoObj;
    [SerializeField] private Sprite[] skillIconArray;
    [SerializeField] private GameObject warningMessageObj;

    [Header("����� UI ��ҵ�")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text skillPointText;
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillLevelText;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Text skillInfoText;
    [SerializeField] private Text warningMessageText;

    private string[] skillNameArray = { "ü�� �ܷ�", "���� �ܷ�", "�ƹ��ų� �¾ƶ�!", "��ȣ�� ����", "ġ���� ����", "�������" };
    private string[] skillPathArray = { "HP", "MP", "Attack", "Barrior", "Heal", "PowerUp"};

    private int currentSkill = -1;

    private void Start()
    {
        goldText.text = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"]) < 10000 ? GameManager.Instance.FirebaseData.InfoDictionary["Gold"].ToString() + " ���" : DataEquation.GetUnit(Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"])) + " ���";
        skillPointText.text = GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"].ToString();
    }
    public void BtnEvt_UpgradeSkill()
    {
        if (!CheckHaveSkillPoint()) return;
        UpgradeSkill();       
    }
    public void BtnEvt_NextSkillInfo()
    {
        currentSkill++;
        if (currentSkill > 5) currentSkill = 0;
        ChangeSkillUI(currentSkill);
    }
    public void BtnEvt_PreviousSkillInfo()
    {
        currentSkill--;
        if (currentSkill < 0) currentSkill = 5;
        ChangeSkillUI(currentSkill);
    }
    private bool CheckHaveSkillPoint()
    {
        int point = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"]);
        if (point <= 0)
        {
            warningMessageText.text = "��ų ���ļ��� �����մϴ�!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        else
        {
            return true;
        }
    }
    private void UpgradeSkill()
    {
        if (CheckSkillLevel())
        {
            int point = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"]);
            point--;
            GameManager.Instance.FirebaseData.SaveData("Skill", "SkillPoints", point);
            skillPointText.text = GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"].ToString();
            int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary[skillPathArray[currentSkill]]) + 1;
            GameManager.Instance.FirebaseData.SaveData("Skill", skillPathArray[currentSkill], level);
            ChangeSkillUI(currentSkill);
        }
    }
    private bool CheckSkillLevel()
    {
        if (currentSkill < 2) return true;
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary[skillPathArray[currentSkill]]);
        if (level >= 5)
        {
            warningMessageText.text = "�̹� �ִ�ġ �Դϴ�!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        return true;
    }
    public void BtnEvt_LoadMainScene()
    {
        GameManager.Instance.LoadScene(0);
    }
    public void BtnEvt_ActiveSkillInfoObj(int i)
    {
        if (!skillInfoObj.activeSelf)
        {
            skillInfoObj.SetActive(!skillInfoObj.activeSelf);
        }
        if(currentSkill == i) return;
        currentSkill = i;
        ChangeSkillUI(i);
    }
    public void BtnEvt_ActiveObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    private void ChangeSkillUI(int i)
    {
        skillNameText.text = skillNameArray[i];
        skillLevelText.text = $"Lv. {GameManager.Instance.FirebaseData.SkillDictionary[skillPathArray[i]]}";
        skillIconImage.sprite = skillIconArray[i];
        ChangeSkillInfo(i);
    }
    private void ChangeSkillInfo(int i)
    {
        switch (i)
        {
            case 0:
                var (hMax, hRegen) = DataEquation.PlayerSkillHPToLevel();
                skillInfoText.text = $"������ ����� ü���� �ܷ��մϴ�.\nü�� �ִ뷮 = ({hMax})\nü�� ȸ���� = ({hRegen})";
                break;
            case 1:
                var (mMax, mRegen) = DataEquation.PlayerSkillMPToLevel();
                skillInfoText.text = $"������ �׾� ������ ȿ�������� ����� �� �ְ� �˴ϴ�.\n���� �ִ뷮 = ({mMax})\n���� ȸ���� = ({mRegen})";
                break;
            case 2:
                var (apple, rock, bomb) = DataEquation.PlayerSkillAttackToLevel();
                skillInfoText.text = $"�տ� ������ ������ ������ �����ϴ�.\n���, ������, ��ź�� ���� ({apple}), ({rock}), ({bomb}) ��ŭ�� �������� �����ϴ�.\n���� �Ҹ� = (5)\n��Ÿ�� = (3) ��";
                break;
            case 3:
                var (bValue, bDuration, bMana, bCool) = DataEquation.PlayerSkillBarriorToLevel();
                skillInfoText.text = $"��ȣ���� �����ϴ� ������ ����� �Ʊ��� ��ȣ�մϴ�.\n({bDuration}) �� �� �����Ǵ� �Ʊ� �ִ� ü�� ({bValue}%) ��ŭ�� ��ȣ���� �����Ѵ�.\n���� �Ҹ� = ({bMana})\n��Ÿ�� = ({bCool}) ��";
                break;
            case 4:
                var (hValue, hMana, hCool) = DataEquation.PlayerSkillHealToLevel();
                skillInfoText.text = $"ȸ�� ������ ����� �Ʊ��� ġ���մϴ�.\n�Ʊ� �ִ� ü���� ({hValue}%) ��ŭ ȸ�� ��Ų��.\n���� �Ҹ� = ({hMana})\n��Ÿ�� = ({hCool}) ��";
                break;
            case 5:
                var (rValue, rMana) = DataEquation.PlayerSkillRageToLevel();
                skillInfoText.text = $"��⸦ �ϵ��� �Ʊ��� ���ϰ� ����ϴ�.\n�Ʊ� ���ݷ��� ({rValue}%) ��ŭ ������ŵ�ϴ�.\n���� �Ҹ� = �ʴ� ({rMana})\n��Ÿ�� = (1)��";
                break;
            default:
                break;
        }
    }
    private IEnumerator Co_WarningMessageAnim()
    {
        warningMessageObj.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        warningMessageObj.SetActive(false);
    }
}
