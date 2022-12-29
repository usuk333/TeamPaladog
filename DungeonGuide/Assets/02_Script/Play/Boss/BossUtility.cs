using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossUtility
{
    [SerializeField] private float stunTime;
    public void KnockBack()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            item.isKnockBack = true;
            item.penaltyTime = stunTime;
        }
    }
}
