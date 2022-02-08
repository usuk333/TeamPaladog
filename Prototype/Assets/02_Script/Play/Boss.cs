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
    [SerializeField] private float maxHp; // 보스 최대 체력, 난이도에 따라 씬 로딩 시 초기화
    [SerializeField] private float currentHp; // 보스 현재 체력. 씬 로딩 시 maxHp로 초기화
    [SerializeField] private float attackDamage; // 보스 공격력. 난이도에 따라 씬 로딩 시 초기화
    [SerializeField] private float attackSpeed; // 보스 공격 속도. 난이도에 따라 씬 로딩 시 초기화
    [SerializeField] private EBossState eBossState;
    public float MaxHp { get => maxHp; }
    public float CurrentHp { get => currentHp; }

    public void InitializeBossStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
    }
    public void DecreaseHp(float damage)//체력 감소 함수 (공격하는 쪽에서 호출)
    {
        currentHp -= damage;
    }
    private void Attack() //보스 평타
    {
        if(currentUnit == null) //유닛 다 죽으면 플레이어 공격
        {
            player.DecreaseHp(attackDamage);
            return;
        }
        currentUnit.DecreaseHp(attackDamage);
    }
    private IEnumerator Co_UpdateUnitReference()
    {
        //인게임매니저의 유닛 리스트 인덱스를 각각 할당시켜주면 될 듯 (0,1,2,3 순서로 탱커, 근딜, 마법사, 원거리 딜러)
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
    private IEnumerator Co_UpdateState() // 보스 유한상태기계
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
    private void Awake()//임시로 초기화
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
