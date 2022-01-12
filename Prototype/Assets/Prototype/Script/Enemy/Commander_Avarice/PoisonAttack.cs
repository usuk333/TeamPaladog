using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttack : MonoBehaviour
{
    private Avarice avarice;
    private void Awake()
    {
        avarice = GetComponentInParent<Avarice>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (!avarice.Collisions.Contains(collision.gameObject))
            {
                avarice.Collisions.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (avarice.Collisions.Contains(collision.gameObject))
            {
                avarice.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
