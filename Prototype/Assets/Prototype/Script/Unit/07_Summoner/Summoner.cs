using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    [SerializeField] private GameObject[] summons;
    private int summonCount = 0;
    private float attakSpeed;

    public float AttakSpeed { get => attakSpeed; set => attakSpeed = value; }

    private void Start()
    {
        attakSpeed = GetComponent<Unit>().AttackSpeed;
    }
    public void Summon()
    {
        int i = ChooseRandom();
        Instantiate(summons[i], transform.position, Quaternion.identity, transform);
        summonCount++;
        if(summonCount > 1)
        {
            Destroy(this.gameObject); //추후 오브젝트 풀링으로 전환
        }
        GetComponentInChildren<MpBar>().DecreaseMpBar();
    }
    private int ChooseRandom()
    {
        int total = 100;
        float rand = Random.value * total;
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
    public void ResetSummoner()
    {
        summonCount = 0;
        GetComponentInChildren<MpBar>().DecreaseMpBar();
    }
}
