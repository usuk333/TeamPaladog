using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRange : MonoBehaviour
{
    private BomberMan bomberMan;
    private void Awake()
    {
        bomberMan = GetComponentInParent<BomberMan>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (bomberMan.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            bomberMan.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (bomberMan.Collisions.Contains(collision.gameObject))
            {
                bomberMan.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
