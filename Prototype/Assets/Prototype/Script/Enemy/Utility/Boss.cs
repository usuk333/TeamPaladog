using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private enum EBossKinds
    {
        GoblinKing,
        MagicTool,
        SlaveTrader,
        IntermediateAsmodian
    }
    private enum EBossPattern
    {
        Normal,
        Special
    }
    [SerializeField] private EUnitState bossState = EUnitState.NonCombat;
    [SerializeField] private EBossKinds eBossKinds;
    [SerializeField] private EBossPattern eBossPattern;
    // 인게임 매니저에다 hp증가, 감소 함수를 만들고 파라메터로 누구 hp를 감소시킬거냐
    [SerializeField] private float attackSpeed;
    [SerializeField] private float currentAttackSpeed;

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float increaseDamage;

    [SerializeField] private Unit currentUnit;
    [SerializeField] private Player player;

    [SerializeField] private float patternHp = 0.8f;

    public EUnitState BossState { get => bossState; set => bossState = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public Unit CurrentUnit { get => currentUnit; set => currentUnit = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public Player Player { get => player; set => player = value; }
    public float CurrentAttackSpeed { get => currentAttackSpeed; set => currentAttackSpeed = value; }
    public float CurrentMoveSpeed { get => currentMoveSpeed; set => currentMoveSpeed = value; }
    public float AttackSpeed { get => attackSpeed; }
    public float MoveSpeed { get => moveSpeed; }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_Battle());
        StartCoroutine(Co_StateBattle());
        currentHp = maxHp;
        currentMoveSpeed = moveSpeed;
        currentAttackSpeed = attackSpeed;
    }

    private void FixedUpdate()
    {
        if (bossState == EUnitState.NonCombat)
        {
            transform.position += Vector3.left * currentMoveSpeed * Time.deltaTime;
        }
        if(bossState == EUnitState.KnockBack)
        {
            transform.position += Vector3.right * 2 * Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private IEnumerator Co_StateBattle()
    {
        while (true)
        {
            if (bossState != EUnitState.Battle && bossState != EUnitState.Wait && bossState != EUnitState.KnockBack)
            {
                if (currentUnit != null || player != null)
                {
                    bossState = EUnitState.Battle;
                }
            }
            if (bossState == EUnitState.Battle)
            {
                if (currentUnit == null && player == null)
                {
                    bossState = EUnitState.NonCombat;
                }
            }
            yield return null;
        }
    }
    private IEnumerator Co_Battle() //배틀 -> 논컴뱃 전환에 딜레이가 약간 필요해 보임. (너무 빨리 다시 추적함)
    {
        while (true)
        {
            if (bossState == EUnitState.Battle)
            {
                if (currentUnit && !player)
                {
                    Attack(false);
                }
                else if (player && !currentUnit)
                {
                    Attack(true);
                }
                yield return new WaitForSeconds(currentAttackSpeed);
            }
            yield return null;
        }
    }
    private void Attack(bool isPlayer)
    {
        switch (eBossKinds)
        {
            case EBossKinds.GoblinKing:
                AttackGoblinKing(isPlayer);
                break;
            case EBossKinds.MagicTool:
                AttackMagicTool(isPlayer);
                break;
            case EBossKinds.SlaveTrader:
                break;
            case EBossKinds.IntermediateAsmodian:
                break;
            default:
                break;
        }
    }
    private void AttackGoblinKing(bool isPlayer)
    {
        if(GetComponent<GoblinKing>().AttackCount > 3)
        {
            GetComponent<GoblinKing>().AttackCount = 0;
            GetComponent<GoblinKing>().AttackShockWave(attackPower);
        }
        else
        {
            AttackBasic(isPlayer);
            GetComponent<GoblinKing>().AttackCount++;
        }
    }
    private void AttackMagicTool(bool isPlayer)
    {
        if(GetComponent<MaigcTool>().AttackCount > 1)
        {
            GetComponent<MaigcTool>().AttackCount = 0;
            GetComponent<MaigcTool>().ActiveLightning();
        }
        else
        {
            AttackBasic(isPlayer);
            GetComponent<MaigcTool>().AttackCount++;
        }
       
    }
    void AttackBasic(bool isPlayer)
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
        bossState = EUnitState.Wait;
        Invoke("WakeUpForInvoke", 1f);
    }
    private void WakeUpForInvoke()
    {
        bossState = EUnitState.NonCombat;
    }
    public void DecreaseHp(float damage)
    {
        currentHp -= damage + increaseDamage;
        GetComponentInChildren<BossHpBar>().UpdateBossHpUI();
        if(currentHp < maxHp * patternHp)
        {
            StartCoroutine(PushOut());
            patternHp -= 0.2f;
        }
    }
    public void IncreaseHp(float value)
    {
        currentHp += value;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        GetComponentInChildren<BossHpBar>().UpdateBossHpUI();
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

    private IEnumerator PushOut()
    {
        Debug.Log("코루틴 호출");
        bossState = EUnitState.KnockBack;
        yield return new WaitForSeconds(2f);
        bossState = EUnitState.Wait;
        yield return new WaitForSeconds(1f);
        bossState = EUnitState.Battle;
    }
}
