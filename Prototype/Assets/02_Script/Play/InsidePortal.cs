using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsidePortal : MonoBehaviour
{
   /* [SerializeField] private float castTime;
    private Inside inside;
    private BoxCollider2D boxCollider;
    private bool castFinish;
    private void Awake()
    {
        inside = GetComponentInParent<Inside>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        boxCollider.enabled = true;
        StartCoroutine(Co_MoveInside());
    }
    private void OnDisable()
    {
        boxCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.Casting(false, castTime);
        }
    }
    private IEnumerator Co_MoveInside()
    {
        yield return new WaitUntil(() => InGameManager.Instance.Player.isCastFinish);
        inside.MoveToInside();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.Casting(true);
        }
    }*/
}
