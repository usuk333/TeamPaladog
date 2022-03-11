using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjectilePool : MonoBehaviour
{
    private static UnitProjectilePool instance;
    [SerializeField] private GameObject[] projectiles;
    private Queue<GameObject> arrowQueue = new Queue<GameObject>();
    private Queue<GameObject> fireBallQueue = new Queue<GameObject>();
    private Queue<GameObject> bulletQueue = new Queue<GameObject>();
    public static UnitProjectilePool Instance { get => instance; }
    public Queue<GameObject> ArrowQueue { get => arrowQueue; }
    public Queue<GameObject> FireBallQueue { get => fireBallQueue; }
    public Queue<GameObject> BulletQueue { get => bulletQueue; }

    public static GameObject GetProjectile(int i) //매개변수 i에 따른 투사체 오브젝트를 반환해줌
    {
        switch (i)
        {
            case 0:
                return DequeueProjectile(instance.arrowQueue, i);
            case 1:
                return DequeueProjectile(instance.bulletQueue, i);
            case 2:
                return DequeueProjectile(instance.fireBallQueue, i);
            default:
                Debug.Assert(false);
                return null;
        }
    }
    public static void ReturnProjectile(GameObject obj, Queue<GameObject> queue)
    {
        obj.SetActive(false);
        obj.transform.SetParent(instance.transform);
        queue.Enqueue(obj);
    }
    private static GameObject DequeueProjectile(Queue<GameObject> queue, int i) // 투사체 큐에서 투사체를 디큐하고 해당 투사체 반환
    {
        if (queue.Count > 0)
        {
            var obj = queue.Dequeue();
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewProjectile(i);
            return newObj;
        }
    }
    private void InitializeProjectile() //투사체 초기화. 각 투사체 큐에 생성된 투사체를 인큐. 맨 처음에 호출
    {
        for (int i = 0; i < 5; i++)
        {
            arrowQueue.Enqueue(CreateNewProjectile(0));
            bulletQueue.Enqueue(CreateNewProjectile(1));
            fireBallQueue.Enqueue(CreateNewProjectile(2));
        }
    }
    private GameObject CreateNewProjectile(int i) //투사체 오브젝트 생성
    {
        GameObject obj = Instantiate(projectiles[i]);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    private void Awake()
    {
        instance = this;
        InitializeProjectile();
    }
}
