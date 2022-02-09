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

    public bool InitializeUnitStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
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
        currentHp -= damage;
        if(currentHp <= 0)
        {
            StopCoroutine(Co_UpdateState());
            StartCoroutine(Co_Dead());
        }
    }
    private void Attack() //공격 로직
    {
        switch (eUnitKind)
        {
            case EUnitKind.None:
                boss.DecreaseHp(attackDamage); //투사체 발사 유닛이 아닌 경우에는 그냥 타격
                break;
            //이 밑으로는 투사체 발사 유닛. 유닛 종류에 맞게 투사체 꺼내오는 함수 호출
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
    private void SetProjectile(int i)//투사체 발사하는 유닛만 사용. 투사체 풀에서 꺼내오면서 초기화
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
    private IEnumerator Co_Dead() //유닛 죽는 애니메이션 연출 코루틴
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
    private IEnumerator Co_UpdateState() //유닛 유한상태기계
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
                            yield return new WaitForSeconds(2f); // 나중에 스킬 시전시간 변수로 변경 
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
