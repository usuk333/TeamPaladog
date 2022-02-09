using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountUnit : MonoBehaviour
{
    private enum EUnitKind
    {
        Warrior,
        Shielder,
        Taoist,
        Mechanic
    }
    private int currentAttackCount;
    private Boss boss;
    [SerializeField] private float skillDamage;
    [SerializeField] private int attackCount;
    [SerializeField] private EUnitKind eUnitKind;
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }
    public int AttackCount { get => attackCount; set => attackCount = value; }

    public void AttackSpecial()
    {
        switch (eUnitKind)
        {
            case EUnitKind.Warrior:
                AttackWarrior();
                break;
            case EUnitKind.Shielder:
                break;
            case EUnitKind.Taoist:
                break;
            case EUnitKind.Mechanic:
                break;
            default:
                break;
        }
    }
    private void AttackWarrior()
    {
        boss.DecreaseHp(skillDamage);
    }
    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
    }
}
