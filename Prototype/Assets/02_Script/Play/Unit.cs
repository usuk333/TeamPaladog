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
        Dead,
    }
    private EUnitState eUnitState = EUnitState.Idle;
    [SerializeField] private Boss boss;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private EUnitType eUnitType;

    public EUnitType UnitType { get => eUnitType; }
    public void InitializeUnitStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
    }
    public void DecreaseHp(float damage) //유닛 Hp 감소 함수(공격하는 쪽에서 호출함)
    {
        currentHp -= damage;
    }
    private IEnumerator Co_UpdateState() //유닛 유한상태기계
    {
        while (true)
        {
            switch (eUnitState)
            {
                case EUnitState.Idle:
                    break;
                case EUnitState.Move:
                    break;
                case EUnitState.Attack:
                    break;
                case EUnitState.Skill:
                    break;
                case EUnitState.Dead:
                    break;
                default:
                    break;
            }
            yield return null;
        }
    }
    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
    }
}
