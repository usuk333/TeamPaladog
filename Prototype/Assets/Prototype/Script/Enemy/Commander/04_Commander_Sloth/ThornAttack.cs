using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornAttack : MonoBehaviour
{
    private Sloth sloth;
    private void Awake()
    {
        sloth = GetComponentInParent<Sloth>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (sloth.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            sloth.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (sloth.Collisions.Contains(collision.gameObject))
            {
                sloth.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
