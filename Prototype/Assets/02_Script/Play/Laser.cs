using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Boss_TimeJudge boss_TimeJudge;
    private void Awake()
    {
        boss_TimeJudge = GetComponentInParent<Boss_TimeJudge>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (boss_TimeJudge.LaserCollisions.Contains(collision.gameObject))
            {
                return;
            }
            boss_TimeJudge.LaserCollisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (!boss_TimeJudge.LaserCollisions.Contains(collision.gameObject))
            {
                return;
            }
            boss_TimeJudge.LaserCollisions.Remove(collision.gameObject);
        }
    }
}
