using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKing : MonoBehaviour //보스캐릭터 고블린왕 특수공격 스크립트
{
    private int currentAttackCount = 0;
    private List<GameObject> collisions = new List<GameObject>();
    [Header("충격파 발생을 위한 공격 횟수")]
    [SerializeField] private int attackCount;
    [SerializeField] private float damage;
    public int AttackCount { get => attackCount; }
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }

    public void AttackShockWave()
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            if(collisions[i] != null)
            {
                if (collisions[i].GetComponent<Unit>())
                {
                    collisions[i].GetComponent<Unit>().DecreaseHp(damage);
                }
                else if (collisions[i].GetComponent<Player>())
                {
                    collisions[i].GetComponent<Player>().DecreaseHp(damage);
                }
            }
        }
        collisions.Clear();
        Debug.Log("고블린 왕 충격파");
    }
}
