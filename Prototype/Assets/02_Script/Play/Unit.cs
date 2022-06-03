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

    //�׽�Ʈ
    public bool isKnockBack { get; set; }
    public float penaltyTime { get; set; }
    public bool isIncapable { get; set; }
    [SerializeField] private Transform knockBackPoint;
   // private SkeletonAnimation spine;
    private CountUnit countUnit;
    private PercentageUnit percentageUnit;

    [SerializeField] private Transform pitch;
    [SerializeField] private Vector3 basicEffectOffset;
    [SerializeField] private Vector3 skillEffectOffset;
    [SerializeField] private ParticleSystem basicEffect;
    [SerializeField] private ParticleSystem skillEffect;
    [SerializeField] private EUnitValue eUnitValue;
    [SerializeField] private EUnitState eUnitState = EUnitState.Idle;
    [SerializeField] private EUnitType eUnitType;
    [SerializeField] private EUnitKind eUnitKind;
    [SerializeField] private CommonStatus commonStatus = new CommonStatus();

    public EUnitType UnitType { get => eUnitType; }
    public CommonStatus CommonStatus { get => commonStatus; set => commonStatus = value; }
    public bool InitializeUnitStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
       // spine = GetComponent<SkeletonAnimation>();
        //maxHp = DB �ƽ�ü��
        //currentHp = maxHp;
        //atackDamage = DB ���ݷ�
        //atackSpeed = DB ���ݼӵ�
        //�̵��ӵ���?
        return true;
    }
    public void StartAllCoroutines()
    {
        StartCoroutine(Co_UpdateState());
        StartCoroutine(Co_OutOfStateCondition());
    }
    #region -����ü �߻� �Լ�-
    private void SetProjectile(int i)//����ü �߻��ϴ� ���ָ� ���. ����ü Ǯ���� �������鼭 �ʱ�ȭ
    {
        var obj = UnitProjectilePool.GetProjectile(i);
        obj.GetComponent<Projectile>().Initialze(InGameManager.Instance.Boss, transform, commonStatus.CurrentAttackDamage);
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
                InGameManager.Instance.Boss.CommonStatus.DecreaseHp(commonStatus.CurrentAttackDamage); //����ü �߻� ������ �ƴ� ��쿡�� �׳� Ÿ��
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
    private IEnumerator Co_OutOfStateCondition() //�ൿ �Ұ� ���� ����
    {
        while (true)
        {
            yield return null;
            if (isKnockBack)
            {
                while(transform.position != knockBackPoint.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, knockBackPoint.position, commonStatus.KnockBackSpeed * Time.deltaTime);
                    yield return null;
                }
                isKnockBack = false;
                isIncapable = true;
            }
            if (isIncapable)
            {
                yield return new WaitForSeconds(penaltyTime);
                isIncapable = false;
                eUnitState = EUnitState.Move;
            }
        }
    }
    private IEnumerator Co_UpdateState() //���� ���ѻ��±��(��ǻ� ������)
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            yield return null;
            if (isKnockBack || isIncapable)
            {
                continue;
            }
            switch (eUnitState)
            {
                case EUnitState.Idle:
                   // Debug.Log("�ڷ�ƾ ������");
                    yield return new WaitForSeconds(commonStatus.AttackSpeed);
                    if(commonStatus.CurrentHp > 0)
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
                    yield return null;
                    break;
                case EUnitState.Move:
                    transform.position = Vector3.MoveTowards(transform.position, pitch.position,commonStatus.CurrentMoveSpeed*Time.deltaTime);
                    if (transform.position == pitch.position)
                    {
                        eUnitState = EUnitState.Idle;
                    }
                    break;
                case EUnitState.Attack:
                    Attack();
                    yield return null;
                    //���� �ִϸ��̼� ��� �ð���ŭ �ڷ�ƾ�� �����ð� �ֱ�
                    eUnitState = EUnitState.Idle;
                    break;
                case EUnitState.Skill:
                    AttackSkill();
                    yield return null;
                    //��ų �ִϸ��̼� ��� �ð���ŭ �ڷ�ƾ�� �����ð� �ֱ�
                    eUnitState = EUnitState.Idle;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
    private void Awake()
    {
        commonStatus.CurrentHp = commonStatus.MaxHp;
        commonStatus.CurrentMoveSpeed = commonStatus.MoveSpeed;
        //spine = GetComponent<SkeletonAnimation>();
        if (eUnitValue == EUnitValue.Count)
        {
            countUnit = GetComponent<CountUnit>();  
        }
        else
        {
            percentageUnit = GetComponent<PercentageUnit>();
        }
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateState());
        StartCoroutine(Co_OutOfStateCondition());
        if (transform.childCount > 0)
        {
            basicEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
            skillEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
            basicEffect.transform.position = InGameManager.Instance.Boss.transform.position + basicEffectOffset;
            skillEffect.transform.position = InGameManager.Instance.Boss.transform.position + skillEffectOffset;
        }
    }
    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.A))
        {
            isKnockBack = true;
            penaltyTime = 5f;
        }*/
    }
}
