using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour //��ȯ�� ������ ��� ��ũ��Ʈ
{
    private int currentSummonCount = 0;
    private float attakSpeed;
    private MpBar mpBar;
    [Header("�ִ� ��ȯ Ƚ��")]
    [SerializeField] private int summonCount;
    [SerializeField] private GameObject[] summons;
    public float AttakSpeed { get => attakSpeed; set => attakSpeed = value; }

    private void Awake()
    {
        attakSpeed = GetComponent<Unit>().AttackSpeed;
        mpBar = GetComponentInChildren<MpBar>();
    }
    public void Summon()
    {
        int i = ChooseRandom();
        Instantiate(summons[i], transform.position, Quaternion.identity, transform); //���߿� ���� Ǯ������ ��ȯ���� 
        summonCount++;
        if(currentSummonCount > summonCount)
        {
            UnitPool.ReturnUnit(GetComponent<Unit>());
            InGameManager.Instance.UnitList.Remove(this.gameObject);
        }
        mpBar.DecreaseMpBar();
    }
    public void ResetSummoner()
    {
        currentSummonCount = 0;
        if(mpBar != null)
        {
            mpBar.DecreaseMpBar();
        }
    }
    private int ChooseRandom()
    {
        float rand = Random.value * 100;
        Debug.Log(rand);
        int value = 0;
        if(rand <= 45)
        {
            value = 0;
        }
        else if(rand > 45 && rand <= 80)
        {
            value = 1;
        }
        else if(rand > 80)
        {
            value = 2;
        }
        return value;
    }
}
