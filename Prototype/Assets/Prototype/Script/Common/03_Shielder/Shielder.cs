using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : MonoBehaviour //���к��� ���� ����� ���� ����Ƚ�� üũ ��ũ��Ʈ
{
    private int currentAttackCount = 0;
    [Header("���� ���� Ƚ��")]
    [SerializeField] private int attackCount;
    [Header("���� ���� �ð�")]
    [SerializeField] private float stunSecond;
    public int AttackCount { get => attackCount; }
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }
    public float StunSecond { get => stunSecond; }
}
