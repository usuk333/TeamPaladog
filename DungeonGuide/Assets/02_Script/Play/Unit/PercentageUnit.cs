using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageUnit : MonoBehaviour
{
    private enum EUnitKind
    {
        Warrior,
        archer
    }
    //private Boss boss;
    private Unit unit;
    [SerializeField] private EUnitKind eUnitKind;
    public void UseSkill()
    {
        switch (eUnitKind)
        {
            case EUnitKind.Warrior:
                Skill_Warrior();
                break;
            case EUnitKind.archer:
                Skill_Archer();
                break;
            default:
                break;
        }
    }
    private void Skill_Warrior()
    {
        InGameManager.Instance.Boss.CommonStatus.DecreaseHp(unit.CommonStatus.CurrentAttackDamage * 2);
    }
    private void Skill_Archer()
    {
        InGameManager.Instance.Boss.CommonStatus.DecreaseHp(unit.CommonStatus.CurrentAttackDamage * 1.8f);
    }
    private void Awake()
    {
        unit = GetComponent<Unit>();
    }
}
