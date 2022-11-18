using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InsideFallingObj : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private CastingObject castingObject;
    private SpriteRenderer sprite;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        castingObject = GetComponent<CastingObject>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if(castingObject != null)
        {
            StartCoroutine(Co_FadeAnim());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            boxCollider.isTrigger = true;
            transform.gameObject.layer = 0;
        }
    }
    private IEnumerator Co_FadeAnim()
    {
        yield return new WaitUntil(() => castingObject.CastFinish);
        sprite.DOFade(0, 1f);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}

