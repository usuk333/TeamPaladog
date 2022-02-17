using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuturePlayer : MonoBehaviour
{
    private Boss_TimeJudge boss_TimeJudge;
    private Player player;
    [SerializeField] private CastingObject castingObject;
    private void Awake()    
    {
        player = FindObjectOfType<Player>();
        boss_TimeJudge = GetComponentInParent<Boss_TimeJudge>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == castingObject.PlayerTag)
        {
            if (boss_TimeJudge.isBombMoveFutuer)
            {
                return;
            }
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
                boss_TimeJudge.isBombMoveFutuer = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == castingObject.PlayerTag)
        {
            if (boss_TimeJudge.isBombMoveFutuer)
            {
                return;
            }
            player.isCast = false;
        }
    }
}
