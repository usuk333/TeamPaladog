using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Avarice : MonoBehaviour
{
    private int patternCount = 3;
    private int currentPatternCount = 0;
    private int posXMin = 2;
    private int posXMax = 3;
    private Boss boss;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private int dotCount;
    [SerializeField] private GameObject[] poisons;
    [SerializeField] private float explosionDamage;
    [SerializeField] private float poisonDamage;
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    // Start is called before the first frame update
    public void ActivePoison()
    {
        StartCoroutine(Co_ActivePoison());
    }
    private void AttackPoison()
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            if (collisions[i] != null)
            {
                if (collisions[i].GetComponent<Unit>())
                {
                    collisions[i].GetComponent<Unit>().DecreaseHp(explosionDamage);
                    collisions[i].GetComponent<Unit>().DecreaseHpDot(dotCount, poisonDamage, 2f);
                }
                else if (collisions[i].GetComponent<Player>())
                {
                    collisions[i].GetComponent<Player>().DecreaseHp(explosionDamage);
                    collisions[i].GetComponent<Player>().DecreaseHpDot(dotCount, poisonDamage, 2f);
                }
            }
        }
        ResetPoison();
        if (++currentPatternCount >= patternCount)
        {
            currentPatternCount = 0;
            boss.BossState = EUnitState.Battle;
        }
        else
        {
            ActivePoison();
        }
    }
    private void ResetPoison()
    {
        for (int i = 0; i < poisons.Length; i++)
        {
            poisons[i].transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            poisons[i].SetActive(false);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private IEnumerator Co_ActivePoison()
    {
        yield return new WaitForSeconds(1f);
        boss.BossState = EUnitState.Wait;
        for (int i = 0; i < poisons.Length; i++)
        {
            poisons[i].SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax),
                                       Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            poisons[i].transform.position = rand;
            poisons[i].transform.GetChild(0).GetComponent<Transform>().DOScale(1, 3f);
            posXMin += 2;
            posXMax += 3;
        }
        yield return new WaitForSeconds(3.1f);
        AttackPoison();
    }
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
}
