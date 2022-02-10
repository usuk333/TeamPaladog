using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageUnit : MonoBehaviour
{
    private enum EUnitKind
    {
        Assasin,
        Druid,
        mage,
        Elementalist
    }
    private Boss boss;
    private Unit unit;
    [SerializeField] private float skillValue;
    [SerializeField] private float skillPercentage;
    [SerializeField] private EUnitKind eUnitKind;

    public float SkillPercentage { get => skillPercentage; }

    public void UseSkill()
    {
        switch (eUnitKind)
        {
            case EUnitKind.Assasin:
                Skill_Assasin();
                break;
            case EUnitKind.Druid:
                Skill_Druid();
                break;
            case EUnitKind.mage:
                Skill_Mage();
                break;
            case EUnitKind.Elementalist:
                Skill_Elementalist();
                break;
            default:
                break;
        }
    }
    private void Skill_Assasin() //암살자 스킬
    {
        boss.DecreaseHp(unit.AttackDamage + skillValue); //추후 곱절 or 퍼센트 연산으로 적용
    }
    private void Skill_Druid() //드루이드 스킬
    {
        // 수정 필요할듯 아직 스킬 설명이 애매하다
    }
    private void Skill_Mage()
    {
        boss.DecreaseHp(unit.AttackDamage + skillValue); //추후 곱절 or 퍼센트 연산으로 적용
    }
    private void Skill_Elementalist()
    {
        boss.DecreaseHp(unit.AttackDamage + skillValue); //추후 곱절 or 퍼센트 연산으로 적용
    }
    private void Awake()
    {
        unit = GetComponent<Unit>();
        boss = FindObjectOfType<Boss>();
    }
}
