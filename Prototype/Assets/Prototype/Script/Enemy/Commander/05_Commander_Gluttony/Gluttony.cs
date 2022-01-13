using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gluttony : MonoBehaviour
{
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private GameObject eater;
    [SerializeField] private float damage;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }

    public void ActiveEater()
    {
        StartCoroutine(Co_ActiveEater());
    }
    private void AttackEater()
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            if (collisions[i].GetComponent<Unit>())
            {
                collisions[i].GetComponent<Unit>().DecreaseHp(damage);
                if(collisions[i].GetComponent<Unit>().CurrentHp <= 0)
                {
                    boss.IncreaseHp(collisions[i].GetComponent<Unit>().MaxHp);
                }
            }
            else if (collisions[i].GetComponent<Player>())
            {
                collisions[i].GetComponent<Player>().DecreaseHp(damage);
            }
        }
        ResetEater();
        boss.BossState = EUnitState.Battle;
    }
    private void ResetEater()
    {
        eater.transform.GetChild(0).transform.localScale = Vector2.zero;
        eater.SetActive(false);
        collisions.Clear();
    }
    private IEnumerator Co_ActiveEater()
    {
        yield return new WaitForSeconds(1f);
        eater.SetActive(true);
        Vector3 rand = new Vector3(Random.Range(-8.5f, transform.position.x), transform.position.y);
        eater.transform.position = rand;
        eater.transform.GetChild(0).transform.DOScale(1, 3f);
        yield return new WaitForSeconds(3.1f);
        AttackEater();
    }
    private void Awake()
    {
        boss = GetComponent<Boss>();
    }
}
