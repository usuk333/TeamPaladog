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
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private bool isInvincibility;
    [SerializeField] private float shield;
    [SerializeField] private float knockBackSpeed;

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool IsInvincibility { get => isInvincibility; set => isInvincibility = value; }
    public float CurrentMoveSpeed { get => currentMoveSpeed; set => currentMoveSpeed = value; }
    public float Shield { get => shield; set => shield = value; }
    public float KnockBackSpeed { get => knockBackSpeed; set => knockBackSpeed = value; }

    public void DecreaseHp(float damage)
    {
        if (isInvincibility)
        {
            return;
        }
        if(shield > 0)
        {
            shield -= damage;
            return;
        }
        if ((CurrentHp -= damage) <= 0)
        {
            //사망 애니메이션
        }
    }
    public void SlowDown(bool isReturn, float percentage = 0)
    {
        if (isReturn)
        {
            currentMoveSpeed = MoveSpeed;
            return;
        }
        currentMoveSpeed = moveSpeed - moveSpeed * percentage / 100;
    }
}
