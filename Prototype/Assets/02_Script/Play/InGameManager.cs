using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //�ΰ��Ӿ� Ŭ�������� ���� �����ϵ��� ����ƽ���� ���
    [SerializeField] private Unit[] units;

    public static InGameManager Instance { get => instance; }
    public Unit[] Units { get => units; }

    //�����غ��� ��������Ʈ�� �ϴ°� �� ������?
    private IEnumerator Co_InitializeInGameData() //�ʱ�ȭ�� �ʿ��� ��� �ΰ��� ������ �ʱ�ȭ. �Ϸ�Ǹ� true ��ȯ �� �ε� �Ϸ�.
    {
        yield return null;
    }
    private void Awake()
    {
        instance = this;
        StartCoroutine(Co_InitializeInGameData());
        units = FindObjectsOfType<Unit>();
        Unit unit;
        for (int i = 0; i < units.Length; i++) //�� �κ� ���Ŀ� �Լ��� ���� (���ָ���Ʈ ��Ŀ ~ ���Ÿ� ������ ����)
        {
            switch (units[i].UnitType)
            {
                case Unit.EUnitType.Tanker:
                    unit = units[0];
                    units[0] = units[i];
                    units[i] = unit;
                    break;
                case Unit.EUnitType.CloseDealer:
                    unit = units[1];
                    units[1] = units[i];
                    units[i] = unit;
                    break;
                case Unit.EUnitType.Wizard:
                    unit = units[2];
                    units[2] = units[i];
                    units[i] = unit;
                    break;
                case Unit.EUnitType.RemoteDealer:
                    unit = units[3];
                    units[3] = units[i];
                    units[i] = unit;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
