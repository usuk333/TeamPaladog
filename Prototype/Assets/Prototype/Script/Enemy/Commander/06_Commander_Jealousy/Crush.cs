using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour
{
    private enum EParent
    {
        Jealousy,
        Devil
    }
    private Devil devil;
    private Jealousy jealousy;
    [SerializeField] private EParent parent;
    private void Start()
    {
        if(parent == 0)
        {
            jealousy = GetComponentInParent<Jealousy>();
        }
        else
        {
            devil = GetComponentInParent<Devil>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (jealousy)
            {
                if (!jealousy.Collisions.Contains(collision.gameObject))
                {
                    jealousy.Collisions.Add(collision.gameObject);
                }
            }
            else
            {
                if (!devil.CrushCollisions.Contains(collision.gameObject))
                {
                    devil.CrushCollisions.Add(collision.gameObject);
                }
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (jealousy)
            {
                if (jealousy.Collisions.Contains(collision.gameObject))
                {
                    jealousy.Collisions.Remove(collision.gameObject);
                }
            }
            else
            {
                if (devil.CrushCollisions.Contains(collision.gameObject))
                {
                    devil.CrushCollisions.Remove(collision.gameObject);
                }
            }
        }
    }
}
