using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
   /* [SerializeField] private float castTime;
    private Mushroom mushroom;
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        mushroom = GetComponentInParent<Mushroom>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        castTime = mushroom.mushroomStatus.fifthPatternCastingTime;
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
        if(collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.Casting(false, castTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER")
        {
            if (InGameManager.Instance.Player.isCastFinish)
            {
                mushroom.SetSlimeVincible();
            }
            if (InGameManager.Instance.Player.test)
            {
                Debug.Log(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.Casting(true);
        }
    }*/
}
