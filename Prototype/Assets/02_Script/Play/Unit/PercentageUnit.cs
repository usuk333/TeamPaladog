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
    private void Skill_Assasin() //�ϻ��� ��ų
    {
        boss.DecreaseHp(unit.AttackDamage + skillValue); //���� ���� or �ۼ�Ʈ �������� ����
    }
    private void Skill_Druid() //����̵� ��ų
    {
        // ���� �ʿ��ҵ� ���� ��ų ������ �ָ��ϴ�
    }
    private void Skill_Mage()
    {
        boss.DecreaseHp(unit.AttackDamage + skillValue); //���� ���� or �ۼ�Ʈ �������� ����
    }
    private void Skill_Elementalist()
    {
        boss.DecreaseHp(unit.AttackDamage + skillValue); //���� ���� or �ۼ�Ʈ �������� ����
    }
    private void Awake()
    {
        unit = GetComponent<Unit>();
        boss = FindObjectOfType<Boss>();
    }
}
