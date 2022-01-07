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
public class Unit : MonoBehaviour
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
    [SerializeField] private int index;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float cost;

    [SerializeField] private EUnitState unitState = EUnitState.NonCombat;
    [SerializeField] private EUnitKinds unitKinds = EUnitKinds.Warrior;
  
    [SerializeField] private Enemy currentEnemy;
    [SerializeField] private Boss boss;

    public EUnitState UnitState { get => unitState; set => unitState = value; }
    public Enemy CurrentEnemy { get => currentEnemy; set => currentEnemy = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Cost { get => cost; set => cost = value; }
    public EUnitKinds UnitKinds { get => unitKinds; set => unitKinds = value; }
    public Boss Boss { get => boss; set => boss = value; }
    public float AttackPower { get => attackPower; set => attackPower = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Index { get => index; }

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
        else if(unitState == EUnitState.Back)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {     
        if (currentHp <= 0)
        {
            if (unitKinds == EUnitKinds.Priest)
            {
                InGameManager.Instance.UpdatePriestList(GetComponent<Priest>());
            }
            InGameManager.Instance.RemoveHealingList(this);
            //initializeUnit();
            UnitPool.ReturnUnit(this);
        }
        if (unitKinds == EUnitKinds.Assasin && transform.position.x > 8.5f)
        {
            transform.position = new Vector3(8.49f, transform.position.y, transform.position.z);
            unitState = EUnitState.Wait;
            Invoke("MoveAssasinBack", 1f);
        }
        else if(unitKinds == EUnitKinds.Assasin && transform.position.x < -8.5f)
        {
            transform.position = new Vector3(-8.4f, transform.position.y, transform.position.z);
            unitState = EUnitState.Wait;
            Invoke("MoveAssasinForward", 1f);
        }
    }
    private IEnumerator Co_StateBattle()
    {
        while (true)
        {
            if(unitState != EUnitState.Battle && unitState != EUnitState.Wait)
            {
                if (currentEnemy != null || boss != null)
                {
                    unitState = EUnitState.Battle;
                }
            }
            if(unitState == EUnitState.Battle)
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
    void Attack(bool isBoss)
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
    private void AttackWizard()
    {
        GetComponent<Wizard>().AttackWizard();
    }
    private void MoveAssasinBack()
    {
        unitState = EUnitState.Back;
        GetComponent<Assasin>().UnStealth();
    }
    private void MoveAssasinForward()
    {
        unitState = EUnitState.NonCombat;
        GetComponent<Assasin>().Hide();
    }
    private void AttackShielder(bool isBoss)
    {
        AttackBasic(isBoss);
        GetComponent<Shielder>().AttackCount++;
        if(GetComponent<Shielder>().AttackCount >= 3)
        {
            GetComponent<Shielder>().AttackCount = 0;
            if (!isBoss)
            {
                currentEnemy.Stun();
            }
        }
    }
    private void Heal()
    {
        GetComponent<Priest>().Heal();
    }
    private void Summon()
    {
        GetComponent<Summoner>().Summon();    
    }
    public void DecreaseHp(float damage)
    {
        currentHp -= damage;
        GetComponentInChildren<HpBar>().UpdateUnitOrEnemyHpBar(true);
    }
    public void IncreaseHp(float value)
    {
        currentHp += value;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        GetComponentInChildren<HpBar>().UpdateUnitOrEnemyHpBar(true);
    }
    private void AttackDruid(bool isBoss)
    {
        if (GetComponent<Druid>().DruidState == Druid.EDruidState.Wolf)
        {
            GetComponent<Druid>().AttackWolf();
        }
        else
        {
            AttackBasic(isBoss);
        }
    }
    public void Stun()
    {
        unitState = EUnitState.Wait;
        Invoke("Invoke_WakeUp", 2f);
    }
    private void Invoke_WakeUp()
    {
        unitState = EUnitState.Battle;
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
}
