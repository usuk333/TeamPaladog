using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour //���� ĳ������ Hp�� UI�� �������ִ� ��ũ��Ʈ
{
    private Image hpBar;
    private Boss boss;
    public void UpdateBossHpUI()
    {
        hpBar.fillAmount = boss.CurrentHp / boss.MaxHp;
    }
    private IEnumerator Co_UpdateBossHpUI() //���� ü�¹� ���Ҹ� �ε巴�� �Ѵٸ� �ش� �ڷ�ƾ���� ��ü
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
