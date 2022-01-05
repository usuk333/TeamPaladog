using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject[] projectiles;
    private static ProjectilePool instance;

   [SerializeField] private List<GameObject> poolingProjectiles = new List<GameObject>();
    [SerializeField] private Queue<FireBall> poolingFireBalls = new Queue<FireBall>();
    [SerializeField] private Queue<Arrow> poolingArrows = new Queue<Arrow>();
    private Queue<FireBall> poolingEnemyFireBalls = new Queue<FireBall>();
    public static ProjectilePool Instance { get => instance; }

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        InitializePool(1);
    }
    private void InitializePool(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingFireBalls.Enqueue(CreateNewFireBall());
            poolingArrows.Enqueue(CreateNewArrow());
        }
    }
    private Arrow CreateNewArrow()
    {
        Arrow obj = Instantiate(projectiles[0]).GetComponent<Arrow>();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    private FireBall CreateNewFireBall()
    {
        FireBall obj = Instantiate(projectiles[1]).GetComponent<FireBall>();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    public static Arrow GetArrow()
    {
        if(instance.poolingArrows.Count > 0)
        {
            var obj = instance.poolingArrows.Dequeue();
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewArrow();
            return newObj;
        }
    }
    public static FireBall GetFireBall()
    {
        if (instance.poolingFireBalls.Count > 0)
        {
            Debug.Log("Ǯ���� ���� ���̾");
            var obj = instance.poolingFireBalls.Dequeue();
            return obj;
        }
        else
        {
            Debug.Log("���� ���� ���̾");
            var newObj = instance.CreateNewFireBall();
            return newObj;
        }
    }
    public static void ReturnArrow(Arrow arrow)
    {
        arrow.gameObject.SetActive(false);
        arrow.transform.SetParent(instance.transform);
        instance.poolingArrows.Enqueue(arrow);
    }
    public static void ReturnFireBall(FireBall fireBall)
    {
        fireBall.Collisions.Clear();
        fireBall.gameObject.SetActive(false);
        fireBall.transform.SetParent(instance.transform);
        instance.poolingFireBalls.Enqueue(fireBall);
    }
}
