using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //�ΰ��Ӿ����� ���� �����ϵ��� ����ƽ���� ���
    [SerializeField] private Unit[] units;

    public static InGameManager Instance { get => instance; }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
