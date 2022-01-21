using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterMediateAsmodian : MonoBehaviour
{
    private int currentAttackCount;
    private List<GameObject> archers = new List<GameObject>();
    [Header("암흑화살 발사를 위한 공격 횟수")]
    [SerializeField] private int attackCount;
    [SerializeField] private float damage;
    public int AttackCount { get => attackCount; }
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }

    public void AttackAllArcher()
    {
        UpdateArcherList();
        foreach (var item in archers)
        {
            item.GetComponent<Unit>().DecreaseHp(damage);
        }
        archers.Clear();
    }
    private void UpdateArcherList()
    {
        foreach (var item in InGameManager.Instance.UnitList)
        {
            if (item.GetComponent<Archer>())
            {
                archers.Add(item);
            }
        }
    }
}
