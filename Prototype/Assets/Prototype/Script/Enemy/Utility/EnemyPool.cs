using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour //�� ������ Ǯ������ �����ϴ� ��ũ��Ʈ
{
    private static EnemyPool instance;
    [SerializeField] private GameObject[] poolingEnemies;
    [SerializeField] private List<Enemy> poolingEnemyList = new List<Enemy>();
    public static Enemy GetEnemy(int index)
    {
        if (instance.poolingEnemyList.Find(x => x.Index == index))
        {
            Debug.Log("Ǯ���� ������");
            var enemy = instance.poolingEnemyList.Find(x => x.Index == index);
            instance.poolingEnemyList.Remove(enemy);
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            Debug.Log("Ǯ�� ��� ���� ����");
            var enemy = instance.CreateNewEnemy(index);
            enemy.gameObject.SetActive(true);
            return enemy;
        }
    }
    public static void ReturnEnemy(Enemy enemy)
    {
        Debug.Log("Ǯ�� ���ư�");
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
