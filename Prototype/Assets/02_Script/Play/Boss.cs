using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{ 
    private enum EBossState
    {
        Idle,
        Attack,
        Skill,
        Dead
    }
    [SerializeField] private Unit unit;
    [SerializeField] private float maxHp; // 보스 최대 체력, 난이도에 따라 씬 로딩 시 초기화
    [SerializeField] private float currentHp; // 보스 현재 체력. 씬 로딩 시 maxHp로 초기화
    [SerializeField] private float attackDamage; // 보스 공격력. 난이도에 따라 씬 로딩 시 초기화
    [SerializeField] private float attackSpeed; // 보스 공격 속도. 난이도에 따라 씬 로딩 시 초기화
    public float MaxHp { get => maxHp; }
    public float CurrentHp { get => currentHp; }

    public void InitializeBossStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
    }
    private void AttackBasic() //보스 평타
    {
        unit.DecreaseHp(attackDamage);
    }
    private void UpdateUnitReference()
    {
        //인게임매니저의 유닛 리스트 인덱스를 각각 할당시켜주면 될 듯 (0,1,2,3 순서로 탱커, 근딜, 마법사, 원거리 딜러)
    }
    private void Awake()//임시로 초기화
    {
        currentHp = maxHp;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
