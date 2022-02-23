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
    private void Skill_Warrior() //전사 스킬
    {
        boss.CommonStatus.DecreaseHp(skillValue);
    }
    private void Skill_Shielder() // 방패병 스킬
    {
        unit.CommonStatus.CurrentHp += skillValue;
        if(unit.CommonStatus.CurrentHp > unit.CommonStatus.MaxHp)
        {
            unit.CommonStatus.CurrentHp = unit.CommonStatus.MaxHp;
        }
    }
    private void Skill_Taoist()
    {
        //연출을 트페 w 카드 뽑듯이 머리 위에 스킬 부적 뜨게 한다음 보스 밑에서 불기둥 일어나게 하는건 어떤지
        boss.CommonStatus.DecreaseHp(skillValue);
    }
    private void Skill_Mechanic()
    {
        //보스 머리위에서 폭탄 투하? 아니면 사선으로 투하?
        boss.CommonStatus.DecreaseHp(skillValue);
    }
    private void Awake()
    {
        unit = GetComponent<Unit>();
        boss = FindObjectOfType<Boss>();
    }
}
