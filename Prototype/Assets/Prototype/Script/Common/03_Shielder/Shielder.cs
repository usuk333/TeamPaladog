using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : MonoBehaviour //방패병의 스턴 기능을 위한 공격횟수 체크 스크립트
{
    private int attackCount = 0;

    public int AttackCount { get => attackCount; set => attackCount = value; }
}
