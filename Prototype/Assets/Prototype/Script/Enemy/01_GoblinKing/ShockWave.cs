using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (GetComponentInParent<GoblinKing>().Collisions.Contains(collision.gameObject))
            {
                return;
            }
            GetComponentInParent<GoblinKing>().Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (GetComponentInParent<GoblinKing>().Collisions.Contains(collision.gameObject))
            {
                return;
            }
            GetComponentInParent<GoblinKing>().Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (GetComponentInParent<GoblinKing>().Collisions.Contains(collision.gameObject))
            {
                GetComponentInParent<GoblinKing>().Collisions.Remove(collision.gameObject);
            }
        }
    }
}
