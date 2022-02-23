using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : MonoBehaviour
{
    [SerializeField] private float dotDamage;
    [SerializeField] private float slowDownValue;

    /*private void Shouting()
    {
        float knockBackTime = stunTime;
        foreach (var item in InGameManager.Instance.Units)
        {
            item.isKnockBack = true;
            item.penaltyTime = knockBackTime--;
        }
        isShout = true;
    }*/
}
