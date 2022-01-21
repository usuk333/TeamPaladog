using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jealousy : MonoBehaviour
{
    private List<GameObject> collisions = new List<GameObject>();
    private List<Unit> donutCollisions = new List<Unit>();
    private Boss boss;
    private GameObject donut;
    private bool isDonut;
    [Header("도넛 데미지")]
    [SerializeField] private float donutDamage;
    [Header("도넛 생성 주기(초단위)")]
    [SerializeField] private float donutSecond;
    [Header("도넛 생성 후 피해를 주기까지의 시간")]
    [SerializeField] private float donutAttackSecond;
    [Header("충격파 데미지")]
    [SerializeField] private float damage;
    [Header("충격파 생성 후 직접 피해를 주기까지 시간")]
    [SerializeField] private float crushSecond;
    [SerializeField] private int crushCount;
    [SerializeField] private GameObject donutPrefab;
    [SerializeField] private GameObject crushPrefab;
    [SerializeField] private List<GameObject> crushs = new List<GameObject>();

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public List<Unit> DonutCollisions { get => donutCollisions; set => donutCollisions = value; }

    public void ActiveCrush()
    {
        StartCoroutine(Co_ActiveCrush());
    }
    private void InitCrush()
    {
        for (int i = 1; i < crushCount+1; i++)
        {
            var obj = Instantiate(crushPrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3(-i - (i * 0.1f), 0);
            obj.SetActive(false);
            crushs.Add(obj);
        }
    }
    private void InitDonut()
    {
        donut = Instantiate(donutPrefab, transform.position, Quaternion.identity);
        donut.SetActive(false);
        donut.transform.SetParent(FindObjectOfType<Player>().transform);
        donut.transform.localPosition = new Vector3(0, -1.1f);
    }
    private void ActiveCrushInOrder(int index)
    {
        for (int i = index; i < crushs.Count; i+=2)
        {
            crushs[i].SetActive(true);
            crushs[i].transform.GetChild(0).GetComponent<Transform>().DOScale(1, crushSecond);
        }
    }
    private void ResetCrushInOrder(int index)
    {
        for (int i = index; i < crushs.Count; i += 2)
        {
            crushs[i].SetActive(false);
            crushs[i].transform.GetChild(0).GetComponent<Transform>().localScale = new Vector3(0, 1);
        }
    }
    private void AttackCrush()
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
    private void AttackDonut()
    {
        foreach (var item in donutCollisions)
        {
            item.DecreaseHp(donutDamage);
        }
    }
    private IEnumerator Co_ActiveDonut()
    {
        isDonut = true;
        donut.SetActive(true);
        donut.transform.GetChild(0).GetComponent<Transform>().DOScale(1, donutAttackSecond);
        yield return new WaitForSeconds(donutAttackSecond + 0.1f);
        AttackDonut();
        donut.SetActive(false);
        donut.transform.GetChild(0).GetComponent<Transform>().localScale = new Vector3(0, 0);
        isDonut = false;
    }
    private IEnumerator Co_ActiveCrush()
    {
        yield return new WaitForSeconds(1f);
        ActiveCrushInOrder(0);
        yield return new WaitForSeconds(crushSecond + 0.1f);
        AttackCrush();
        ResetCrushInOrder(0);
        ActiveCrushInOrder(1);
        yield return new WaitForSeconds(crushSecond + 0.1f);
        AttackCrush();
        ResetCrushInOrder(1);
        collisions.Clear();
        boss.BossState = EUnitState.Battle;
    }
    private IEnumerator Co_Donut()
    {
        float time = 0;
        while (true)
        {
            Debug.Log(time);
            if (!isDonut)
            {
                time += Time.deltaTime;
                if (time >= donutSecond)
                {
                    StartCoroutine(Co_ActiveDonut());
                    time = 0;
                }
            }
            yield return null;
        }
    }
    private void Awake()
    {
        InitCrush();
        InitDonut();
        boss = GetComponent<Boss>();
        StartCoroutine(Co_Donut());
    }

}
