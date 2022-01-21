using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EParent
{
    Unit,
    Enemy
}

public class FireBall : MonoBehaviour //마법사의 마법 구체의 기능 스크립트
{
    private float damage;
    private SpriteRenderer sprite;
    private bool isBoom = false;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private BoxCollider2D explosionRange;
    [SerializeField] private EParent parent;
    [Header("마법구체 속도")]
    [SerializeField] private float speed;
    [Header("마법구체 스플래쉬 데미지(곱으로 적용, 0.1~0.9 사이값으로")]
    [SerializeField] private float splashDamage;
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public EParent Parent { get => parent; set => parent = value; }
    private void BoomFireBall()
    {
        if (parent == EParent.Unit)
        {
            for (int i = 0; i < collisions.Count; i++)
            {
                if (i > 2)
                {
                    break;
                }
                if (collisions[i] != null)
                {
                    if (collisions[i].GetComponent<Enemy>())
                    {
                        collisions[i].GetComponent<Enemy>().DecreaseHp(i == 0 ? damage : damage * splashDamage);
                    }
                    else if (collisions[i].GetComponent<Boss>())
                    {
                        collisions[i].GetComponent<Boss>().DecreaseHp(i == 0 ? damage : damage * splashDamage);
                    }
                }
            }
            ProjectilePool.ReturnFireBall(this);
            isBoom = false;
        }
        else
        {
            for (int i = 0; i < collisions.Count; i++)
            {
                if (i > 2)
                {
                    break;
                }
                if (collisions[i] != null)
                {
                    if (collisions[i].GetComponent<Unit>())
                    {
                        collisions[i].GetComponent<Unit>().DecreaseHp(i == 0 ? damage : damage * 0.5f);
                    }
                    else if (collisions[i].GetComponent<Player>())
                    {
                        collisions[i].GetComponent<Player>().DecreaseHp(i == 0 ? damage : damage * 0.5f);
                    }
                }
            }
            isBoom = false;
            ProjectilePool.ReturnFireBall(this);
        }
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();     
    }
    private void OnEnable()
    {
        explosionRange.enabled = false;
        if (parent == EParent.Unit)
        {
            damage = GetComponentInParent<Unit>().AttackPower;
        }
        else
        {
            damage = GetComponentInParent<Enemy>().AttackPower;
        }
        sprite.sortingLayerName = GetComponentInParent<SpriteRenderer>().sortingLayerName;
        sprite.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder;
        transform.SetParent(transform.parent.parent);
    }
    private void FixedUpdate()
    {
        if (isBoom)
        {
            return;
        }
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
        if (!isBoom)
        {
            if(parent == EParent.Unit)
            {
                if (collision.tag == "ENEMY" || collision.tag == "BOSS")
                {
                    isBoom = true;
                    collisions.Add(collision.gameObject);
                    explosionRange.enabled = true;                 
                    Invoke("BoomFireBall", 0.5f);
                }
            }
            else
            {
                if (collision.tag == "UNIT" || collision.tag == "PLAYER")
                {
                    isBoom = true;
                    collisions.Add(collision.gameObject); //첫 충돌한 애를 리스트 첫번째로
                    explosionRange.enabled = true;
                    Invoke("BoomFireBall", 0.5f);
                }
            }
        }      
    }
}
