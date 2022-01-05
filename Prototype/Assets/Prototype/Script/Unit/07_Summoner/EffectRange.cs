using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ENEMY" || collision.tag == "BOSS")
        {
            transform.parent.GetComponent<Summons>().Collisions.Add(collision.gameObject);

        }
    }
}
