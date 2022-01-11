using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour //����ĳ���� ������� ����� ���� ���� ������ �߷��� ��ũ��Ʈ
{
    private GoblinKing goblinKing;
    private void Awake()
    {
        goblinKing = GetComponentInParent<GoblinKing>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (goblinKing.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            goblinKing.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (goblinKing.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            goblinKing.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (goblinKing.Collisions.Contains(collision.gameObject))
            {
                goblinKing.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
