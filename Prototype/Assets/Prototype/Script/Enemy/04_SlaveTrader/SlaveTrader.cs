using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveTrader : MonoBehaviour
{
    private int currentAttackCount;
    private List<GameObject> collisions = new List<GameObject>();
    [Header("���� Ÿ���� ���� ���� Ƚ��")]
    [SerializeField] private int attackCount;
    [SerializeField] private float damage;
    public int AttackCount { get => attackCount; }
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }

    public void Attack()
    {
        foreach (var item in collisions)
        {
            if (item.GetComponent<Unit>())
            {
                item.GetComponent<Unit>().DecreaseHp(damage);
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().DecreaseHp(damage);
            }
        }
    }
}
