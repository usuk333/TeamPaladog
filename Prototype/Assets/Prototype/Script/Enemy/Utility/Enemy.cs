using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EEnemyKinds
    {
        Warrior,
        Archer,
        Wizard,
        Shielder,
        Priest,
        Assasin
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

    public EUnitState EnemyState { get => enemyState; set => enemyState = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public Unit CurrentUnit { get => currentUnit; set => currentUnit = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public Player Player { get => player; set => player = value; }
    public float MoveSpeed { get => moveSpeed; }
    public float AttackSpeed { get => attackSpeed; }
    public float AttackPower { get => attackPower; set => attackPower = value; }
    public EEnemyKinds EnemyKinds { get => enemyKinds; set => enemyKinds = value; }
    public int Index { get => index; }

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(Co_Battle());
        StartCoroutine(Co_StateBattle());
        initializeEnemy();
    }

    private void initializeEnemy()
    {
        currentHp = maxHp;
        GetComponentInChildren<HpBar>().ResetHpBar();
        switch (enemyKinds)
        {
            case EEnemyKinds.Warrior:
                break;
            case EEnemyKinds.Archer:
                break;
            case EEnemyKinds.Wizard:
                break;
            case EEnemyKinds.Shielder:
                GetComponent<Shielder>().AttackCount = 0;
                break;
            case EEnemyKinds.Priest:
                GetComponent<Priest>().ClearEnemyList();
                break;
            case EEnemyKinds.Assasin:
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
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
    // Update is called once per frame
    void Update()
    {
        if(currentHp <= 0)
        {
            if (enemyKinds == EEnemyKinds.Priest)
            {
                InGameManager.Instance.UpdateEnemyPriestList(GetComponent<Priest>());
            }
            InGameManager.Instance.RemoveEnemyHealingList(this);
            EnemyPool.ReturnEnemy(this);

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
            if(enemyState == EUnitState.Battle)
            {
                if(currentUnit && !player)
                {
                    Attack(false);
                }
                else if(player && !currentUnit)
                {
                    Attack(true);
                }
                yield return new WaitForSeconds(attackSpeed);
            }       
            yield return null;
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
            default:
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
    public void Stun()
    {
        enemyState = EUnitState.Wait;
        Invoke("WakeUpForInvoke", 2f);
    }
    private void WakeUpForInvoke()
    {
        enemyState = EUnitState.Battle;
    }
    public void DecreaseHp(float damage)
    {
        currentHp -= damage + increaseDamage;
        GetComponentInChildren<HpBar>().UpdateUnitOrEnemyHpBar(false);
    }
    public void IncreaseHp(float value)
    {
        currentHp += value;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        GetComponentInChildren<HpBar>().UpdateUnitOrEnemyHpBar(false);
    }
    public void IncreaseDamage(float damage, float value)
    {
        increaseDamage = damage;
        Invoke("ResetIncreaseDamage", value);
    }
    private void ResetIncreaseDamage()
    {
        increaseDamage = 0;
    }
    private void AttackArcher()
    {
        GetComponent<Archer>().AttackArcher();
    }
    private void AttackShielder(bool isPlayer)
    {
        AttackBasic(isPlayer);
        GetComponent<Shielder>().AttackCount++;
        if (GetComponent<Shielder>().AttackCount >= 3)
        {
            GetComponent<Shielder>().AttackCount = 0;
            if (!isPlayer)
            {
                currentUnit.Stun();
            }
            else
            {
                player.Stun();
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

}
