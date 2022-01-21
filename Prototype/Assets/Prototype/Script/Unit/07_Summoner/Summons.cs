using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summons : MonoBehaviour //��ȯ���� ��ȯ�� ��� ��ũ��Ʈ
{
    private enum ESummonsKind
    {
        Dot,
        Increase,
        Both
    }
    private Summoner summoner;
    private SpriteRenderer sprite;
    private bool isBoom = false;
    private float damage;
    [SerializeField] private ESummonsKind summonsKind;
    [Header("��ȯ�� �̵��ӵ�")]
    [SerializeField] private float moveSpeed;
    [Header("�޴� ���� ���� ���ӽð�")]
    [SerializeField] private float duration;
    [Header("�޴� ���� ���� ������ų ���ط�('��ȯ�� ���� ���ݷ�/�ش� ����'�� �����")]
    [SerializeField] private float increaseValue;
    [Header("��Ʈ�� �ݺ� Ƚ��")]
    [SerializeField] private int dotCount;
    [Header("��Ʈ�� �ݺ� �ð� ����")]
    [SerializeField] private float dotSecond;
    [Header("��Ʈ ������('��ȯ�� ���� ���ݷ�/�ش� ����'�� �����")]
    [SerializeField] private float dotDamage;

    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private BoxCollider2D effectRange;
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    private void Invoke_IncreaseDamage()
    {
        foreach (var enemy in collisions)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<Enemy>())
                {
                    enemy.GetComponent<Enemy>().IncreaseDamage(damage / increaseValue, duration);
                }
                else if (enemy.GetComponent<Boss>())
                {
                    enemy.GetComponent<Boss>().IncreaseDamage(damage / increaseValue, duration);
                }
            }
        }
        if (summonsKind != ESummonsKind.Both)
        {
            Destroy(this.gameObject); //���� ������Ʈ Ǯ������ ��ȯ
        }
    }
    private void Invoke_AttackDot()
    {
        foreach (var enemy in collisions)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<Enemy>())
                {
                    enemy.GetComponent<Enemy>().DecreaseHpDot(dotCount, damage / dotDamage, dotSecond);
                }
                else if (enemy.GetComponent<Boss>())
                {
                    enemy.GetComponent<Boss>().DecreaseHpDot(dotCount, damage / dotDamage, dotSecond);
                }
            }
        }
        Destroy(this.gameObject);
    }
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
                        Invoke("Invoke_AttackDot",0.5f);
                        break;
                    case ESummonsKind.Increase:
                        Invoke("Invoke_IncreaseDamage", 0.5f);
                        break;
                    case ESummonsKind.Both:
                        Invoke("Invoke_AttackDot", 0.5f);
                        Invoke("Invoke_IncreaseDamage", 0.5f);
                        break;
                    default:
                        break;
                }             
            }
        }
    }
}
