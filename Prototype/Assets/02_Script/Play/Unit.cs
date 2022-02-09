using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum EUnitType
    {
        Tanker,
        CloseDealer,
        Wizard,
        RemoteDealer
    }
    private enum EUnitState
    {
        Idle,
        Move,
        Attack,
        Skill,
    }
    private enum EUnitKind
    {
        None,
        Archer,
        Mechanic,
        Mage
    }
    private enum EUnitValue
    {
        Count,
        Percentage
    }
    [SerializeField] private EUnitValue eUnitValue;
    [SerializeField] private EUnitState eUnitState = EUnitState.Idle;
    [SerializeField] private Boss boss;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private EUnitType eUnitType;
    [SerializeField] private EUnitKind eUnitKind;

    public EUnitType UnitType { get => eUnitType; }
    public Boss Boss { get => boss; }
    public float MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            currentHp = maxHp;
        }
    }
    public float CurrentHp { get => currentHp; set => currentHp = value; }

    public bool InitializeUnitStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
        boss = FindObjectOfType<Boss>();
        //maxHp = DB �ƽ�ü��
        //currentHp = maxHp;
        //atackDamage = DB ���ݷ�
        //atackSpeed = DB ���ݼӵ�
        //�̵��ӵ���?
        return true;
    }
    public void DecreaseHp(float damage) //���� Hp ���� �Լ�(�����ϴ� �ʿ��� ȣ����)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            StopCoroutine(Co_UpdateState());
            StartCoroutine(Co_Dead());
        }
    }
    private void Attack() //���� ����
    {
        switch (eUnitKind)
        {
            case EUnitKind.None:
                boss.DecreaseHp(attackDamage); //����ü �߻� ������ �ƴ� ��쿡�� �׳� Ÿ��
                break;
            //�� �����δ� ����ü �߻� ����. ���� ������ �°� ����ü �������� �Լ� ȣ��
            case EUnitKind.Archer:
                SetProjectile(0);
                break;
            case EUnitKind.Mechanic:
                SetProjectile(1);
                break;
            case EUnitKind.Mage:
                SetProjectile(2);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    private void SetProjectile(int i)//����ü �߻��ϴ� ���ָ� ���. ����ü Ǯ���� �������鼭 �ʱ�ȭ
    {
        var obj = UnitProjectilePool.GetProjectile(i);
        obj.GetComponent<Projectile>().Initialze(boss, transform, attackDamage);
    }
    private bool AttackSpecial_Count()
    {
        var count = GetComponent<CountUnit>();
        if (count.CurrentAttackCount < count.AttackCount)
        {
            Attack();
            count.CurrentAttackCount++;
            return true;
        }
        else
        {
            count.AttackSpecial();
            count.CurrentAttackCount = 0;
            return false;
        }
    }
    private void AttackPercentage()
    {
        //DoSomething
    }
    private IEnumerator Co_Dead() //���� �״� �ִϸ��̼� ���� �ڷ�ƾ
    {
        var sprite = GetComponent<SpriteRenderer>();
        float alpha = 1;
        while(sprite.color.a > 0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        transform.gameObject.SetActive(false);
    }
    private IEnumerator Co_UpdateState() //���� ���ѻ��±��
    {
        while (true)
        {
            switch (eUnitState)
            {
                case EUnitState.Idle:
                    yield return new WaitForSeconds(attackSpeed);
                    if(currentHp > 0)
                    {
                        eUnitState = EUnitState.Attack;
                    }
                    break;
                case EUnitState.Move:
                    //DoMoveAnim
                    break;
                case EUnitState.Attack:
                    if(eUnitValue == EUnitValue.Count)
                    {
                        if (AttackSpecial_Count())
                        {
                            eUnitState = EUnitState.Idle;
                        }
                        else
                        {
                            yield return new WaitForSeconds(2f); // ���߿� ��ų �����ð� ������ ���� 
                            eUnitState = EUnitState.Idle;
                        }
                    }
                    else
                    {
                        AttackPercentage();
                    }
                    eUnitState = EUnitState.Idle;
                    break;
                case EUnitState.Skill:
                    //DoSkill
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            yield return null;
        }
    }
    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
        currentHp = maxHp;
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateState());
    }
}
