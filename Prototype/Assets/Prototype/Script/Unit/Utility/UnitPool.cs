using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    private static UnitPool instance;
    [SerializeField] private GameObject[] poolingUnits;
    [SerializeField] private List<Unit> poolingUnitList = new List<Unit>();
    public static Unit GetUnit(int index)
    {
        if (instance.poolingUnitList.Find(x => x.Index == index))
        {
            Debug.Log("풀에서 꺼내옴");
            var unit = instance.poolingUnitList.Find(x => x.Index == index);
            instance.poolingUnitList.Remove(unit);
            unit.gameObject.SetActive(true);
            return unit;
        }
        else
        {
            Debug.Log("풀에 없어서 새로 만듬");
            var unit = instance.CreateNewUnit(index);
            unit.gameObject.SetActive(true);
            return unit;
        }
    }
    public static void ReturnUnit(Unit unit)
    {
        Debug.Log("풀로 돌아감");
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(instance.transform);
        instance.poolingUnitList.Add(unit);
    }
    private void InitializePool(int initCount)
    {
        for (int i = 0; i < poolingUnits.Length; i++)
        {
            for (int j = 0; j < initCount; j++)
            {
                poolingUnitList.Add(CreateNewUnit(i));
            }
        }
    }
    private Unit CreateNewUnit(int index)
    {
        Unit unit = Instantiate(poolingUnits[index]).GetComponent<Unit>();
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(transform);
        return unit;
    }
    private void Awake()
    {
        instance = this;
        InitializePool(1);
    }
}
