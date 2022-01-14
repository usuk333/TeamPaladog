using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BomberMan : MonoBehaviour
{
    private Lust lust;
    private Enemy enemy;
    private Player player;
    [SerializeField] private GameObject target;
    private float moveSpeed;
    private BoxCollider2D bombRange;
    private Vector3 direction;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private float bombSecond;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public Lust Lust { get => lust; set => lust = value; }

    public void AttackBomb(float damage)
    {
        bombRange.enabled = true;
        StartCoroutine(Co_AttackBomb(damage));
    }
    private GameObject FindNearestUnit()
    {
        GameObject nearestUnit = InGameManager.Instance.UnitList.OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        }).FirstOrDefault();

        return nearestUnit;
    }
    private IEnumerator Co_AttackBomb(float damage)
    {
        yield return new WaitForSeconds(bombSecond);
        foreach (var item in collisions)
        {
            if (item.GetComponent<Unit>())
            {
                item.GetComponent<Unit>().DecreaseHp(damage);
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().DecreaseHp(damage);
            }
        }
        collisions.Clear();
        if(lust != null)
        {
            lust.ReturnBomberMan(this);
        }
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
        moveSpeed = enemy.MoveSpeed;
        bombRange = transform.Find("BombRange").GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        StartCoroutine(Co_InitTarget());      
    }
    private IEnumerator Co_InitTarget()
    {
        yield return new WaitForSeconds(1f);
        target = FindNearestUnit();
        if (target != null)
        {
            transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
        }
    }
    private void FixedUpdate()
    {
        if(enemy.EnemyState == EUnitState.NonCombat)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
        }
    }
    
}
