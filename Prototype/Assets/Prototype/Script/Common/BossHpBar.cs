using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour //보스 캐릭터의 Hp바 UI를 갱신해주는 스크립트
{
    private Image hpBar;
    private Boss boss;
    public void UpdateBossHpUI()
    {
        hpBar.fillAmount = boss.CurrentHp / boss.MaxHp;
    }
    private IEnumerator Co_UpdateBossHpUI() //보스 체력바 감소를 부드럽게 한다면 해당 코루틴으로 교체
    {
        while (true)
        {
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, boss.CurrentHp / boss.MaxHp, 5 * Time.deltaTime);//boss.CurrentHp / boss.MaxHp;
            yield return null;
        }
    }
    private void Awake()
    {
        hpBar = GetComponent<Image>();
        boss = GetComponentInParent<Boss>();
       // StartCoroutine(Co_UpdateBossHpUI());
    }
}
