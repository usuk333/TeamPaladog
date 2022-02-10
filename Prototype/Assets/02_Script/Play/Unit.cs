using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.AnimationTools;

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
        Other,
        Taoist,
        Mechanic,
        Mage
    }
    private enum EUnitValue
    {
        Count,
        Percentage
    }
    private SkeletonAnimation spine;
    private CountUnit countUnit;
    private PercentageUnit percentageUnit;
    [SerializeField] private ParticleSystem basicEffect;
    [SerializeField] private ParticleSystem skillEffect;
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
    public float AttackDamage { get => attackDamage; }

    public bool InitializeUnitStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
        spine = GetComponent<SkeletonAnimation>();
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
    #region -����ü �߻� �Լ�-
    private void SetProjectile(int i)//����ü �߻��ϴ� ���ָ� ���. ����ü Ǯ���� �������鼭 �ʱ�ȭ
    {
        var obj = UnitProjectilePool.GetProjectile(i);
        obj.GetComponent<Projectile>().Initialze(boss, transform, attackDamage);
    }
    #endregion
    private void Attack() //���� ����
    {
        if (basicEffect != null)
        {
            basicEffect.Play();
        }
        switch (eUnitKind)
        {
            case EUnitKind.Other:
                boss.DecreaseHp(attackDamage); //����ü �߻� ������ �ƴ� ��쿡�� �׳� Ÿ��
                break;
            //�� �����δ� ����ü �߻� ����. ���� ������ �°� ����ü �������� �Լ� ȣ��
            case EUnitKind.Taoist:
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
        if (countUnit) //ī��Ʈ ���� ����Ƚ�� ����
        {
            countUnit.CurrentAttackCount++;
        }
    }
    private void AttackSkill() // ��ų ��� �Լ�. ī��Ʈ ���� or �ۼ�Ʈ �������� ������
    {
        if (skillEffect != null)
        {
            skillEffect.Play();
        }
        if (countUnit)
        {
            countUnit.UseSkill();
        }
        else if (percentageUnit)
        {
            percentageUnit.UseSkill();
        }
    }
    private void TransitionCount()//ī��Ʈ ���� ���� ���� �Լ�
    {
        if (countUnit.CurrentAttackCount >= countUnit.AttackCount)
        {
            countUnit.CurrentAttackCount = 0;
            eUnitState = EUnitState.Skill;         
        }
        else
        {
            eUnitState = EUnitState.Attack;
        }
    }
    private void TransitionPercentage()//�ۼ�Ʈ ���� ���� ���� �Լ�
    {
        float rand = Random.value * 100;
        Debug.Log(rand);
        if (rand <= percentageUnit.SkillPercentage)
        {
            eUnitState = EUnitState.Skill;
        }
        else
        {
            eUnitState = EUnitState.Attack;
        }
    }
    private IEnumerator Co_Dead() //���� �״� �ִϸ��̼� ���� �ڷ�ƾ
    {
        yield return null;
        /*var material = GetComponent<MeshRenderer>().material;
        float alpha = 1;
        while(material.color.a > 0)
        {
            material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
            alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        transform.gameObject.SetActive(false);*/
    }
    private IEnumerator Co_UpdateState() //���� ���ѻ��±��(��ǻ� ������)
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            switch (eUnitState)
            {
                case EUnitState.Idle:
                    yield return new WaitForSeconds(attackSpeed);
                    if(currentHp > 0)
                    {
                        switch (eUnitValue)
                        {
                            case EUnitValue.Count:
                                TransitionCount();
                                break;
                            case EUnitValue.Percentage:
                                TransitionPercentage();
                                break;
                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }
                    break;
                case EUnitState.Move:
                    //DoMoveAnim
                    break;
                case EUnitState.Attack:
                    Attack();
                    //���� �ִϸ��̼� ��� �ð���ŭ �ڷ�ƾ�� �����ð� �ֱ�
                    eUnitState = EUnitState.Idle;
                    break;
                case EUnitState.Skill:
                    AttackSkill();
                    
                    //��ų �ִϸ��̼� ��� �ð���ŭ �ڷ�ƾ�� �����ð� �ֱ�
                    eUnitState = EUnitState.Idle;
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
        //spine = GetComponent<SkeletonAnimation>();
        if (eUnitValue == EUnitValue.Count)
        {
            countUnit = GetComponent<CountUnit>();  
        }
        else
        {
            percentageUnit = GetComponent<PercentageUnit>();
        }
        if (transform.childCount > 0)
        {
            basicEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
            skillEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
        }
        boss = FindObjectOfType<Boss>();
        currentHp = maxHp;
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateState());
    }
}
