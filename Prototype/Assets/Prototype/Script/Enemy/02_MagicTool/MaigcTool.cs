using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaigcTool : MonoBehaviour
{
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    [SerializeField] private GameObject[] lightnings;
    [SerializeField] private float lightningPower;
    private int attackCount = 0;


    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public int AttackCount { get => attackCount; set => attackCount = value; }

    public void ActiveLightning()
    {
        GetComponent<Boss>().BossState = EUnitState.Wait;
        int posXMin = 2;
        int posXMax = 3;
        for (int i = 0; i < lightnings.Length; i++)
        {
            lightnings[i].SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax), Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            //Vector3 rand = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            lightnings[i].transform.position = rand;
            lightnings[i].transform.GetChild(0).GetComponent<Transform>().DOScale(1, 3f);
            posXMin += 2;
            posXMax += 3;
        }
        Invoke("Invoke_AttackLightning", 3.1f);       
    }
    public void Invoke_AttackLightning()
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            if(collisions[i] != null)
            {
                if (collisions[i].GetComponent<Unit>())
                {
                    collisions[i].GetComponent<Unit>().DecreaseHp(lightningPower);
                }
                else if (collisions[i].GetComponent<Player>())
                {
                    collisions[i].GetComponent<Player>().DecreaseHp(lightningPower);
                }
            }
        }
        ResetLightning();
        GetComponent<Boss>().BossState = EUnitState.Battle;
    }
    private void ResetLightning()
    {
        for (int i = 0; i < lightnings.Length; i++)
        {
            lightnings[i].transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            lightnings[i].SetActive(false);
        }
        collisions.Clear();
    }
}
