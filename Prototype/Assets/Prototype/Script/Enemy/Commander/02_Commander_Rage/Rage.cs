using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rage : MonoBehaviour
{
    private Player player;
    private Boss boss;
    private GameObject pattern;
    private BoxCollider2D collider;
    private List<GameObject> units = new List<GameObject>();
    [Header("패턴 활성화 후 돌진까지의 시간")]
    [SerializeField] private float patternSecond;
    [Header("끝에서 끝으로 돌진하는데 걸릴 시간")]
    [SerializeField] private float rushSecond;
    [SerializeField] private float damage;
    [SerializeField] private Transform range;
    public void AttackAllUnit()
    {
        StartCoroutine(Co_AttackAllUnit());
    }
    private IEnumerator Co_AttackAllUnit()
    {
        UpdateUnits();
        StopAllUnit();
        pattern.SetActive(true);
        range.DOScaleX(1f, patternSecond);
        yield return new WaitForSeconds(patternSecond + 0.1f);
        collider.enabled = false;
        AttackRage(-8.5f);
        yield return new WaitForSeconds(rushSecond + 0.1f);
        range.DOScaleX(1f, patternSecond);
        yield return new WaitForSeconds(patternSecond + 0.1f);
        AttackRage(8.5f);
        pattern.SetActive(false);
        yield return new WaitForSeconds(rushSecond + 0.1f);
        collider.enabled = true;
        boss.BossState = EUnitState.Battle; 
    }
    private void AttackRage(float posX)
    {
        transform.DOMoveX(posX, rushSecond);
        AttackAll();
        range.localScale = new Vector3(0, 1, 1);
    }
    private void AttackAll()
    {
        foreach (var item in units)
        {
            item.GetComponent<Unit>().DecreaseHp(damage);
        }
        player.DecreaseHp(damage);
    }
    private void StopAllUnit()
    {
        foreach (var item in units)
        {
            item.GetComponent<Unit>().Stun(patternSecond * 3.5f);
        }
    }
    private void UpdateUnits()
    {
        units = InGameManager.Instance.UnitList;
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        boss = GetComponent<Boss>();
        pattern = transform.Find("Pattern").gameObject;
        collider = GetComponent<BoxCollider2D>();
    }
}
