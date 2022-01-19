using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicTool : MonoBehaviour //보스캐릭터 강력한 마법도구의 공격 기능 스크립트
{
    private int patternCount = 3;
    private int currentPatternCount = 0;
    private int posXMin = 2;
    private int posXMax = 3;
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private GameObject[] lightnings;
    [SerializeField] private float damage;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }

    public void ActiveLightning()
    {      
        StartCoroutine(Co_ActiveLightning());
    }
    private void AttackLightning()
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            if (collisions[i] != null)
            {
                if (collisions[i].GetComponent<Unit>())
                {
                    collisions[i].GetComponent<Unit>().DecreaseHp(damage);
                }
                else if (collisions[i].GetComponent<Player>())
                {
                    collisions[i].GetComponent<Player>().DecreaseHp(damage);
                }
            }
        }
        ResetLightning();
        if (++currentPatternCount >= patternCount)
        {
            currentPatternCount = 0;
            boss.BossState = EUnitState.Battle;
        }
        else
        {
            ActiveLightning();
        }
    }
    private void ResetLightning()
    {
        for (int i = 0; i < lightnings.Length; i++)
        {
            lightnings[i].transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            lightnings[i].SetActive(false);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private IEnumerator Co_ActiveLightning()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < lightnings.Length; i++)
        {
            lightnings[i].SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax), 
                                       Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            lightnings[i].transform.position = rand;
            lightnings[i].transform.GetChild(0).GetComponent<Transform>().DOScale(1, 3f);
            posXMin += 2;
            posXMax += 3;
        }
        yield return new WaitForSeconds(3.1f);
        AttackLightning();
    }
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
}
