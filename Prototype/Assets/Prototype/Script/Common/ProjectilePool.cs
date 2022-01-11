using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour //화살, 마법구체를 풀링으로 관리하는 스크립트
{
    private static ProjectilePool instance;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private Queue<FireBall> poolingFireBalls = new Queue<FireBall>();
    [SerializeField] private Queue<Arrow> poolingArrows = new Queue<Arrow>();
    public static Arrow GetArrow()
    {
        if (instance.poolingArrows.Count > 0)
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
            Debug.Log("풀에서 꺼낸 파이어볼");
            var obj = instance.poolingFireBalls.Dequeue();
            return obj;
        }
        else
        {
            Debug.Log("새로 만든 파이어볼");
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
    private void Awake()
    {
        instance = this;
        InitializePool(1);
    }
}
