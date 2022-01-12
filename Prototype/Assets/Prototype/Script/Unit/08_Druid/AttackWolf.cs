using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWolf : MonoBehaviour
{
    private Druid druid;
    private void Awake()
    {
        druid = GetComponentInParent<Druid>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ENEMY" || collision.tag == "BOSS")
        {
            if (!druid.Collisions.Contains(collision.gameObject))
            {
                druid.Collisions.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ENEMY" || collision.tag == "BOSS")
        {
            if (druid.Collisions.Contains(collision.gameObject))
            {
                druid.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
