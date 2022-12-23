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

    [Header("변경될 UI 요소들")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text skillPointText;
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillLevelText;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Text skillInfoText;
    [SerializeField] private Text warningMessageText;

    private string[] skillNameArray = { "체력 단련", "마나 단련", "아무거나 맞아라!", "보호의 물약", "치유의 물약", "사기진작" };
    private string[] skillPathArray = { "HP", "MP", "Attack", "Barrior", "Heal", "PowerUp"};

    private int currentSkill = -1;

    private void Start()
    {
        goldText.text = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"]) < 10000 ? GameManager.Instance.FirebaseData.InfoDictionary["Gold"].ToString() + " 골드" : DataEquation.GetUnit(Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"])) + " 골드";
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
            warningMessageText.text = "스킬 돌파석이 부족합니다!";
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
            warningMessageText.text = "이미 최대치 입니다!";
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
                skillInfoText.text = $"꾸준한 운동으로 체력을 단련합니다.\n체력 최대량 = ({hMax})\n체력 회복량 = ({hRegen})";
                break;
            case 1:
                var (mMax, mRegen) = DataEquation.PlayerSkillMPToLevel();
                skillInfoText.text = $"지식을 쌓아 마나를 효율적으로 사용할 수 있게 됩니다.\n마나 최대량 = ({mMax})\n마나 회복량 = ({mRegen})";
                break;
            case 2:
                var (apple, rock, bomb) = DataEquation.PlayerSkillAttackToLevel();
                skillInfoText.text = $"손에 잡히는 물건을 적에게 던집니다.\n사과, 돌맹이, 폭탄은 각각 ({apple}), ({rock}), ({bomb}) 만큼의 데미지를 가집니다.\n마나 소모량 = (5)\n쿨타임 = (3) 초";
                break;
            case 3:
                var (bValue, bDuration, bMana, bCool) = DataEquation.PlayerSkillBarriorToLevel();
                skillInfoText.text = $"보호막을 생성하는 물약을 사용해 아군을 보호합니다.\n({bDuration}) 초 간 유지되는 아군 최대 체력 ({bValue}%) 만큼의 보호막을 제공한다.\n마나 소모량 = ({bMana})\n쿨타임 = ({bCool}) 초";
                break;
            case 4:
                var (hValue, hMana, hCool) = DataEquation.PlayerSkillHealToLevel();
                skillInfoText.text = $"회복 물약을 사용해 아군을 치유합니다.\n아군 최대 체력의 ({hValue}%) 만큼 회복 시킨다.\n마나 소모량 = ({hMana})\n쿨타임 = ({hCool}) 초";
                break;
            case 5:
                var (rValue, rMana) = DataEquation.PlayerSkillRageToLevel();
                skillInfoText.text = $"사기를 북돋아 아군을 강하게 만듭니다.\n아군 공격력을 ({rValue}%) 만큼 증가시킵니다.\n마나 소모량 = 초당 ({rMana})\n쿨타임 = (1)초";
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
