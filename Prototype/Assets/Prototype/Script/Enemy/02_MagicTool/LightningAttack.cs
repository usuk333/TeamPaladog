using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour //����ĳ���� ������ ���������� ���ϰ��� ���� ���� ������ �߷��� ��ũ��Ʈ
{
    private MagicTool magicTool;
    private void Awake()
    {
        magicTool = GetComponentInParent<MagicTool>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "UNIT" || collision.tag== "PLAYER")
        {
            if(magicTool.Collisions.Contains(collision.gameObject))
            {
                return;
            }
            magicTool.Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (magicTool.Collisions.Contains(collision.gameObject))
            {
                magicTool.Collisions.Remove(collision.gameObject);
            }
        }
    }
}
