using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut : MonoBehaviour
{
    private enum EKind
    {
        In,
        Middle,
        Out,
        Point
    }
    [SerializeField] private EKind eKind;
    private Pride pride;
    private void Awake()
    {
        pride = GetComponentInParent<Pride>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (eKind == EKind.In)
        {
            if(collision.tag == "PLAYER" || collision.tag == "UNIT")
            {
                pride.IsIn = true;
                if (!pride.Collisions.Contains(collision.gameObject))
                {
                    pride.Collisions.Add(collision.gameObject);
                }
            }       
        }
        if (eKind == EKind.Out)
        {
            if (collision.tag == "PLAYER" || collision.tag == "UNIT")
            {
                if (!pride.Collisions.Contains(collision.gameObject))
                {
                    pride.Collisions.Add(collision.gameObject);
                }
            }
        }
        if(eKind == EKind.Point)
        {
            if (collision.tag == "PLAYER" || collision.tag == "UNIT")
            {
                pride.IsIn = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(eKind == EKind.Middle)
        {
            if (!pride.IsIn)
            {
                if (collision.tag == "PLAYER" || collision.tag == "UNIT")
                {
                    if (pride.Collisions.Contains(collision.gameObject))
                    {
                        pride.Collisions.Remove(collision.gameObject);
                    }
                }
            }  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (eKind == EKind.Point)
        {
            if (collision.tag == "PLAYER" || collision.tag == "UNIT")
            {
                pride.IsIn = true;
                if (!pride.Collisions.Contains(collision.gameObject))
                {
                    pride.Collisions.Add(collision.gameObject);
                }
            }
        }
        if (eKind == EKind.In)
        {
            if (collision.tag == "PLAYER" || collision.tag == "UNIT")
            {
                pride.IsIn = false;
            }
        }
        if(eKind == EKind.Out)
        {
            if (collision.tag == "PLAYER" || collision.tag == "UNIT")
            {
                if (pride.Collisions.Contains(collision.gameObject))
                {
                    pride.Collisions.Remove(collision.gameObject);
                }
            }
        }
        if(eKind == EKind.Middle)
        {
            if (collision.tag == "PLAYER" || collision.tag == "UNIT")
            {
                if (!pride.Collisions.Contains(collision.gameObject))
                {
                    pride.Collisions.Add(collision.gameObject);
                }
            }
        }  
    }
}
