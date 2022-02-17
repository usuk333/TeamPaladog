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
        Invincibility,
        Incapable
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
    private bool isInvincibility;
    private IEnumerator updateState;
    private Rigidbody2D rigidbody2D;
    [SerializeField] private Transform pitch;
    [SerializeField] private Vector3 basicEffectOffset;
    [SerializeField] private Vector3 skillEffectOffset;
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
    public Rigidbody2D Rigidbody2D { get => rigidbody2D; set => rigidbody2D = value; }
    
    public void KnockBack(Vector3 pos)
    {
        eUnitState = EUnitState.Incapable;
        StartCoroutine(Co_KnockBack(pos));
    }
    public void SetInvincibility(float time)
    {
        StartCoroutine(Co_SetInvincibility(time));
    }
    public bool InitializeUnitStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
        spine = GetComponent<SkeletonAnimation>();
        boss = FindObjectOfType<Boss>();
        //maxHp = DB 맥스체력
        //currentHp = maxHp;
        //atackDamage = DB 공격력
        //atackSpeed = DB 공격속도
        //이동속도도?
        return true;
    }
    public void DecreaseHp(float damage) //유닛 Hp 감소 함수(공격하는 쪽에서 호출함)
    {
        if (isInvincibility)
        {
            return;
        }
        currentHp -= damage;
        if(currentHp <= 0)
        {
            StopCoroutine(updateState);
            StartCoroutine(Co_Dead());
        }
    }
    #region -투사체 발사 함수-
    private void SetProjectile(int i)//투사체 발사하는 유닛만 사용. 투사체 풀에서 꺼내오면서 초기화
    {
        var obj = UnitProjectilePool.GetProjectile(i);
        obj.GetComponent<Projectile>().Initialze(boss, transform, attackDamage);
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
                boss.DecreaseHp(attackDamage); //투사체 발사 유닛이 아닌 경우에는 그냥 타격
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
    private void Invoke_UnInvincibility()
    {
        isInvincibility = false;
        eUnitState = EUnitState.Idle;
    }
    private IEnumerator Co_KnockBack(Vector3 pos)
    {
        while(transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        eUnitState = EUnitState.Move;
    }
    private IEnumerator Co_Dead() //유닛 죽는 애니메이션 연출 코루틴
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
    private IEnumerator Co_UpdateState() //유닛 유한상태기계(사실상 무한임)
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            switch (eUnitState)
            {
                case EUnitState.Idle:
                    Debug.Log("코루틴 도는중");
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
                    transform.position = Vector3.MoveTowards(transform.position, pitch.position, moveSpeed * Time.deltaTime);
                    break;
                case EUnitState.Attack:
                    Attack();
                    //공격 애니메이션 재생 시간만큼 코루틴에 지연시간 주기
                    eUnitState = EUnitState.Idle;
                    break;
                case EUnitState.Skill:
                    AttackSkill();
                    
                    //스킬 애니메이션 재생 시간만큼 코루틴에 지연시간 주기
                    eUnitState = EUnitState.Idle;
                    break;
                case EUnitState.Invincibility:
                    Debug.Log("무적");
                    break;
                case EUnitState.Incapable:
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            yield return null;
        }
    }
    private IEnumerator Co_SetInvincibility(float time)
    {
        isInvincibility = true;
        StopCoroutine(updateState);
        yield return new WaitForSeconds(time);
        isInvincibility = false;
        StartCoroutine(updateState);
    }
    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
        updateState = Co_UpdateState();
        currentHp = maxHp;
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
            basicEffect.transform.position = boss.transform.position + basicEffectOffset;
            skillEffect.transform.position = boss.transform.position + skillEffectOffset;
        }
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(updateState);
    }
}
