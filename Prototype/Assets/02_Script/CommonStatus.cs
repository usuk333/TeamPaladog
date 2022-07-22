using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommonStatus
{
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackDamage;
    [SerializeField] private float currentAttackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private bool isInvincibility;
    [SerializeField] private float shield;
    [SerializeField] private float knockBackSpeed;
    [SerializeField] private float currentShield;

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float CurrentAttackDamage { get => currentAttackDamage; set => currentAttackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool IsInvincibility { get => isInvincibility; set => isInvincibility = value; }
    public float CurrentMoveSpeed { get => currentMoveSpeed; set => currentMoveSpeed = value; }
    public float Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
            currentShield = shield;
        }
    }
    public float KnockBackSpeed { get => knockBackSpeed; set => knockBackSpeed = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float CurrentShield { get => currentShield; }

    public void DecreaseHp(float damage)
    {
        if (isInvincibility)
        {
            return;
        }
        if(currentShield > 0)
        {
            currentShield -= damage;
            return;
        }
        else
        {
            shield = 0;
        }
        currentHp -= damage;
    }
    public void IncreaseHp(float value)
    {
        currentHp += value;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
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
    public void SetAttackDamage(float value)
    {
        currentAttackDamage = value;
    }
}
