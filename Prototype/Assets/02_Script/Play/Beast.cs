using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : MonoBehaviour
{
    private bool isCrush;
    private bool isShout;
    private Boss boss;
    [SerializeField] private BossUtility bossUtility;
    [SerializeField] private float[] crushDamages;
    [SerializeField] private GameObject[] crushs;
    [SerializeField] private float[] conditionHp;

    private void Shouting()
    {
        bossUtility.KnockBack();
        isShout = true;
    }
    private IEnumerator Co_Crush()
    {
        while (true)
        {
            if (isCrush)
            {
                boss.isPattern = true;
                /* float rand = Random.Range(x, y); 패턴 반복 시간 (x~y 사이의 랜덤한 값마다 반복, 랜덤 값은 반복 할 때마다 변경됨)
                yield return new WaitForSeconds(rand);*/
                yield return new WaitForSeconds(1f);
                crushs[1].SetActive(true);
                yield return new WaitForSeconds(2f);
                Crush();
                crushs[1].SetActive(false);
                boss.PatternCollisions.Clear();
                isCrush = false;
                boss.isPattern = false;
            }
            yield return null;
        }
    }
    private IEnumerator Co_KnockBackCrush()
    {
        while (true)
        {
            if (isShout)
            {
                boss.isPattern = true;
                crushs[0].SetActive(true);
                yield return new WaitForSeconds(4f);
                KnockBackCrush();
                crushs[0].SetActive(false);
                boss.PatternCollisions.Clear();
                isShout = false;
                boss.isPattern = false;
            }
            yield return null;
        }      
    }
    private IEnumerator Co_CheckHp()
    {
        for (int i = 0; i < conditionHp.Length; i++)
        {
            yield return new WaitUntil(() => boss.CommonStatus.CurrentHp <= conditionHp[i]);
            boss.CommonStatus.AttackDamage = boss.CommonStatus.AttackDamage * 1.2f;
            boss.CommonStatus.AttackSpeed = boss.CommonStatus.AttackSpeed / 1.1f;
        }
    }
    private void Crush()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (boss.PatternCollisions.Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(crushDamages[1]);
            }
        }
        if (boss.PatternCollisions.Contains(boss.Player.gameObject))
        {
            boss.Player.DecreaseHp(crushDamages[1]);
        }
    }
    private void KnockBackCrush()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (boss.PatternCollisions.Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(crushDamages[0]);
            }
        }
        if (boss.PatternCollisions.Contains(boss.Player.gameObject))
        {
            boss.Player.DecreaseHp(crushDamages[0]);
        }
    }
    private void Awake()
    {
        boss = GetComponent<Boss>();
    }
    private void Start()
    {
        StartCoroutine(Co_KnockBackCrush());
        StartCoroutine(Co_Crush());
        StartCoroutine(Co_CheckHp());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shouting();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            isCrush = true;
        }
    }
}
