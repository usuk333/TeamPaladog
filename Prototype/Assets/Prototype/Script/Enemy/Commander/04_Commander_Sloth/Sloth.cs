using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sloth : MonoBehaviour
{
    private int currentAttackCount = 0;
    private int currentPatternCount = 0;
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    [Header("ġ��Ÿ ������ ���� ���� Ƚ��")]
    [SerializeField] private int attackCount = 0;
    [Header("ġ��Ÿ ������(���ݷ¿� �ش� ��ġ ������ ����")]
    [SerializeField] private float critical = 0;
    [Header("���� ���� ����")]
    [SerializeField] private int posXMin = 2;
    [SerializeField] private int posXMax = 3;
    [Header("������ ���� ��")]
    [SerializeField] private GameObject[] thorns;
    [Header("���� �ݺ� Ƚ��")]
    [SerializeField] private int patternCount = 3;
    [Header("���� ���� �� ���� ���ظ� �ֱ���� �ð�")]
    [SerializeField] private float patternSecond = 3;
    [SerializeField] private float damage;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public int AttackCount { get => attackCount; }
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }
    public float Critical { get => critical; }

    public void ActiveThorn()
    {
        StartCoroutine(Co_ActiveThorn());
    }
    private void AttackThorn()
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
        ResetThorn();
        if (++currentPatternCount >= patternCount)
        {
            currentPatternCount = 0;
            boss.BossState = EUnitState.Battle;
        }
        else
        {
            ActiveThorn();
        }
    }
    private void ResetThorn()
    {
        foreach (var item in thorns)
        {
            item.transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            item.SetActive(false);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private IEnumerator Co_ActiveThorn()
    {
        yield return new WaitForSeconds(1f);
        boss.BossState = EUnitState.Wait;
        foreach (var item in thorns)
        {
            item.SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax),
                                       Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            item.transform.position = rand;
            item.transform.GetChild(0).GetComponent<Transform>().DOScale(1, patternSecond);
            posXMin += 2;
            posXMax += 3;
        }
        yield return new WaitForSeconds(patternSecond + 0.1f);
        AttackThorn();
    }
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
}
