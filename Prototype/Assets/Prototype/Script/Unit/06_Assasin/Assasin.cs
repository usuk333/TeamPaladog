using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assasin : MonoBehaviour
{
    private SpriteRenderer sprite;

    private BoxCollider2D boxCollider;
    [SerializeField] private BoxCollider2D attackDist;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        attackDist = transform.GetChild(0).GetComponent<BoxCollider2D>();        
        sprite = GetComponent<SpriteRenderer>();
    }
    public void UnStealth()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        boxCollider.enabled = true;
        attackDist.enabled = true;
    }
    public void Hide()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        boxCollider.enabled = false;
        attackDist.enabled = false;
    }
}
