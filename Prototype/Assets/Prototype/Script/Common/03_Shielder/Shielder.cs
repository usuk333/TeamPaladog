using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : MonoBehaviour //방패병의 스턴 기능을 위한 공격횟수 체크 스크립트
{
    private int currentAttackCount = 0;
    [Header("스턴 공격 횟수")]
    [SerializeField] private int attackCount;
    [Header("스턴 지속 시간")]
    [SerializeField] private float stunSecond;
    public int AttackCount { get => attackCount; }
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }
    public float StunSecond { get => stunSecond; }
}
