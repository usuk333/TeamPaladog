using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    /*private Boss_TimeJudge boss_TimeJudge;
    private Player player;
    [SerializeField] private Transform oppositePortal;
    [SerializeField] private CastingObject castingObject;
    private void TeleportOppositePortal()
    {
        boss_TimeJudge.isPresent = !boss_TimeJudge.isPresent;
        player.transform.position = oppositePortal.transform.position;
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        boss_TimeJudge = GetComponentInParent<Boss_TimeJudge>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == castingObject.PlayerTag)
        {
            player.Casting(false, castingObject.CastTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == castingObject.PlayerTag)
        {
            if (player.isCastFinish)
            {
                player.isCastFinish = false;
                TeleportOppositePortal();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == castingObject.PlayerTag)
        {
            player.Casting(true);
        }
    }*/
}
