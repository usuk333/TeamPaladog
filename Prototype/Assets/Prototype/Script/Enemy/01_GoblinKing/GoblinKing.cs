using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKing : MonoBehaviour
{
    private int attackCount = 0;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private BoxCollider2D shockWaveDist;
   

    public int AttackCount { get => attackCount; set => attackCount = value; }
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public BoxCollider2D ShockWaveDist { get => shockWaveDist; set => shockWaveDist = value; }

    private void Awake()
    {
        shockWaveDist = transform.Find("ShockWaveDist").GetComponent<BoxCollider2D>();
    }
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
