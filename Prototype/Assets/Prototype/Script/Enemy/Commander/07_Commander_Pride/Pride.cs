using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pride : MonoBehaviour
{
    private bool isIn = false;
    private Boss boss;
    [Header("패턴 생성 후 직접 피해를 주기까지 시간")]
    [SerializeField] private float patternSecond;
    [SerializeField] private float damage;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private GameObject[] donuts = new GameObject[2];
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public bool IsIn { get => isIn; set => isIn = value; }
    
    public void ActiveDonut()
    {
        StartCoroutine(Co_ActiveDonut());
    }
    private void AttackDonut()
    {
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
    }
    private void ActiveDonutInOrder(int index, Vector3 pos)
    {
        donuts[index].SetActive(true);
        donuts[index].transform.position = pos;
    }
    private void DisableDonutInOrder(int index)
    {
        donuts[index].SetActive(false);
        donuts[index].transform.position = transform.position;
    }
    private IEnumerator Co_ActiveDonut()
    {
        yield return new WaitForSeconds(1f);
        Vector3 rand = new Vector3(Random.Range(1f, transform.position.x), 0);
        ActiveDonutInOrder(0, rand);
        yield return new WaitForSeconds(patternSecond + 0.1f);
        AttackDonut();
        DisableDonutInOrder(0);
        ActiveDonutInOrder(1, rand);
        yield return new WaitForSeconds(patternSecond + 0.1f);
        AttackDonut();
        DisableDonutInOrder(1);
        collisions.Clear();
        boss.BossState = EUnitState.Battle;
    }
    private void Awake()
    {
        for (int i = 0; i < donuts.Length; i++)
        {
            donuts[i] = transform.Find($"0{i + 1}_Donut").gameObject;
        }
        boss = GetComponent<Boss>();
    }
}
