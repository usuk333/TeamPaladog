using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private CastingObject castingObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == castingObject.PlayerTag)
        {

        }
    }
}
