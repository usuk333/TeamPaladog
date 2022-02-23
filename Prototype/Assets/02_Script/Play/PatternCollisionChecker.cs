using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatternCollisionChecker : MonoBehaviour
{
    private Boss boss;
    private BoxCollider2D boxCollider2D;
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }
    private void OnEnable()
    {
        boxCollider2D.enabled = true;
    }
    private void OnDisable()
    {
        boxCollider2D.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
             if (boss.PatternCollisions.Contains(collision.gameObject))
             {
                 return;
             }
             boss.PatternCollisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (boss.PatternCollisions.Contains(collision.gameObject))
            {
                return;
            }
            boss.PatternCollisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (!boss.PatternCollisions.Contains(collision.gameObject))
            {
                return;
            }
            boss.PatternCollisions.Remove(collision.gameObject);
        }
    }
}
