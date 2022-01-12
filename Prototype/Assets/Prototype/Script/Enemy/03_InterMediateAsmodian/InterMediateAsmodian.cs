using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterMediateAsmodian : MonoBehaviour
{
    private float damage = 0.5f;
    [SerializeField] private int attackCount;
    [SerializeField] private List<GameObject> archers = new List<GameObject>();
    public int AttackCount { get => attackCount; set => attackCount = value; }
    public void AttackAllArcher()
    {
        UpdateArcherList();
        for (int i = 0; i < archers.Count; i++)
        {
            archers[i].GetComponent<Unit>().DecreaseHp(damage);
        }
        ResetArcherList();
    }
    private void UpdateArcherList()
    {
        for (int i = 0; i < InGameManager.Instance.UnitList.Count; i++)
        {
            if (InGameManager.Instance.UnitList[i].GetComponent<Archer>())
            {
                archers.Add(InGameManager.Instance.UnitList[i]);
            }
        }
    }
    private void ResetArcherList()
    {
        archers.Clear();
    }
}
