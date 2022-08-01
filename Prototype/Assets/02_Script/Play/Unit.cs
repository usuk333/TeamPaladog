using System.Collections;
using UnityEngine;
using Spine.Unity;

public class Unit : MonoBehaviour
{
    public enum EUnitType
    {
        Tanker,
        CloseDealer,
        Wizard,
        RemoteDealer
    }
    public enum EUnitState
    {
        Idle,
        Move,
        Attack,
        Skill,
        Die
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

    //테스트
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
    [SerializeField] private float attackAnimDelay;
    [SerializeField] private float skillAnimDelay;
    [SerializeField] private ParticleSystem healEffect;
    [SerializeField] private ParticleSystem shieldEffect;

    private SkeletonAnimation skeletonAnimation;
    public EUnitType UnitType { get => eUnitType; }
    public CommonStatus CommonStatus { get => commonStatus; set => commonStatus = value; }
    public EUnitState GetUnitState { get => eUnitState; }
    public ParticleSystem HealEffect { get => healEffect; set => healEffect = value; }
    public ParticleSystem ShieldEffect { get => shieldEffect; set => shieldEffect = value; }

    public bool InitializeUnitStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
       // spine = GetComponent<SkeletonAnimation>();
        //maxHp = DB 맥스체력
        //currentHp = maxHp;
        //atackDamage = DB 공격력
        //atackSpeed = DB 공격속도
        //이동속도도?
        return true;
    }
    public void StartAllCoroutines()
    {
        StartCoroutine(Co_UpdateState());
        StartCoroutine(Co_OutOfStateCondition());
    }
    #region -투사체 발사 함수-
    private void SetProjectile(int i)//투사체 발사하는 유닛만 사용. 투사체 풀에서 꺼내오면서 초기화
    {
        var obj = UnitProjectilePool.GetProjectile(i);
        obj.GetComponent<Projectile>().Initialze(InGameManager.Instance.Boss, transform, commonStatus.CurrentAttackDamage);
    }
    #endregion
    private void Attack() //공격 로직
    {
        if (basicEffect != null)
        {
            basicEffect.Play();
        }
        switch (eUnitKind)
        {
            case EUnitKind.Other:
                InGameManager.Instance.Boss.CommonStatus.DecreaseHp(commonStatus.CurrentAttackDamage); //투사체 발사 유닛이 아닌 경우에는 그냥 타격
                break;
            //이 밑으로는 투사체 발사 유닛. 유닛 종류에 맞게 투사체 꺼내오는 함수 호출
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
        if (countUnit) //카운트 유닛 공격횟수 증가
        {
            countUnit.CurrentAttackCount++;
        }
    }
    private void AttackSkill() // 스킬 사용 함수. 카운트 유닛 or 퍼센트 유닛으로 나눠짐
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
    private void TransitionCount()//카운트 유닛 상태 전이 함수
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
    private void TransitionPercentage()//퍼센트 유닛 상태 전이 함수
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
    private IEnumerator Co_Dead() //유닛 죽는 애니메이션 연출 코루틴
    {
        yield return new WaitUntil(() => commonStatus.CurrentHp <= 0);
        eUnitState = EUnitState.Die;
        skeletonAnimation.AnimationState.SetAnimation(0, "Die", false);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    private IEnumerator Co_OutOfStateCondition() //행동 불가 상태 정의
    {
        while (true)
        {
            yield return null;
            if (isKnockBack)
            {
                while(transform.position.x != knockBackPoint.position.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(knockBackPoint.position.x,transform.position.y), commonStatus.KnockBackSpeed * Time.deltaTime);
                    yield return null;
                }
                isKnockBack = false;
                isIncapable = true;
            }
            if (isIncapable)
            {
                yield return new WaitForSeconds(penaltyTime);
                isIncapable = false;
                if(eUnitState != EUnitState.Die)
                {
                    eUnitState = EUnitState.Move;
                    skeletonAnimation.AnimationState.SetAnimation(0, "Move", true);
                }
            }
        }
    }
    private IEnumerator Co_UpdateState() //유닛 유한상태기계(사실상 무한임)
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(Co_Dead());
        while (eUnitState != EUnitState.Die)
        {
            yield return null;
            if (isKnockBack || isIncapable)
            {
                continue;
            }
            switch (eUnitState)
            {
                case EUnitState.Idle:
                    // Debug.Log("코루틴 도는중");
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
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(pitch.position.x,transform.position.y),commonStatus.CurrentMoveSpeed*Time.deltaTime);
                    if (transform.position.x == pitch.position.x)
                    {
                        eUnitState = EUnitState.Idle;
                        skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                    }
                    break;
                case EUnitState.Attack:
                    if(skeletonAnimation != null)
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
                        skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, attackAnimDelay);
                    }
                    Attack();
                    yield return null;
                    //공격 애니메이션 재생 시간만큼 코루틴에 지연시간 주기
                    eUnitState = EUnitState.Idle;
                    break;
                case EUnitState.Skill:
                    if (skeletonAnimation != null)
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "Skill", false);
                        skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, skillAnimDelay);
                    }
                    AttackSkill();
                    yield return null;
                    //스킬 애니메이션 재생 시간만큼 코루틴에 지연시간 주기
                    eUnitState = EUnitState.Idle;
                    break;
                case EUnitState.Die:
                    yield break;
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
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if(skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }   
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateState());
        StartCoroutine(Co_OutOfStateCondition());
        if (transform.childCount > 0)
        {
           // basicEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
          //  skillEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
           // basicEffect.transform.position = InGameManager.Instance.Boss.transform.position + basicEffectOffset;
           // skillEffect.transform.position = InGameManager.Instance.Boss.transform.position + skillEffectOffset;
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
