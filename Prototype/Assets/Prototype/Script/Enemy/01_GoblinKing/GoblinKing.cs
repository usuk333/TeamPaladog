using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKing : MonoBehaviour //보스캐릭터 고블린왕 특수공격 스크립트
{
    private int attackCount = 0;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();

    public int AttackCount { get => attackCount; set => attackCount = value; }
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public void AttackShockWave(float damage)
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
