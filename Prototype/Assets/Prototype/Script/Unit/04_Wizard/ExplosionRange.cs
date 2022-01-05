using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRange : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.parent.GetComponent<FireBall>().Parent == EParent.Unit)
        {
            if (collision.tag == "ENEMY" || collision.tag == "BOSS")
            {
                if (transform.parent.GetComponent<FireBall>().Collisions.Contains(collision.gameObject))
                {
                    return;
                }
                transform.parent.GetComponent<FireBall>().Collisions.Insert(1, collision.gameObject);
            }
        }
        else
        {
            if (collision.tag == "UNIT" || collision.tag == "PLAYER")
            {
                if (transform.parent.GetComponent<FireBall>().Collisions.Contains(collision.gameObject))
                {
                    return;
                }
                transform.parent.GetComponent<FireBall>().Collisions.Insert(1, collision.gameObject);
                //transform.parent.GetComponent<FireBall>().Collisions.Insert(1, collision.gameObject);
            }
        }     
    }
}
