using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour //적 유닛들의 능력치 설정, 공격 로직을 호출하는 스크립트
{
    public enum EEnemyKinds
    {
        Warrior,
        Archer,
        Wizard,
        Shielder,
        Priest,
        Assasin,
        BomberMan
    }
    [SerializeField] private int index;
    [SerializeField] private EUnitState enemyState = EUnitState.NonCombat;
    [SerializeField] private EEnemyKinds enemyKinds;

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;   
    [SerializeField] private float increaseDamage;

    [SerializeField] private Unit currentUnit;
    [SerializeField] private Player player;
    private HpBar hpBar;

    public EUnitState EnemyState { get => enemyState; set => enemyState = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public Unit CurrentUnit { get => currentUnit; set => currentUnit = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public Player Player { get => player; set => player = value; }
    public float AttackPower { get => attackPower; set => attackPower = value; }
    public int Index { get => index; }
    public float MoveSpeed { get => moveSpeed; }

    public void DecreaseHp(float damage)
    {
        currentHp -= damage + increaseDamage;
        hpBar.UpdateUnitOrEnemyHpBar();
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
        hpBar.UpdateUnitOrEnemyHpBar();
    }
    public void IncreaseDamage(float damage, float value)
    {
        increaseDamage = damage;
        Invoke("Invoke_ResetIncreaseDamage", value);
    }
    public void Stun(float second)
    {
        enemyState = EUnitState.Wait;
        Invoke("Invoke_WakeUp", second);
    }
    private void initializeEnemy()
    {
        currentHp = maxHp;
        GetComponentInChildren<HpBar>().ResetHpBar();
        switch (enemyKinds)
        {
            case EEnemyKinds.Shielder:
                GetComponent<Shielder>().AttackCount = 0;
                break;
            case EEnemyKinds.Priest:
                GetComponent<Priest>().ClearEnemyList();
                break;
            default:
                break;
        }
    }
    private void Attack(bool isPlayer)
    {
        switch (enemyKinds)
        {
            case EEnemyKinds.Warrior:
                AttackBasic(isPlayer);
                break;
            case EEnemyKinds.Archer:
                AttackArcher();
                break;
            case EEnemyKinds.Wizard:
                AttackWizard();
                break;
            case EEnemyKinds.Shielder:
                AttackShielder(isPlayer);
                break;
            case EEnemyKinds.Priest:
                Heal();
                break;
            case EEnemyKinds.Assasin:
                AttackBasic(isPlayer);
                break;
            case EEnemyKinds.BomberMan:
                ExplodeBomb();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    private void AttackBasic(bool isPlayer)
    {
        if (!isPlayer)
        {
            currentUnit.DecreaseHp(attackPower);
        }
        else
        {
            player.DecreaseHp(attackPower);
        }
    }
    private void AttackArcher()
    {
        GetComponent<Archer>().AttackArcher();
    }
    private void AttackShielder(bool isPlayer)
    {
        AttackBasic(isPlayer);
        var shielder = GetComponent<Shielder>();
        shielder.AttackCount++;
        if (shielder.AttackCount >= 3)
        {
            shielder.AttackCount = 0;
            if (!isPlayer)
            {
                currentUnit.Stun(2f);
            }
            else
            {
                player.Stun(2f);
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
    private void MoveAssasinBack()
    {
        enemyState = EUnitState.Back;
        GetComponent<Assasin>().UnStealth();
    }
    private void MoveAssasinForward()
    {
        enemyState = EUnitState.NonCombat;
        GetComponent<Assasin>().Hide();
    }
    private void ExplodeBomb()
    {
        GetComponent<BomberMan>().AttackBomb(attackPower);
    }
    private void Invoke_WakeUp()
    {
        enemyState = EUnitState.Battle;
    }
    private void Invoke_ResetIncreaseDamage()
    {
        increaseDamage = 0;
    }
    private IEnumerator Co_StateBattle()
    {
        while (true)
        {
            if (enemyState != EUnitState.Battle && enemyState != EUnitState.Wait)
            {
                if (currentUnit != null || player != null)
                {
                    enemyState = EUnitState.Battle;
                }
            }
            if (enemyState == EUnitState.Battle)
            {
                if (currentUnit == null && player == null)
                {
                    if (enemyKinds == EEnemyKinds.Assasin)
                    {
                        enemyState = EUnitState.Back;
                    }
                    else
                    {
                        enemyState = EUnitState.NonCombat;
                    }
                }
            }
            yield return null;
        }
    }
    private IEnumerator Co_Battle() //배틀 -> 논컴뱃 전환에 딜레이가 약간 필요해 보임. (너무 빨리 다시 추적함)
    {
        while (true)
        {
            if (enemyState == EUnitState.Battle)
            {
                if (currentUnit && !player)
                {
                    Attack(false);
                }
                else if (player && !currentUnit)
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
        while (dotCount >= 0)
        {
            DecreaseHp(damage);
            yield return new WaitForSeconds(second);
            dotCount--;
        }
    }
    private void Awake()
    {
        hpBar = GetComponentInChildren<HpBar>();
    }
    private void OnEnable()
    {
        StartCoroutine(Co_Battle());
        StartCoroutine(Co_StateBattle());
        initializeEnemy();
    }
    private void Update()
    {
        if (currentHp <= 0)
        {
            if (enemyKinds == EEnemyKinds.Priest)
            {
                InGameManager.Instance.UpdateEnemyPriestList(GetComponent<Priest>());
            }
            InGameManager.Instance.RemoveEnemyHealingList(this);
            if(enemyKinds != EEnemyKinds.BomberMan)
            {
                EnemyPool.ReturnEnemy(this);
                return;
            }
            else
            {
                FindObjectOfType<Lust>().ReturnBomberMan(GetComponent<BomberMan>());
            }
        }
        if (enemyKinds == EEnemyKinds.Assasin && transform.position.x > 8.5f)
        {
            transform.position = new Vector3(8.49f, transform.position.y, transform.position.z);
            enemyState = EUnitState.Wait;
            Invoke("MoveAssasinForward", 1f);
        }
        else if (enemyKinds == EEnemyKinds.Assasin && transform.position.x < -8.5f)
        {
            transform.position = new Vector3(-8.49f, transform.position.y, transform.position.z);
            enemyState = EUnitState.Wait;
            Invoke("MoveAssasinBack", 1f);
        }
    }
    private void FixedUpdate()
    {
        if(enemyKinds != EEnemyKinds.BomberMan)
        {
            if (enemyState == EUnitState.NonCombat)
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
            else if (enemyState == EUnitState.Back)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
        }
    }
}
