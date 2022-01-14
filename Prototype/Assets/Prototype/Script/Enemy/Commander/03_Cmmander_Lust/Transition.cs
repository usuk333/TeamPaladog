using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    private Lust lust;
    private void Awake()
    {
        lust = GetComponentInParent<Lust>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (lust.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            lust.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (lust.Collisions.Contains(collision.gameObject))
            {
                lust.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
