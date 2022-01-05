using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    [SerializeField] private GameObject fireBall;
    [SerializeField] private EParent parent;

    public void AttackWizard()
    {
        var obj = ProjectilePool.GetFireBall();
        if (parent == EParent.Unit)
        {
            SetFireBall(obj, EParent.Unit);
        }
        else
        {
            SetFireBall(obj, EParent.Enemy);
        }
        //Instantiate(fireBall, transform.position, Quaternion.identity, transform); //���� ������Ʈ Ǯ������ ��ȯ
    }
    private void SetFireBall(FireBall obj, EParent eParent)
    {
        obj.Parent = eParent;
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        obj.gameObject.SetActive(true);
    }
}
