using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Devil devil;
    private void Awake()
    {
        devil = GetComponentInParent<Devil>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (devil.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            devil.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (devil.Collisions.Contains(collision.gameObject))
            {
                devil.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
