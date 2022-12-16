using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UnitStatus
{
    private int level;
    public float currentDamage;
    public float maxDamage;
    public float currentHp;
    public float maxHp;
    public float attackSpeed;
    public float skillCondition; // 현재 스킬 발동 조건


    public float damageUpValue;
    public float damageMaxUpValue;
    public float hpUpValue;
    public float hpMaxUpValue;
    public float skillUpValue; //스킬 발동 조건 증감값

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            if (level == 1)
            {
                return;
            }
            maxDamage += damageMaxUpValue;
            maxHp += hpMaxUpValue;
            if (level % 10 == 0)
            {
                skillCondition += skillUpValue;
            }
        }
    }
}

[System.Serializable]
public struct Passive
{
    private float level;
    public float max;
    public float maxUpValue;
    public float regen;
    public float regenUpValue;

    public float Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            if (level == 1)
            {
                return;
            }
            max += maxUpValue;
            if(level % 5 == 0)
            {
                regen += regenUpValue;
            }
        }
    }
}

[System.Serializable]
public struct Barrior
{
    public float maxLevel;
    private float level;
    public float size;
    public float sizeUpValue;
    public float time;
    public float timeUpValue;
    public float cost;
    public float costUpValue;
    public float coolTime;
    public float coolTimeUpValue;//쿨감

    public float Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            if(level == 1)
            {
                return;
            }
            if (level > maxLevel)
            {
                level = maxLevel;
                Debug.Log("최대치입니다");
                return;
            }
            size += sizeUpValue;
            time += timeUpValue;
            cost += costUpValue;
            coolTime += coolTimeUpValue;
        }
    }
}

[System.Serializable]
public struct Heal
{
    public float maxLevel;
    private float level;
    public float size;
    public float sizeUpValue;
    public float cost;
    public float costUpValue;
    public float coolTime;
    public float coolTimeUpValue;//쿨감
    public float Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            if (level == 1)
            {
                return;
            }
            if (level > maxLevel)
            {
                level = maxLevel;
                Debug.Log("최대치입니다");
                return;
            }
            size += sizeUpValue;
            cost += costUpValue;
            coolTime += coolTimeUpValue;
        }
    }
}

[System.Serializable]
public struct PowerUp
{
    public float maxLevel;
    private float level;
    public float size;
    public float sizeUpValue;
    public float cost;
    public float costUpValue;
    public float coolTime;//1초
    public float Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            if (level == 1)
            {
                return;
            }
            if (level > maxLevel)
            {
                level = maxLevel;
                Debug.Log("최대치입니다");
                return;
            }
            size += sizeUpValue;
            cost += costUpValue;
        }
    }
}

[System.Serializable]
public struct Attack
{
    public float maxLevel;
    private float level;
    public float[] attackPercentage;
    public float[] attackDamage;
    public float sizeUpValue;
    public float cost;
    public float coolTime;
    public float Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            if (level == 1)
            {
                return;
            }
            if (level > maxLevel)
            {
                level = maxLevel;
                Debug.Log("최대치입니다");
                return;
            }
            for (int i = 0; i < attackDamage.Length; i++)
            {
                attackDamage[i] += sizeUpValue;
            }
        }
    }
}

public class StageTest : MonoBehaviour
{
    public GameObject stageSection;
    public GameObject unitSection;
    public GameObject skillSection;
    public UnitStatus tanker;
    public Text[] tankerInfo;
    public UnitStatus warrior;
    public Text[] warriorInfo;
    public UnitStatus mage;
    public Text[] mageInfo;
    public UnitStatus archer;
    public Text[] archerInfo;

    public Passive HP;
    public Text[] HPInfo;
    public Passive MP;
    public Text[] MPInfo;
    public Barrior Barrior;
    public Text[] BarriorInfo;
    public Heal Heal;
    public Text[] HealInfo;
    public PowerUp PowerUp;
    public Text[] PowerUpInfo;
    public Attack Attack;
    public Text[] AttackInfo;

    public AudioClip[] stageBGMClipArray;
    public GameObject[] bossObjArray;




