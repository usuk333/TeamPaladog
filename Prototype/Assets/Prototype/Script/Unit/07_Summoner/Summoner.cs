using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour //소환사 유닛의 기능 스크립트
{
    private int currentSummonCount = 0;
    private float attakSpeed;
    private MpBar mpBar;
    [Header("최대 소환 횟수")]
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
        Instantiate(summons[i], transform.position, Quaternion.identity, transform); //나중에 로컬 풀링으로 전환하자 
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
