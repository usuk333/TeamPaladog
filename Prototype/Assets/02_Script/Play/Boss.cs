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
    [SerializeField] private float maxHp; // ���� �ִ� ü��, ���̵��� ���� �� �ε� �� �ʱ�ȭ
    [SerializeField] private float currentHp; // ���� ���� ü��. �� �ε� �� maxHp�� �ʱ�ȭ
    [SerializeField] private float attackDamage; // ���� ���ݷ�. ���̵��� ���� �� �ε� �� �ʱ�ȭ
    [SerializeField] private float attackSpeed; // ���� ���� �ӵ�. ���̵��� ���� �� �ε� �� �ʱ�ȭ
    public float MaxHp { get => maxHp; }
    public float CurrentHp { get => currentHp; }

    public void InitializeBossStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
    }
    private void AttackBasic() //���� ��Ÿ
    {
        unit.DecreaseHp(attackDamage);
    }
    private void UpdateUnitReference()
    {
        //�ΰ��ӸŴ����� ���� ����Ʈ �ε����� ���� �Ҵ�����ָ� �� �� (0,1,2,3 ������ ��Ŀ, �ٵ�, ������, ���Ÿ� ����)
    }
    private void Awake()//�ӽ÷� �ʱ�ȭ
    {
        currentHp = maxHp;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
