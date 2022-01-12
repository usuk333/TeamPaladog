using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rage : MonoBehaviour
{
    private Player player;
    private Boss boss;
    private GameObject pattern;
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
        pattern.SetActive(true);
        range.DOScaleX(1f, 3f);
        yield return new WaitForSeconds(3.1f);
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<Unit>().DecreaseHp(damage);
        }
        player.DecreaseHp(damage);
        boss.BossState = EUnitState.Battle;
        ResetSetting();
    }
    private void UpdateUnits()
    {
        units = InGameManager.Instance.UnitList;
    }
    private void ResetSetting()
    {
        pattern.SetActive(false);
        units.Clear();
        range.DOScaleX(0f, 0.1f);

    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        boss = GetComponent<Boss>();
        pattern = transform.Find("Pattern").gameObject;
    }
}
