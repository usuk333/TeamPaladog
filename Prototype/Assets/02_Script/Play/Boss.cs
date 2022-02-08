using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{ 
    private enum EBossState
    {
        Idle,
        Attack,
        Skill
    }
    private Player player;
    [SerializeField] private Unit currentUnit;
    [SerializeField] private float maxHp; // ���� �ִ� ü��, ���̵��� ���� �� �ε� �� �ʱ�ȭ
    [SerializeField] private float currentHp; // ���� ���� ü��. �� �ε� �� maxHp�� �ʱ�ȭ
    [SerializeField] private float attackDamage; // ���� ���ݷ�. ���̵��� ���� �� �ε� �� �ʱ�ȭ
    [SerializeField] private float attackSpeed; // ���� ���� �ӵ�. ���̵��� ���� �� �ε� �� �ʱ�ȭ
    [SerializeField] private EBossState eBossState;
    public float MaxHp { get => maxHp; }
    public float CurrentHp { get => currentHp; }

    public void InitializeBossStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
    }
    public void DecreaseHp(float damage)//ü�� ���� �Լ� (�����ϴ� �ʿ��� ȣ��)
    {
        currentHp -= damage;
    }
    private void Attack() //���� ��Ÿ
    {
        if(currentUnit == null) //���� �� ������ �÷��̾� ����
        {
            player.DecreaseHp(attackDamage);
            return;
        }
        currentUnit.DecreaseHp(attackDamage);
    }
    private IEnumerator Co_UpdateUnitReference()
    {
        //�ΰ��ӸŴ����� ���� ����Ʈ �ε����� ���� �Ҵ�����ָ� �� �� (0,1,2,3 ������ ��Ŀ, �ٵ�, ������, ���Ÿ� ����)
        int i = 0;
        currentUnit = InGameManager.Instance.Units[i];
        while (true)
        {
            if(currentUnit.CurrentHp <= 0)
            {
                if(i >= 3)
                {
                    currentUnit = null;
                    yield break;
                }
                currentUnit = InGameManager.Instance.Units[++i];
            }
            yield return null;
        }
    }
    private IEnumerator Co_UpdateState() // ���� ���ѻ��±��
    {
        while (true)
        {
            switch (eBossState)
            {
                case EBossState.Idle:
                    yield return new WaitForSeconds(attackSpeed);
                    eBossState = EBossState.Attack;
                    break;
                case EBossState.Attack:
                    Attack();
                    eBossState = EBossState.Idle;
                    break;
                case EBossState.Skill:
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()//�ӽ÷� �ʱ�ȭ
    {
        currentHp = maxHp;
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateUnitReference());
        StartCoroutine(Co_UpdateState());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
