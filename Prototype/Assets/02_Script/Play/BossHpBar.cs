using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    private Image hpBar; //���� Hp�� �̹���
    private Boss boss; //���� Ŭ���� ���۷���
    private IEnumerator Co_UpdateBossHpBar() // ���� Hp�ٸ� �������� �ڷ�ƾ. Hp ��ȭ�� ���� ������ ������� �� �ڷ�ƾ���� ����ϴ°� �´� ��
    {
        while (true)
        {
            hpBar.fillAmount = 1 / boss.CommonStatus.MaxHp * boss.CommonStatus.CurrentHp;
            yield return null;
        }
    }
    private void Awake() //�ش� �κе� �̴ϼȶ���¡ �Լ��� �����ұ�?
    {
        hpBar = GetComponent<Image>();
        boss = GetComponentInParent<Boss>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateBossHpBar());
    }
}
