using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingObject : MonoBehaviour
{
    private Gargoyle gargoyle;
    private void Awake()
    {
        gargoyle = GetComponentInParent<Gargoyle>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER")
        {

        }
    }
}
