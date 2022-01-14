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
    [SerializeField] private Transform range;
    [SerializeField] private List<GameObject> units = new List<GameObject>();
    [SerializeField] private float damage;
    public void AttackAllUnit()
    {
        StartCoroutine(Co_AttackAllUnit());
    }
    private IEnumerator Co_AttackAllUnit()
    {
        UpdateUnits();
        StopAllUnit();
        pattern.SetActive(true);
        range.DOScaleX(1f, 1.5f);
        yield return new WaitForSeconds(1.6f);
        collider.enabled = false;
        AttackRage(-8.5f);
        yield return new WaitForSeconds(1f);
        range.DOScaleX(1f, 1.5f);
        yield return new WaitForSeconds(1.6f);
        AttackRage(8.5f);
        pattern.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
        boss.BossState = EUnitState.Battle; 
    }
    private void AttackRage(float posX)
    {
        transform.DOMoveX(posX, 0.5f);
        AttackAll();
        range.localScale = new Vector3(0, 1, 1);
    }
    private void AttackAll()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<Unit>().DecreaseHp(damage);
        }
        player.DecreaseHp(damage);
    }
    private void StopAllUnit()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<Unit>().Stun(5f);
        }
    }
    private void UpdateUnits()
    {
        units = InGameManager.Instance.UnitList;
    }
    private void ResetSetting()
    {
       // pattern.SetActive(false);
       // units.Clear();
        range.localScale = new Vector3(0, 1, 1);

    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        boss = GetComponent<Boss>();
        pattern = transform.Find("Pattern").gameObject;
        collider = GetComponent<BoxCollider2D>();
    }
}
