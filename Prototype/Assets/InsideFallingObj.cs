using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideFallingObj : MonoBehaviour
{
    private Inside inside;
    [SerializeField] private float castTime;
    private bool isCast;
    private bool isCastReady;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        inside = GetComponentInParent<Inside>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            boxCollider.isTrigger = true;
            isCastReady = true;
            transform.gameObject.layer = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCastReady)
        {
            return;
        }
        if(collision.tag == "PLAYER" && !isCast)
        {
            InGameManager.Instance.Player.Casting(castTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isCastReady)
        {
            return;
        }
        if (collision.tag == "PLAYER")
        {
            if (InGameManager.Instance.Player.isCastFinish)
            {
                isCast = true;
                if (!inside.CastedFallingObjList.Contains(this))
                {
                    inside.CastedFallingObjList.Add(this);
                }
                InGameManager.Instance.Player.isCastFinish = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.isCast = false;
        }
    }
    public void SetDefault()
    {
        rigidbody.constraints = RigidbodyConstraints2D.None;
        boxCollider.isTrigger = false;
        isCastReady = false;
        isCast = false;
        transform.gameObject.layer = 8;
    }
}

