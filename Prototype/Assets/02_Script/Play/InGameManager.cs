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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
