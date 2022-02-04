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
    public void InitializeUnitStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
    }
    public void DecreaseHp(float damage) //���� Hp ���� �Լ�(�����ϴ� �ʿ��� ȣ����)
    {
        currentHp -= damage;
    }
    private IEnumerator Co_UpdateState() //���� ���ѻ��±��
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
