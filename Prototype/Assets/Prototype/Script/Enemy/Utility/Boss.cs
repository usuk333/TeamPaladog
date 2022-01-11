using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour //��� ���� ĳ���͵��� �ɷ�ġ ����, ���� ������ ȣ���� ��ũ��Ʈ
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
    // �ΰ��� �Ŵ������� hp����, ���� �Լ��� ����� �Ķ���ͷ� ���� hp�� ���ҽ�ų�ų�
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float currentAttackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed;
    private float increaseDamage;
    [SerializeField] private float knockBackSpeed;

    [SerializeField] private Unit currentUnit;
    [SerializeField] private Player player;
    [SerializeField] private float patternHp = 0.8f;
    private BossHpBar bossHpBar;

    public EUnitState BossState { get => bossState; set => bossState = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public Unit CurrentUnit { get => currentUnit; set => currentUnit = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public Player Player { get => player; set => player = value; }

    public void DecreaseHp(float damage)
    {
        currentHp -= damage + increaseDamage;
        bossHpBar.UpdateBossHpUI();
        if (currentHp < maxHp * patternHp)
        {
            StartCoroutine(Co_PushOut());
            patternHp -= 0.2f; //�ش� �κе� �������� �ٸ��� �����ϱ� ���� ������ �����ҵ�
        }
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
                break;
            case EBossKinds.IntermediateAsmodian:
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
        if(goblinKing.AttackCount > 3)
        {
            goblinKing.AttackCount = 0;
            goblinKing.AttackShockWave(attackPower);
        }
        else
        {
            AttackBasic(isPlayer);
            goblinKing.AttackCount++;
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
    private IEnumerator Co_PushOut()
    {
        Debug.Log("�ڷ�ƾ ȣ��");
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
    private IEnumerator Co_Battle() //��Ʋ -> ���Ĺ� ��ȯ�� �����̰� �ణ �ʿ��� ����. (�ʹ� ���� �ٽ� ������)
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
    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
