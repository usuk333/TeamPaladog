using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAttack : MonoBehaviour
{
    private SlaveTrader slaveTrader;
    private void Awake()
    {
        slaveTrader = GetComponentInParent<SlaveTrader>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (!slaveTrader.Collisions.Contains(collision.gameObject))
            {
                slaveTrader.Collisions.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (slaveTrader.Collisions.Contains(collision.gameObject))
            {
                slaveTrader.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
