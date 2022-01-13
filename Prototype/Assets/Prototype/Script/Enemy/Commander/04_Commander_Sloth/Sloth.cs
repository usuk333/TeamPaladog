using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sloth : MonoBehaviour
{
    private int attackCount = 0;
    private int patternCount = 3;
    private int currentPatternCount = 0;
    private int posXMin = 2;
    private int posXMax = 3;
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private GameObject[] thorns;
    [SerializeField] private float thornPower;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public int AttackCount { get => attackCount; set => attackCount = value; }

    public void ActiveThorn()
    {
        StartCoroutine(Co_ActiveThorn());
    }
    private void AttackThorn()
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            if (collisions[i] != null)
            {
                if (collisions[i].GetComponent<Unit>())
                {
                    collisions[i].GetComponent<Unit>().DecreaseHp(thornPower);
                }
                else if (collisions[i].GetComponent<Player>())
                {
                    collisions[i].GetComponent<Player>().DecreaseHp(thornPower);
                }
            }
        }
        ResetThorn();
        if (++currentPatternCount >= patternCount)
        {
            currentPatternCount = 0;
            boss.BossState = EUnitState.Battle;
        }
        else
        {
            ActiveThorn();
        }
    }
    private void ResetThorn()
    {
        for (int i = 0; i < thorns.Length; i++)
        {
            thorns[i].transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            thorns[i].SetActive(false);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private IEnumerator Co_ActiveThorn()
    {
        yield return new WaitForSeconds(1f);
        boss.BossState = EUnitState.Wait;
        for (int i = 0; i < thorns.Length; i++)
        {
            thorns[i].SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax),
                                       Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            thorns[i].transform.position = rand;
            thorns[i].transform.GetChild(0).GetComponent<Transform>().DOScale(1, 3f);
            posXMin += 2;
            posXMax += 3;
        }
        yield return new WaitForSeconds(3.1f);
        AttackThorn();
    }
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
}
