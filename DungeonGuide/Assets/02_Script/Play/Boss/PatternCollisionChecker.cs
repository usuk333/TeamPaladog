using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatternCollisionChecker : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    [SerializeField] private int arrayCount;
    public bool isGargoyle;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }
    private void OnEnable()
    {
        boxCollider2D.enabled = true;
    }
    private void OnDisable()
    {
        boxCollider2D.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isGargoyle)
        {
            if(collision.tag == "BOSS")
            {
                if (InGameManager.Instance.Boss.CollisionsArray[arrayCount].Contains(collision.gameObject))
                {
                    return;
                }
                InGameManager.Instance.Boss.CollisionsArray[arrayCount].Add(collision.gameObject);
            }
            return;
        }
        if (collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (InGameManager.Instance.Boss.CollisionsArray[arrayCount].Contains(collision.gameObject))
            {
                return;
            }
            InGameManager.Instance.Boss.CollisionsArray[arrayCount].Add(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isGargoyle)
        {
            if (collision.tag == "BOSS")
            {
                if (InGameManager.Instance.Boss.CollisionsArray[arrayCount].Contains(collision.gameObject))
                {
                    return;
                }
                InGameManager.Instance.Boss.CollisionsArray[arrayCount].Add(collision.gameObject);
            }
            return;
        }
        if (collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (InGameManager.Instance.Boss.CollisionsArray[arrayCount].Contains(collision.gameObject))
            {
                return;
            }
            InGameManager.Instance.Boss.CollisionsArray[arrayCount].Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (isGargoyle)
        {
            if (collision.tag == "BOSS")
            {
                if (!InGameManager.Instance.Boss.CollisionsArray[arrayCount].Contains(collision.gameObject))
                {
                    return;
                }
                InGameManager.Instance.Boss.CollisionsArray[arrayCount].Remove(collision.gameObject);
            }
            return;
        }
        if (collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (!InGameManager.Instance.Boss.CollisionsArray[arrayCount].Contains(collision.gameObject))
            {
                return;
            }
            InGameManager.Instance.Boss.CollisionsArray[arrayCount].Remove(collision.gameObject);
        }
    }
}
