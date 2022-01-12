using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EUnitState
{
    Battle,
    NonCombat,
    Back,
    Wait,
    KnockBack,
    Die
}
public class Unit : MonoBehaviour //모든 아군 유닛들의 능력치와 공격 로직을 호출하는 스크립트
{
    public enum EUnitKinds
    {
        Warrior,
        Archer,
        Wizard,
        Shielder,
        Priest,
        Assasin,
        Summoner,
        Druid,
        Special
    }
    [SerializeField] private EUnitState unitState = EUnitState.NonCombat;
    [SerializeField] private EUnitKinds unitKinds = EUnitKinds.Warrior;
    [SerializeField] private int index;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float cost;
    [SerializeField] private Enemy currentEnemy;
    [SerializeField] private Boss boss;

    public EUnitState UnitState { get => unitState; }
    public Enemy CurrentEnemy { get => currentEnemy; set => currentEnemy = value; }
    public float CurrentHp { get => currentHp; }
    public float MaxHp { get => maxHp; }
    public float Cost { get => cost; }
    public EUnitKinds UnitKinds { get => unitKinds; }
    public Boss Boss { get => boss; set => boss = value; }
    public float AttackPower { get => attackPower; set => attackPower = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Index { get => index; }
    public void DecreaseHp(float damage)
    {
        currentHp -= damage;
        GetComponentInChildren<HpBar>().UpdateUnitOrEnemyHpBar();
    }
    public void DecreaseHpDot(int dotCount, float damage, float second)
    {
        StartCoroutine(Co_DotDamage(dotCount, damage, second));
    }
    public void IncreaseHp(float value)
    {
        currentHp += value;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        GetComponentInChildren<HpBar>().UpdateUnitOrEnemyHpBar();
    }
    public void Stun()
    {
        unitState = EUnitState.Wait;
        Invoke("Invoke_WakeUp", 2f);
    }
    private void initializeUnit()
    {
        currentHp = maxHp;
        GetComponentInChildren<HpBar>().ResetHpBar();
        switch (unitKinds)
        {
            case EUnitKinds.Warrior:
                break;
            case EUnitKinds.Archer:
                break;
            case EUnitKinds.Wizard:
                break;
            case EUnitKinds.Shielder:
                GetComponent<Shielder>().AttackCount = 0;
                break;
            case EUnitKinds.Priest:
                GetComponent<Priest>().ClearList();
                break;
            case EUnitKinds.Assasin:
                break;
            case EUnitKinds.Summoner:
                GetComponent<Summoner>().ResetSummoner();
                break;
            case EUnitKinds.Druid:
                GetComponent<Druid>().ResetTransmog();
                break;
            case EUnitKinds.Special:
                break;
            default:
                break;
        }
    }
    private void Attack(bool isBoss)
    {
        switch (unitKinds)
        {
            case EUnitKinds.Warrior:
                AttackBasic(isBoss);
                break;
            case EUnitKinds.Archer:
                AttackArcher();
                break;
            case EUnitKinds.Wizard:
                AttackWizard();
                break;
            case EUnitKinds.Shielder:
                AttackShielder(isBoss);
                break;
            case EUnitKinds.Priest:
                Heal();
                break;
            case EUnitKinds.Assasin:
                AttackBasic(isBoss);
                break;
            case EUnitKinds.Summoner:
                Summon();
                break;
            case EUnitKinds.Druid:
                AttackDruid(isBoss);
                break;
            case EUnitKinds.Special:
                break;
            default:
                break;
        }
    }
    private void AttackBasic(bool isBoss)
    {
        if(!isBoss)
        {            
            currentEnemy.DecreaseHp(attackPower);     
        }
        else
        {
            boss.DecreaseHp(attackPower);
        }
    }
    private void AttackArcher()
    {
        GetComponent<Archer>().AttackArcher();
    }
    private void AttackShielder(bool isBoss)
    {
        AttackBasic(isBoss);
        var shielder = GetComponent<Shielder>();
        shielder.AttackCount++;
        if (shielder.AttackCount >= 3)
        {
            shielder.AttackCount = 0;
            if (!isBoss)
            {
                currentEnemy.Stun();
            }
        }
    }
    private void AttackWizard()
    {
        GetComponent<Wizard>().AttackWizard();
    }
    private void Heal()
    {
        GetComponent<Priest>().Heal();
    }
    private void Invoke_MoveAssasinBack()
    {
        unitState = EUnitState.Back;
        GetComponent<Assasin>().UnStealth();
    }
    private void Invoke_MoveAssasinForward()
    {
        unitState = EUnitState.NonCombat;
        GetComponent<Assasin>().Hide();
    }
    private void Summon()
    {
        GetComponent<Summoner>().Summon();    
    }
    private void AttackDruid(bool isBoss)
    {
        var druid = GetComponent<Druid>();
        if (druid.DruidState == Druid.EDruidState.Wolf)
        {
            druid.AttackWolf();
        }
        else
        {
            AttackBasic(isBoss);
        }
    }
    private void Invoke_WakeUp()
    {
        unitState = EUnitState.Battle;
    }
    private IEnumerator Co_StateBattle()
    {
        while (true)
        {
            if (unitState != EUnitState.Battle && unitState != EUnitState.Wait)
            {
                if (currentEnemy != null || boss != null)
                {
                    unitState = EUnitState.Battle;
                }
            }
            if (unitState == EUnitState.Battle)
            {
                if (currentEnemy == null && boss == null)
                {
                    if (unitKinds == EUnitKinds.Assasin)
                    {
                        unitState = EUnitState.Back;
                    }
                    else
                    {
                        unitState = EUnitState.NonCombat;
                    }
                }
            }
            yield return null;
        }
    }
    private IEnumerator Co_Battle()
    {
        while (true)
        {
            if (unitState == EUnitState.Battle)
            {
                if (currentEnemy && !boss)
                {
                    Attack(false);
                }
                else if (boss && !currentEnemy)
                {
                    Attack(true);
                }
                yield return new WaitForSeconds(attackSpeed);
            }
            yield return null;
        }
    }
    private IEnumerator Co_DotDamage(int dotCount, float damage, float second)
    {
        while(dotCount >= 0)
        {
            DecreaseHp(damage);
            yield return new WaitForSeconds(second);
            dotCount--;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(Co_Battle());
        StartCoroutine(Co_StateBattle());
        initializeUnit();
    }
    private void FixedUpdate()
    {
        if (unitState == EUnitState.NonCombat)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if (unitState == EUnitState.Back)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }
    private void Update()
    {
        if (currentHp <= 0)
        {
            if (unitKinds == EUnitKinds.Priest)
            {
                InGameManager.Instance.UpdatePriestList(GetComponent<Priest>());
            }
            InGameManager.Instance.RemoveHealingList(this);
            InGameManager.Instance.UnitList.Remove(this.gameObject);
            UnitPool.ReturnUnit(this);
        }
        if (unitKinds == EUnitKinds.Assasin && transform.position.x > 8.5f)
        {
            transform.position = new Vector3(8.49f, transform.position.y, transform.position.z);
            unitState = EUnitState.Wait;
            Invoke("Invoke_MoveAssasinBack", 1f);
        }
        else if (unitKinds == EUnitKinds.Assasin && transform.position.x < -8.5f)
        {
            transform.position = new Vector3(-8.4f, transform.position.y, transform.position.z);
            unitState = EUnitState.Wait;
            Invoke("Invoke_MoveAssasinForward", 1f);
        }
    }
}
