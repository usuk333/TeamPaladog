using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveTrader : MonoBehaviour
{
    private int attackCount;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    public int AttackCount { get => attackCount; set => attackCount = value; }
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }

    public void Attack(float damage)
    {
        for (int i = 0; i < collisions.Count; i++)
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
        Debug.Log("노예상 간부 특수공격");
    }
}
