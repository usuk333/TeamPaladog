using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsidePortal : MonoBehaviour
{
    [SerializeField] private float castTime;
    private Inside inside;
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        inside = GetComponentInParent<Inside>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        boxCollider.enabled = true;
    }
    private void OnDisable()
    {
        boxCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.Casting(castTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER")
        {
            if (InGameManager.Instance.Player.isCastFinish)
            {
                inside.MoveToInside();
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
}
