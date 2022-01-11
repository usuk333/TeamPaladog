using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour //�ü� ������ ���� ��� ��ũ��Ʈ
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private EParent parent;
    public void AttackArcher()
    {
        var obj = ProjectilePool.GetArrow();
        if (parent == EParent.Unit)
        {
            SetArrow(obj, EParent.Unit);
        }
        else
        {
            SetArrow(obj, EParent.Enemy);
        }
    }
    private void SetArrow(Arrow obj, EParent eParent)
    {
        obj.Parent = eParent;
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        obj.gameObject.SetActive(true);
    }
}
