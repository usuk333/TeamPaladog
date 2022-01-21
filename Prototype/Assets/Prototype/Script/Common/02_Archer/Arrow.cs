using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour //궁수 유닛이 사용하는 화살 오브젝트 기능 스크립트
{
    private float damage;
    private SpriteRenderer sprite;
    [SerializeField] private EParent parent;
    [Header("화살 속도")]
    [SerializeField] private float speed;
    public EParent Parent { get => parent; set => parent = value; }
    private void AttackArrow(GameObject collision)
    {
        if (parent == EParent.Unit)
        {
            if (collision.tag == "ENEMY")
            {
                collision.GetComponent<Enemy>().DecreaseHp(damage);
                ProjectilePool.ReturnArrow(this);
            }
            else if (collision.tag == "BOSS")
            {
                collision.GetComponent<Boss>().DecreaseHp(damage);
                ProjectilePool.ReturnArrow(this);
            }
        }
        else
        {
            if (collision.tag == "UNIT")
            {
                collision.GetComponent<Unit>().DecreaseHp(damage);
                ProjectilePool.ReturnArrow(this);
            }
            else if (collision.tag == "PLAYER")
            {
                collision.GetComponent<Player>().DecreaseHp(damage);
                ProjectilePool.ReturnArrow(this);
            }
        }
    }
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if (parent == EParent.Unit)
        {
            damage = GetComponentInParent<Unit>().AttackPower;
        }
        else
        {
            damage = GetComponentInParent<Enemy>().AttackPower;
        }
        sprite.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder;
        transform.SetParent(transform.parent.parent);
    }
    private void FixedUpdate()
    {
        if(parent == EParent.Unit)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackArrow(collision.gameObject);
    }
}
