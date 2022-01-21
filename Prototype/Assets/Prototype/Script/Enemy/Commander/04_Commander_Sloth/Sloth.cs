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
    [Header("치명타 공격을 위한 공격 횟수")]
    [SerializeField] private int attackCount = 0;
    [Header("치명타 데미지(공격력에 해당 수치 곱으로 적용")]
    [SerializeField] private float critical = 0;
    [Header("가시 생성 간격")]
    [SerializeField] private int posXMin = 2;
    [SerializeField] private int posXMax = 3;
    [Header("생성할 가시 수")]
    [SerializeField] private GameObject[] thorns;
    [Header("패턴 반복 횟수")]
    [SerializeField] private int patternCount = 3;
    [Header("패턴 생성 후 직접 피해를 주기까지 시간")]
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
