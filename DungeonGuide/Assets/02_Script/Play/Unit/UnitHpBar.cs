using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHpBar : MonoBehaviour
{
    private Image hpBar; //���� Hp�� �̹���
    private Image shield;
    private Unit unit; //���� Ŭ���� ���۷���
    [SerializeField] private int unitIndex;

    private IEnumerator Co_UpdateBossHpBar() // ���� Hp�ٸ� �������� �ڷ�ƾ. Hp ��ȭ�� ���� ������ ������� �� �ڷ�ƾ���� ����ϴ°� �´� ��
    {
        while (true)
        {
            hpBar.fillAmount = 1 / unit.CommonStatus.MaxHp * unit.CommonStatus.CurrentHp;
            shield.fillAmount = 1 / unit.CommonStatus.Shield * unit.CommonStatus.CurrentShield;
            yield return null;
        }
    }
    private void Awake() //�ش� �κе� �̴ϼȶ���¡ �Լ��� �����ұ�?
    {
        hpBar = GetComponent<Image>();
        shield = transform.GetChild(0).GetComponent<Image>();
       // unit = InGameManager.Instance.Units[unitIndex];
    }
    private void Start()
    {
        unit = InGameManager.Instance.Units[unitIndex];
        StartCoroutine(Co_UpdateBossHpBar());
    }
}
