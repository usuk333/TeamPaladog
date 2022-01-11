using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour //적 유닛을 풀링으로 관리하는 스크립트
{
    private static EnemyPool instance;
    [SerializeField] private GameObject[] poolingEnemies;
    [SerializeField] private List<Enemy> poolingEnemyList = new List<Enemy>();
    public static Enemy GetEnemy(int index)
    {
        if (instance.poolingEnemyList.Find(x => x.Index == index))
        {
            Debug.Log("풀에서 꺼내옴");
            var enemy = instance.poolingEnemyList.Find(x => x.Index == index);
            instance.poolingEnemyList.Remove(enemy);
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            Debug.Log("풀에 없어서 새로 만듬");
            var enemy = instance.CreateNewEnemy(index);
            enemy.gameObject.SetActive(true);
            return enemy;
        }
    }
    public static void ReturnEnemy(Enemy enemy)
    {
        Debug.Log("풀로 돌아감");
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(instance.transform);
        instance.poolingEnemyList.Add(enemy);
    }
    private void InitializePool(int initCount)
    {
        for (int i = 0; i < poolingEnemies.Length; i++)
        {
            for (int j = 0; j < initCount; j++)
            {
                poolingEnemyList.Add(CreateNewEnemy(i));
            }
        }
    }
    private Enemy CreateNewEnemy(int index)
    {
        Enemy enemy = Instantiate(poolingEnemies[index]).GetComponent<Enemy>();
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(transform);
        return enemy;
    }
    private void Awake()
    {
        instance = this;
        InitializePool(1);
    }
}
