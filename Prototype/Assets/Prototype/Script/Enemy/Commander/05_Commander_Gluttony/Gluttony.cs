using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gluttony : MonoBehaviour
{
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    [Header("패턴 활성화 후 직접 피해를 주기까지 시간")]
    [SerializeField] private float patternSecond;
    [Header("잡아먹기 패턴 발동 체력(0~1 사이의 값으로)")]
    [SerializeField] private float gluttonyHealPattern;
    [Header("잡아먹기 패턴 발동 체력 감소식(뺄셈으로 적용)")]
    [SerializeField] private float gluttonyHealPatternReduction;
    [SerializeField] private float damage;
    [SerializeField] private GameObject eater;
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }

    public void AttackGluttony()
    {
        if (boss.CurrentHp < boss.MaxHp * gluttonyHealPattern)
        {
            int index = Random.Range(0, InGameManager.Instance.UnitList.Count);
            float damage = InGameManager.Instance.UnitList[index].GetComponent<Unit>().CurrentHp;
            InGameManager.Instance.UnitList[index].GetComponent<Unit>().DecreaseHp(damage);
            boss.IncreaseHp(damage);
            gluttonyHealPattern -= gluttonyHealPatternReduction;
        }
    }
    public void ActiveEater()
    {
        StartCoroutine(Co_ActiveEater());
    }
    private void AttackEater()
    {
        foreach (var item in collisions)
        {
            if (item.GetComponent<Unit>())
            {
                var unit = item.GetComponent<Unit>();
                unit.DecreaseHp(damage);
                if(unit.CurrentHp <= 0)
                {
                    boss.IncreaseHp(unit.MaxHp);
                }
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().DecreaseHp(damage);
            }
        }
        ResetEater();
        boss.BossState = EUnitState.Battle;
    }
    private void ResetEater()
    {
        eater.transform.GetChild(0).transform.localScale = Vector2.zero;
        eater.SetActive(false);
        collisions.Clear();
    }
    private IEnumerator Co_ActiveEater()
    {
        yield return new WaitForSeconds(1f);
        eater.SetActive(true);
        Vector3 rand = new Vector3(Random.Range(-8.5f, transform.position.x), transform.position.y);
        eater.transform.position = rand;
        eater.transform.GetChild(0).transform.DOScale(1, patternSecond);
        yield return new WaitForSeconds(patternSecond + 0.1f);
        AttackEater();
    }
    private void Awake()
    {
        boss = GetComponent<Boss>();
    }
}
