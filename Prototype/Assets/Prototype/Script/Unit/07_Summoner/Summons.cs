using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summons : MonoBehaviour
{
    private enum ESummonsKind
    {
        Dot,
        Increase,
        Both
    }

    [SerializeField] private ESummonsKind summonsKind;
    [SerializeField] private float value;
    [SerializeField] private float duration;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private BoxCollider2D effectRange;
    private Summoner summoner;
    private SpriteRenderer sprite;
    private bool isBoom = false;
    private float damage;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }

    private void Awake()
    {
        summoner = GetComponentInParent<Summoner>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder;
        damage = GetComponentInParent<Unit>().AttackPower;
        transform.parent = transform.parent.parent;
    }
    private void FixedUpdate()
    {
        if (isBoom)
        {
            return;
        }
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBoom)
        {
            if (collision.tag == "ENEMY" || collision.tag == "BOSS")
            {
                isBoom = true;
                effectRange.enabled = true;
                switch (summonsKind)
                {
                    case ESummonsKind.Dot:
                        StartCoroutine(Co_AttackDot());
                        break;
                    case ESummonsKind.Increase:
                        Invoke("IncreaseDamage", 0.5f);
                        break;
                    case ESummonsKind.Both:
                        StartCoroutine(Co_AttackDot());
                        Invoke("IncreaseDamage", 0.5f);
                        break;
                    default:
                        break;
                }             
            }
        }
    }
    private void IncreaseDamage()
    {
        foreach (var enemy in collisions)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<Enemy>())
                {
                    enemy.GetComponent<Enemy>().IncreaseDamage(damage / 10, duration);
                }
                else if (enemy.GetComponent<Boss>())
                {
                    enemy.GetComponent<Boss>().IncreaseDamage(damage / 10, duration);
                }
            }
        }
        if(summonsKind != ESummonsKind.Both)
        {
            Destroy(this.gameObject); //추후 오브젝트 풀링으로 전환
        }
    }
    private IEnumerator Co_AttackDot()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("폭발");
        GetComponent<BoxCollider2D>().enabled = false;
        sprite.enabled = false;
        float count = 0;
        while(count++ < duration)
        {
            foreach (var enemy in collisions)
            {
                if (enemy != null)
                {
                    if (enemy.GetComponent<Enemy>())
                    {
                        enemy.GetComponent<Enemy>().DecreaseHp(damage / 10);
                        enemy.GetComponentInChildren<HpBar>().UpdateUnitOrEnemyHpBar(false);
                    }
                    else if (enemy.GetComponent<Boss>())
                    {
                        enemy.GetComponent<Boss>().DecreaseHp(damage / 10);
                        enemy.GetComponentInChildren<BossHpBar>().UpdateBossHpUI();
                    }
                }
            }
            yield return new WaitForSeconds(value);
        }
        Destroy(this.gameObject);
    }
}
