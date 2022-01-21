using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Avarice : MonoBehaviour
{
    private int currentPatternCount = 0;
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    [Header("패턴 반복 횟수")]
    [SerializeField] private int patternCount = 3;
    [Header("패턴 생성 후 독 폭발까지의 시간")]
    [SerializeField] private float patternSecond;
    [Header("독 폭발 간격")]
    [SerializeField] private int posXMin = 2;
    [SerializeField] private int posXMax = 3;
    [Header("독 도트딜 반복 횟수")]
    [SerializeField] private int dotCount;
    [Header("독 도트딜 반복 시간")]
    [SerializeField] private float dotSecond;
    [Header("독 폭발 데미지")]
    [SerializeField] private float explosionDamage;
    [Header("독 도트 데미지")]
    [SerializeField] private float poisonDamage;
    [Header("생성할 독 폭발 수")]
    [SerializeField] private GameObject[] poisons;
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    // Start is called before the first frame update
    public void ActivePoison()
    {
        StartCoroutine(Co_ActivePoison());
    }
    private void AttackPoison()
    {
        foreach (var item in collisions)
        {
            if (item.GetComponent<Unit>())
            {
                item.GetComponent<Unit>().DecreaseHp(explosionDamage);
                item.GetComponent<Unit>().DecreaseHpDot(dotCount, poisonDamage, dotSecond);
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().DecreaseHp(explosionDamage);
                item.GetComponent<Player>().DecreaseHpDot(dotCount, poisonDamage, dotSecond);
            }
        }
        ResetPoison();
        if (++currentPatternCount >= patternCount)
        {
            currentPatternCount = 0;
            boss.BossState = EUnitState.Battle;
        }
        else
        {
            ActivePoison();
        }
    }
    private void ResetPoison()
    {
        foreach (var item in poisons)
        {
            item.transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            item.SetActive(false);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private IEnumerator Co_ActivePoison()
    {
        yield return new WaitForSeconds(1f);
        boss.BossState = EUnitState.Wait;
        foreach (var item in poisons)
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
        AttackPoison();
    }
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
}
