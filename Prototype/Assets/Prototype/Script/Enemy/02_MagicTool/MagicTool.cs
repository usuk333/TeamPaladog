using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicTool : MonoBehaviour //보스캐릭터 강력한 마법도구의 공격 기능 스크립트
{
    private int currentPatternCount = 0;
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private float damage;
    [Header("번개 생성 간격")]
    [SerializeField] private int posXMin = 2;
    [SerializeField] private int posXMax = 3;
    [Header("패턴 반복 횟수")]
    [SerializeField] private int patternCount = 3;
    [Header("범위 생성 후 번개가 떨어지기까지 시간")]
    [SerializeField] private float patternSecond;
    [Header("생성할 번개 수")]
    [SerializeField] private GameObject[] lightnings;
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public void ActiveLightning()
    {      
        StartCoroutine(Co_ActiveLightning());
    }
    private void AttackLightning()
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
        ResetLightning();
        if (++currentPatternCount >= patternCount)
        {
            currentPatternCount = 0;
            boss.BossState = EUnitState.Battle;
        }
        else
        {
            ActiveLightning();
        }
    }
    private void ResetLightning()
    {
        foreach (var item in lightnings)
        {
            item.transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            item.SetActive(false);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private IEnumerator Co_ActiveLightning()
    {
        yield return new WaitForSeconds(1f);
        foreach (var item in lightnings)
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
        AttackLightning();
    }
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
}
