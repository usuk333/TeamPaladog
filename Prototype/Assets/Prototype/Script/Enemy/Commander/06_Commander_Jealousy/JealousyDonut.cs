using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JealousyDonut : MonoBehaviour
{
    private Jealousy jealousy;
    private void Awake()
    {
        jealousy = FindObjectOfType<Jealousy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT")
        {
            if (!jealousy.DonutCollisions.Contains(collision.GetComponent<Unit>()))
            {
                jealousy.DonutCollisions.Add(collision.GetComponent<Unit>());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT")
        {
            if (jealousy.DonutCollisions.Contains(collision.GetComponent<Unit>()))
            {
                jealousy.DonutCollisions.Remove(collision.GetComponent<Unit>());
            }
        }
    }
}
