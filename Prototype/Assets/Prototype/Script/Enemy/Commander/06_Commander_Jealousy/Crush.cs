using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour
{
    private Jealousy jealousy;
    private void Start()
    {
        jealousy = GetComponentInParent<Jealousy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (!jealousy.Collisions.Contains(collision.gameObject))
            {
                jealousy.Collisions.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (jealousy.Collisions.Contains(collision.gameObject))
            {
                jealousy.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
