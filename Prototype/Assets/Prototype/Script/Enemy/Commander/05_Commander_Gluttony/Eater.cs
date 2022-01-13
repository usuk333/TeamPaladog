using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    private Gluttony gluttony;
    private void Awake()
    {
        gluttony = GetComponentInParent<Gluttony>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (gluttony.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            gluttony.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (gluttony.Collisions.Contains(collision.gameObject))
            {
                gluttony.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
