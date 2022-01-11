using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour //보스캐릭터 강력한 마법도구의 패턴공격 범위 내의 적들을 추려낼 스크립트
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
