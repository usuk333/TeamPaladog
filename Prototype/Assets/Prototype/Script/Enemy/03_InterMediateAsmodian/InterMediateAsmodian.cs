using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterMediateAsmodian : MonoBehaviour
{
    private int currentAttackCount;
    private List<GameObject> archers = new List<GameObject>();
    [Header("����ȭ�� �߻縦 ���� ���� Ƚ��")]
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
