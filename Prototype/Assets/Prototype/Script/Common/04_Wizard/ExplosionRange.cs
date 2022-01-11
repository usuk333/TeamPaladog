using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRange : MonoBehaviour //마법사의 마법 구체의 폭발 범위 내 적들을 추려내는 스크립트
{
    private FireBall fireBall;
    private void Awake()
    {
        fireBall = GetComponentInParent<FireBall>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(fireBall.Parent == EParent.Unit)
        {
            if (collision.tag == "ENEMY" || collision.tag == "BOSS")
            {
                if (fireBall.Collisions.Contains(collision.gameObject))
                {
                    return;
                }
                fireBall.Collisions.Insert(1, collision.gameObject);
            }
        }
        else
        {
            if (collision.tag == "UNIT" || collision.tag == "PLAYER")
            {
                if (fireBall.Collisions.Contains(collision.gameObject))
                {
                    return;
                }
                fireBall.Collisions.Insert(1, collision.gameObject);
            }
        }     
    }
}
