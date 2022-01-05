using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "UNIT" || collision.tag== "PLAYER")
        {
            if(GetComponentInParent<MaigcTool>().Collisions.Contains(collision.gameObject))
            {
                return;
            }
            GetComponentInParent<MaigcTool>().Collisions.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UNIT" || collision.tag == "PLAYER")
        {
            if (GetComponentInParent<MaigcTool>().Collisions.Contains(collision.gameObject))
            {
                GetComponentInParent<MaigcTool>().Collisions.Remove(collision.gameObject);
            }
        }
    }
}
