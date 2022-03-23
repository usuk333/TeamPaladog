using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    [SerializeField] private float damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.DecreaseHp(damage);
        }
        if(collision.tag == "UNIT")
        {
            for (int i = 0; i < InGameManager.Instance.Units.Length; i++)
            {
                if(collision.gameObject == InGameManager.Instance.Units[i].gameObject)
                {
                    InGameManager.Instance.Units[i].CommonStatus.DecreaseHp(damage);
                }
            }
        }
    }
}
