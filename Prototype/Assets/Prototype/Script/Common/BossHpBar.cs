using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    private Image hpBar;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = GetComponent<Image>();
    }

    public void UpdateBossHpUI()
    {
        hpBar.fillAmount = transform.parent.parent.GetComponent<Boss>().CurrentHp / transform.parent.parent.GetComponent<Boss>().MaxHp;
    }
}