    public void BtnEvt_LoadStage(int index)
    {
       // BossManager.BossSection = bossObjArray[index];
      //  BossManager.bgm = stageBGMClipArray[index];
        stageSection.SetActive(true);
        //LoadingSceneController.LoadScene("Stage");
    }
    public void BtnEvt_ActiveSection()
    {
        stageSection.SetActive(false);
    }
    public void BtnEvt_ActiveUnitSection()
    {
        unitSection.SetActive(!unitSection.activeSelf);
    }
    public void BtnEvt_ActiveSkillSection()
    {
        skillSection.SetActive(!skillSection.activeSelf);
    }
    //public void BtnEvt_ChooseDifficulty(int index)
    //{
    //    BossManager.difficulty = (BossManager.Difficulty)index;
    //    UpdateUnitData();
    //    UpdateSkillData();
    //    LoadingSceneController.LoadScene("Stage");
    //}
    //private void UpdateUnitData()
    //{
    //    BossManager.tanker = tanker;
    //    BossManager.warrior = warrior;
    //    BossManager.mage = mage;
    //    BossManager.archer = archer;
    //}
    //private void UpdateSkillData()
    //{
    //    BossManager.hp = HP;
    //    BossManager.mp = MP;
    //    BossManager.barrior = Barrior;
    //    BossManager.heal = Heal;
    //    BossManager.powerUp = PowerUp;
    //    BossManager.attack = Attack;
    //}
    public void BtnEvt_ReinforceDamage(int index)
    {
        ReinforceUnitStatus(index, false);
    }
    public void BtnEvt_ReinforceHp(int index)
    {
        ReinforceUnitStatus(index, true);
    }
    public void BtnEvt_ReinforceLevel(int index)
    {
        ReinforceLevel(index);
    }
    public void BtnEvt_ReinforceSkillLevel(int index)
    {
        SkillLevelUp(index);
    }
    private void ReinforceUnitStatus(int index, bool isHp)
    {
        if (isHp)
        {
            switch (index)
            {
                case 0:
                    if (tanker.currentHp >= tanker.maxHp)
                    {
                        print("최대치입니다");
                        return;
                    }
                    tanker.currentHp += tanker.hpUpValue;
                    break;
                case 1:
                    if (warrior.currentHp >= warrior.maxHp)
                    {
                        print("최대치입니다");
                        return;
                    }
                    warrior.currentHp += warrior.hpUpValue;
                    break;
                case 2:
                    if (mage.currentHp >= mage.maxHp)
                    {
                        print("최대치입니다");
                        return;
                    }
                    mage.currentHp += mage.hpUpValue;
                    break;
                case 3:
                    if (archer.currentHp >= archer.maxHp)
                    {
                        print("최대치입니다");
                        return;
                    }
                    archer.currentHp += archer.hpUpValue;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    if (tanker.currentDamage >= tanker.maxDamage)
                    {
                        print("최대치입니다");
                        return;
                    }
                    tanker.currentDamage += tanker.damageUpValue;
                    break;
                case 1:
                    if (warrior.currentDamage >= warrior.maxDamage)
                    {
                        print("최대치입니다");
                        return;
                    }
                    warrior.currentDamage += warrior.damageUpValue;
                    break;
                case 2:
                    if (mage.currentDamage >= mage.maxDamage)
                    {
                        print("최대치입니다");
                        return;
                    }
                    mage.currentDamage += mage.damageUpValue;
                    break;
                case 3:
                    if (archer.currentDamage >= archer.maxDamage)
                    {
                        print("최대치입니다");
                        return;
                    }
                    archer.currentDamage += archer.damageUpValue;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
        UpdateText(index);
    }
    private void ReinforceLevel(int index)
    {
        switch (index)
        {
            case 0:
                tanker.Level += 1;
                break;
            case 1:
                warrior.Level += 1;
                break;
            case 2:
                mage.Level += 1;
                break;
            case 3:
                archer.Level += 1;
                break;
            default:
                Debug.Assert(false);
                break;
        }
        UpdateText(index);
    }
    private void UpdateText(int index)
    {
        switch (index)
        {
            case 0:
                tankerInfo[0].text = "레벨:" + tanker.Level;
                tankerInfo[1].text = "공격력:" + tanker.currentDamage + "/" + tanker.maxDamage;
                tankerInfo[2].text = "체력:" + tanker.currentHp + "/" + tanker.maxHp;
                break;
            case 1:
                warriorInfo[0].text = "레벨:" + warrior.Level;
                warriorInfo[1].text = "공격력:" + warrior.currentDamage + "/" + warrior.maxDamage;
                warriorInfo[2].text = "체력:" + warrior.currentHp + "/" + warrior.maxHp;
                break;
            case 2:
                mageInfo[0].text = "레벨:" + mage.Level;
                mageInfo[1].text = "공격력:" + mage.currentDamage + "/" + mage.maxDamage;
                mageInfo[2].text = "체력:" + mage.currentHp + "/" + mage.maxHp;
                break;
            case 3:
                archerInfo[0].text = "레벨:" + archer.Level;
                archerInfo[1].text = "공격력:" + archer.currentDamage + "/" + archer.maxDamage;
                archerInfo[2].text = "체력:" + archer.currentHp + "/" + archer.maxHp;
                break;
            default:
                break;
        }
    }
    public void BtnEvt_Exit()
    {
        Application.Quit();
    }
    private void Awake()
    {
        tanker.Level = 1;
        warrior.Level = 1;
        mage.Level = 1;
        archer.Level = 1;
        HP.Level = 1;
        MP.Level = 1;
        Barrior.Level = 1;
        Heal.Level = 1;
        PowerUp.Level = 1;
        Attack.Level = 1;
        for (int i = 0; i < 4; i++)
        {
            UpdateText(i);
        }
        for (int i = 0; i < 6; i++)
        {
            SkillTextUpdate(i);
        }
    }

    private void SkillLevelUp(int index)
    {
        switch(index)
        {
            case 0:
                HP.Level += 1;
                break;
            case 1:
                MP.Level += 1;
                break;
            case 2:
                Barrior.Level += 1;
                break;
            case 3:
                Heal.Level += 1;
                break;
            case 4:
                PowerUp.Level += 1;
                break;
            case 5:
                Attack.Level += 1;
                break;
            default:
                break;
        }
        SkillTextUpdate(index);
    }

    private void SkillTextUpdate(int index)
    {
        switch (index)
        {
            case 0:
                HPInfo[0].text = "최대치 : " + HP.max;
                HPInfo[1].text = "회복량 : " + HP.regen;
                HPInfo[2].text = "레벨 : " + HP.Level;
                break;
            case 1:
                MPInfo[0].text = "최대치 : " + MP.max;
                MPInfo[1].text = "회복량 : " + MP.regen;
                MPInfo[2].text = "레벨 : " + MP.Level;
                break;
            case 2:
                BarriorInfo[0].text = "계수 : " + Barrior.size;
                BarriorInfo[1].text = "유지시간 : " + Barrior.time;
                BarriorInfo[2].text = "마나 : " + Barrior.cost;
                BarriorInfo[3].text = "쿨타임 : " + Barrior.coolTime;
                BarriorInfo[4].text = "레벨 : " + Barrior.Level;
                break;
            case 3:
                HealInfo[0].text = "계수 : " + Heal.size;
                HealInfo[1].text = "마나 : " + Heal.cost;
                HealInfo[2].text = "쿨타임 : " + Heal.coolTime;
                HealInfo[3].text = "레벨 : " + Heal.Level;
                break;
            case 4:
                PowerUpInfo[0].text = "계수 : " + PowerUp.size;
                PowerUpInfo[1].text = "마나 : " + PowerUp.cost.ToString("F1");
                PowerUpInfo[2].text = "레벨 : " + PowerUp.Level;
                break;
            case 5:
                AttackInfo[0].text = "계수 : " + Attack.attackDamage[0] + "/" + Attack.attackDamage[1] + "/" + Attack.attackDamage[2];
                AttackInfo[1].text = "마나 : " + Attack.cost;
                AttackInfo[2].text = "쿨타임 : " + Attack.coolTime;
                AttackInfo[3].text = "레벨 : " + Attack.Level;
                break;
            default:
                break;
        }
    }
}
