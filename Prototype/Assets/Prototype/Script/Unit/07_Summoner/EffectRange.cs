using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRange : MonoBehaviour //��ȯ������ ���� �� ������ �߷����� ��ũ��Ʈ
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
