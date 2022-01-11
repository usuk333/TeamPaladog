using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRange : MonoBehaviour //소환수들의 범위 내 적들을 추려내는 스크립트
{
    private Summons summons;
    private void Awake()
    {
        summons = GetComponentInParent<Summons>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ENEMY" || collision.tag == "BOSS")
        {
            summons.Collisions.Add(collision.gameObject);
        }
    }
}
