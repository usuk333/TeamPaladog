using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour //모든 보스 캐릭터들의 능력치 설정, 공격 로직을 호출할 스크립트
{
    private enum EBossKinds
    {
        GoblinKing,
        MagicTool,
        SlaveTrader,
        IntermediateAsmodian,
        Avarice,
        Rage,
        Lust,
        Sloth,
        Gluttony,
        Jealousy,
        Pride,
        Devil
    }
    private enum EBossPattern
    {
        Normal,
        Special
    }
    private float increaseDamage;
    private BossHpBar bossHpBar;
    [SerializeField] private EUnitState bossState = EUnitState.NonCombat;
    [SerializeField] private EBossKinds eBossKinds;
    [SerializeField] private EBossPattern eBossPattern;
    [SerializeField] private Unit currentUnit;
    [SerializeField] private Player player;
    [Header("보스 능력치")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float currentAttackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float knockBackSpeed;
    [SerializeField] private float patternHp = 0.8f;
    public EUnitState BossState { get => bossState; set => bossState = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public Unit CurrentUnit { get => currentUnit; set => currentUnit = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public Player Player { get => player; set => player = value; }

    public void DecreaseHp(float damage)
    {
        currentHp -= damage + increaseDamage;
        bossHpBar.UpdateBossHpUI();
        if(currentHp <= 0)
        {
            if(eBossKinds != EBossKinds.Devil)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        if (currentHp <= maxHp * patternHp)
        {
            if (bossState != EUnitState.KnockBack)
            {
                StartCoroutine(Co_PushOut());
            }
            patternHp -= 0.2f; //해당 부분도 보스별로 다르게 설정하기 위해 변수로 빼야할듯
        }
      
        if(eBossKinds == EBossKinds.Gluttony)
        {
            GetComponent<Gluttony>().AttackGluttony();
        }
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
        bossHpBar.UpdateBossHpUI();
    }
    public void IncreaseDamage(float damage, float value)
    {
        increaseDamage = damage;
        Invoke("Invoke_ResetIncreaseDamage", value);
    }
    public void Stun()
    {
        bossState = EUnitState.Wait;
        Invoke("Invoke_WakeUp", 1f);
    }
    private void Attack(bool isPlayer)
    {
        switch (eBossKinds)
        {
            case EBossKinds.GoblinKing:
                AttackGoblinKing(isPlayer);
                break;
            case EBossKinds.MagicTool:
                AttackBasic(isPlayer);
                break;
            case EBossKinds.SlaveTrader:
                AttackSlaveTrader(isPlayer);
                break;
            case EBossKinds.IntermediateAsmodian:
                AttackInterMediateAsmodian(isPlayer);
                break;
            case EBossKinds.Avarice:
                AttackBasic(isPlayer);
                break;
            case EBossKinds.Rage:
                AttackBasic(isPlayer);
                break;
            case EBossKinds.Lust:
                AttackLust(isPlayer);
                break;
            case EBossKinds.Sloth:
                AttackSloth(isPlayer);
                break;
            case EBossKinds.Gluttony:
                AttackBasic(isPlayer);
                break;
            case EBossKinds.Jealousy:
                AttackBasic(isPlayer);
                break;
            case EBossKinds.Pride:
                AttackBasic(isPlayer);
                break;
            case EBossKinds.Devil:
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
    private void AttackGoblinKing(bool isPlayer)
    {
        var goblinKing = GetComponent<GoblinKing>();
        if(goblinKing.CurrentAttackCount > goblinKing.AttackCount)
        {
            goblinKing.AttackShockWave();
            goblinKing.CurrentAttackCount = 0;
        }
        else
        {
            AttackBasic(isPlayer);
            goblinKing.CurrentAttackCount++;
        }
    }
    private void AttackInterMediateAsmodian(bool isPlayer)
    {
        var interMediateAsmodian = GetComponent<InterMediateAsmodian>();
        if(interMediateAsmodian.CurrentAttackCount > interMediateAsmodian.AttackCount)
        {
            interMediateAsmodian.AttackAllArcher();
            AttackBasic(isPlayer);
            interMediateAsmodian.CurrentAttackCount = 0;
        }
        else
        {
            AttackBasic(isPlayer);
            interMediateAsmodian.CurrentAttackCount++;
        }
    }
    private void AttackSlaveTrader(bool isPlayer)
    {
        var slaveTrader = GetComponent<SlaveTrader>();
        if(slaveTrader.CurrentAttackCount > slaveTrader.AttackCount)
        {
            slaveTrader.Attack();
            slaveTrader.CurrentAttackCount = 0;
        }
        else
        {
            AttackBasic(isPlayer);
            slaveTrader.CurrentAttackCount++;
        }
    }
    private void AttackLust(bool isPlayer)
    {
        AttackBasic(isPlayer);
        var lust = GetComponent<Lust>();
        if(++lust.CurrentAttackCount > lust.AttackCount)
        {
            lust.CurrentAttackCount = 0;
            if (isPlayer)
            {
                player.Stun(lust.StunSecond);
            }
            else
            {
                currentUnit.Stun(lust.StunSecond);
            }
        }
    }
    private void AttackSloth(bool isPlayer)
    {
        var sloth = GetComponent<Sloth>();
        if(sloth.CurrentAttackCount > sloth.AttackCount)
        {
            float attack = attackPower;
            attackPower = attackPower * sloth.Critical;
            AttackBasic(isPlayer);
            attackPower = attack;
            sloth.CurrentAttackCount = 0;
        }
        else
        {
            AttackBasic(isPlayer);
            sloth.CurrentAttackCount++;
        }
    }
    private void Invoke_WakeUp()
    {
        bossState = EUnitState.NonCombat;
    }
    private void Invoke_ResetIncreaseDamage()
    {
        increaseDamage = 0;
    }
    private void DoPatternMagicTool()
    {
        GetComponent<MagicTool>().ActiveLightning();
    }
    private void DoPatternAvarice()
    {
        GetComponent<Avarice>().ActivePoison();
    }
    private void DoPatternRage()
    {
        GetComponent<Rage>().AttackAllUnit();
    }
    private void DoPatternLust()
    {
        GetComponent<Lust>().ActiveTransition();
    }
    private void DoPatternSloth()
    {
        GetComponent<Sloth>().ActiveThorn();
    }
    private void DoPatternGluttony()
    {
        GetComponent<Gluttony>().ActiveEater();
    }
    private void DoPatternJealousy()
    {
        GetComponent<Jealousy>().ActiveCrush();
    }
    private void DoPatternPride()
    {
        GetComponent<Pride>().ActiveDonut();
    }
    private void DoPatternDevil()
    {
        var devil = GetComponent<Devil>();
        if (!devil.IsPageChange)
        {
            patternHp = 1;
            devil.ChangePage();
        }
        else
        {
            devil.ActiveCrush();
        }
    }
    private IEnumerator Co_PushOut()
    {
        Debug.Log("코루틴 호출");
        if(eBossPattern == EBossPattern.Normal)
        {
            bossState = EUnitState.KnockBack;
            yield return new WaitForSeconds(2f);
            bossState = EUnitState.Wait;
            yield return new WaitForSeconds(1f);
            bossState = EUnitState.Battle;
        }
        else
        {
            bossState = EUnitState.KnockBack;
            transform.position = InGameManager.Instance.BossSpawnPoint.position;
            switch (eBossKinds)
            {
                case EBossKinds.MagicTool:
                    DoPatternMagicTool();
                    break;
                case EBossKinds.Avarice:
                    DoPatternAvarice();
                    break;
                case EBossKinds.Rage:
                    DoPatternRage();
                    break;
                case EBossKinds.Lust:
                    DoPatternLust();
                    break;
                case EBossKinds.Sloth:
                    DoPatternSloth();
                    break;
                case EBossKinds.Gluttony:
                    DoPatternGluttony();
                    break;
                case EBossKinds.Jealousy:
                    DoPatternJealousy();
                    break;
                case EBossKinds.Pride:
                    DoPatternPride();
                    break;
                case EBossKinds.Devil:
                    DoPatternDevil();
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
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
        currentHp = maxHp;
        currentMoveSpeed = moveSpeed;
        currentAttackSpeed = attackSpeed;
        bossHpBar = GetComponentInChildren<BossHpBar>();
    }
    private void Start()
    {
        StartCoroutine(Co_Battle());
        StartCoroutine(Co_StateBattle());
    }
    private void FixedUpdate()
    {
        if (bossState == EUnitState.NonCombat)
        {
            transform.position += Vector3.left * currentMoveSpeed * Time.deltaTime;
        }
        if (bossState == EUnitState.KnockBack && eBossPattern != EBossPattern.Special)
        {
            transform.position += Vector3.right * knockBackSpeed * Time.deltaTime;
        }
    }
}
