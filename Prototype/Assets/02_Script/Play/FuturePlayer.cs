using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuturePlayer : MonoBehaviour
{
    private Player player;
    [SerializeField] private CastingObject castingObject;
    private void Awake()    
    {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == castingObject.PlayerTag)
        {
            player.Casting(castingObject.CastTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == castingObject.PlayerTag)
        {
            if (player.isCastFinish)
            {
                player.isCastFinish = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == castingObject.PlayerTag)
        {
            player.isCast = false;
        }
    }
}
