using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountUnit : MonoBehaviour
{
    private enum EUnitKind
    {
        Tanker,
        Mage
    }
    private int currentAttackCount;
    private Unit unit;
    [SerializeField] private EUnitKind eUnitKind;
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }

    public void UseSkill()
    {
        switch (eUnitKind)
        {
            case EUnitKind.Tanker:
                Skill_Tanker();
                break;
            case EUnitKind.Mage
            :   Skill_Mage();
                break;
            default:
                break;
        }
    }
    private void Skill_Tanker()
    {
        InGameManager.Instance.Boss.CommonStatus.DecreaseHp(unit.CommonStatus.AttackDamage * 1.5f);
        unit.CommonStatus.CurrentHp += unit.CommonStatus.AttackDamage * 2;
        if (unit.CommonStatus.CurrentHp > unit.CommonStatus.MaxHp)
        {
            unit.CommonStatus.CurrentHp = unit.CommonStatus.MaxHp;
        }
    }
    private void Skill_Mage()
    {
        InGameManager.Instance.Boss.CommonStatus.DecreaseHp(unit.CommonStatus.AttackDamage * 4);
    }
    private void Awake()
    {
        unit = GetComponent<Unit>();
        //InGameManager.Instance.Boss = FindObjectOfType<Boss>();
    }
}
