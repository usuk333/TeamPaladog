using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommonStatus
{
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isInvincibility;

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool IsInvincibility { get => isInvincibility; set => isInvincibility = value; }

    public void DecreaseHp(float damage)
    {
        if (isInvincibility)
        {
            return;
        }
        if((CurrentHp -= damage) <= 0)
        {
            //사망 애니메이션
        }        
    }
}
