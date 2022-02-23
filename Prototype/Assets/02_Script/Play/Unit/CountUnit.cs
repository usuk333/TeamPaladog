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
    private Unit unit;
    [SerializeField] private float skillValue;
    [SerializeField] private int attackCount;
    [SerializeField] private EUnitKind eUnitKind;
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }
    public int AttackCount { get => attackCount; set => attackCount = value; }

    public void UseSkill()
    {
        switch (eUnitKind)
        {
            case EUnitKind.Warrior:
                Skill_Warrior();
                break;
            case EUnitKind.Shielder:
                Skill_Shielder();
                break;
            case EUnitKind.Taoist:
                Skill_Taoist();
                break;
            case EUnitKind.Mechanic:
                Skill_Mechanic();
                break;
            default:
                break;
        }
    }
    private void Skill_Warrior() //���� ��ų
    {
        boss.CommonStatus.DecreaseHp(skillValue);
    }
    private void Skill_Shielder() // ���к� ��ų
    {
        unit.CommonStatus.CurrentHp += skillValue;
        if(unit.CommonStatus.CurrentHp > unit.CommonStatus.MaxHp)
        {
            unit.CommonStatus.CurrentHp = unit.CommonStatus.MaxHp;
        }
    }
    private void Skill_Taoist()
    {
        //������ Ʈ�� w ī�� �̵��� �Ӹ� ���� ��ų ���� �߰� �Ѵ��� ���� �ؿ��� �ұ�� �Ͼ�� �ϴ°� ���
        boss.CommonStatus.DecreaseHp(skillValue);
    }
    private void Skill_Mechanic()
    {
        //���� �Ӹ������� ��ź ����? �ƴϸ� �缱���� ����?
        boss.CommonStatus.DecreaseHp(skillValue);
    }
    private void Awake()
    {
        unit = GetComponent<Unit>();
        boss = FindObjectOfType<Boss>();
    }
}
