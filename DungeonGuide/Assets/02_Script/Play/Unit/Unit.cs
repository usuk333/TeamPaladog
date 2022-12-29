using System.Collections;
using UnityEngine;
using Spine.Unity;
using System.Linq;

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
    [SerializeField] private CommonStatus commonStatus = new CommonStatus();
    [SerializeField] private float attackAnimDelay;
    [SerializeField] private float skillAnimDelay;
    [SerializeField] private float attackEffectDelay;
    [SerializeField] private float skillEffectDelay;
    [SerializeField] private ParticleSystem healEffect;
    [SerializeField] private ParticleSystem shieldEffect;
    [SerializeField] private ParticleSystem rageEffect;

    public int skillCondition;

    private SkeletonAnimation skeletonAnimation;
    public EUnitType UnitType { get => eUnitType; }
    public CommonStatus CommonStatus { get => commonStatus; set => commonStatus = value; }
    public ParticleSystem HealEffect { get => healEffect; set => healEffect = value; }
    public ParticleSystem ShieldEffect { get => shieldEffect; set => shieldEffect = value; }
    public ParticleSystem RageEffect { get => rageEffect; set => rageEffect = value; }

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
    private void Attack() //공격 로직
    {
        if (basicEffect != null)
        {
            basicEffect.Play();
        }
        InGameManager.Instance.Boss.CommonStatus.DecreaseHp(commonStatus.CurrentAttackDamage);
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
        if (countUnit.CurrentAttackCount >= skillCondition)
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
        if (rand <= skillCondition)
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
        InGameManager.Instance.Units.Remove(this);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<BoxCollider2D>().enabled = false;
        eUnitState = EUnitState.Die;
        skeletonAnimation.AnimationState.TimeScale = 0.8f;
        skeletonAnimation.AnimationState.SetAnimation(0, "Die", false);
        yield return new WaitForSeconds(2.3f);
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
        yield return new WaitForSeconds(1f);
        eUnitState = EUnitState.Move;
        skeletonAnimation.AnimationState.SetAnimation(0, "Move", true);
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
                    yield return new WaitForSeconds(attackEffectDelay);
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
                    yield return new WaitForSeconds(skillEffectDelay);
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
        commonStatus.CurrentHp = commonStatus.MaxHp;
        commonStatus.CurrentMoveSpeed = commonStatus.MoveSpeed;
        commonStatus.CurrentAttackDamage = commonStatus.AttackDamage;
        StartCoroutine(Co_UpdateState());
        StartCoroutine(Co_OutOfStateCondition());
        if (basicEffect != null)
        {
            basicEffect.transform.position = InGameManager.Instance.Boss.transform.position + basicEffectOffset;
        }
        if(skillEffect != null)
        {
            skillEffect.transform.position = InGameManager.Instance.Boss.transform.position + skillEffectOffset;
        }
    }
    private void Update()
    {
        if(eUnitState == EUnitState.Die)
        {
            if(commonStatus.CurrentHp != 0)
            {
                commonStatus.CurrentHp = 0;
            }
        }
    }
    private void LateUpdate()
    {
        if(basicEffect != null)
        {
            basicEffect.transform.position = InGameManager.Instance.Boss.transform.position + basicEffectOffset;
        }
        if(skillEffect != null)
        {
            skillEffect.transform.position = InGameManager.Instance.Boss.transform.position + skillEffectOffset;
        }
    }
}
