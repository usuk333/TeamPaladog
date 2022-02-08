using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHpBar : MonoBehaviour
{
    private Image hpBar; //���� Hp�� �̹���
    private Unit unit; //���� Ŭ���� ���۷���
    [SerializeField] private int unitIndex;

    private IEnumerator Co_UpdateBossHpBar() // ���� Hp�ٸ� �������� �ڷ�ƾ. Hp ��ȭ�� ���� ������ ������� �� �ڷ�ƾ���� ����ϴ°� �´� ��
    {
        while (true)
        {
            hpBar.fillAmount = 1 / unit.MaxHp * unit.CurrentHp;
            yield return null;
        }
    }
    private void Awake() //�ش� �κе� �̴ϼȶ���¡ �Լ��� �����ұ�?
    {
        hpBar = GetComponent<Image>();
       // unit = InGameManager.Instance.Units[unitIndex];
    }
    private void Start()
    {
        unit = InGameManager.Instance.Units[unitIndex];
        StartCoroutine(Co_UpdateBossHpBar());
    }
}
