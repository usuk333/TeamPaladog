using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lust : MonoBehaviour
{
    private Boss boss;
    private List<GameObject> collisions = new List<GameObject>();
    private int posXMin = 2;
    private int posXMax = 3;
    private Queue<BomberMan> bomberManQueue = new Queue<BomberMan>();
    [SerializeField] private GameObject bomberMan;
    [SerializeField] private GameObject[] transitions;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }

    public void ActiveTransition()
    {
        StartCoroutine(Co_ActiveTransition());
    }
    private void InitBomberMan()
    {
        for (int i = 0; i < 4; i++)
        {
            bomberManQueue.Enqueue(CreateNewBomberMan());
        }
    }
    private void ActiveBomberMan(BomberMan bomberMan, Vector3 pos)
    {
        bomberMan.gameObject.SetActive(true);
        bomberMan.transform.SetParent(transform.parent);
        bomberMan.transform.position = pos;
    }
    private BomberMan GetBomberMan()
    {
        if (bomberManQueue.Count > 0)
        {
            var obj = bomberManQueue.Dequeue();
            return obj;
        }
        else
        {
            var newObj = CreateNewBomberMan();
            return newObj;
        }
    }
    public void ReturnBomberMan(BomberMan bomberMan)
    {
        bomberMan.gameObject.SetActive(false);
        bomberMan.transform.SetParent(transform);
        bomberManQueue.Enqueue(bomberMan);
    }
    private BomberMan CreateNewBomberMan()
    {
        BomberMan obj = Instantiate(bomberMan).GetComponent<BomberMan>();
        obj.Lust = this;
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    private void ResetTransition()
    {
        foreach (var item in transitions)
        {
            item.transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            item.SetActive(false);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private void TransitionUnit()
    {
        foreach (var item in collisions)
        {
            if (item.GetComponent<Unit>())
            {
                item.GetComponent<Unit>().DecreaseHp(item.GetComponent<Unit>().CurrentHp);
                ActiveBomberMan(GetBomberMan(), item.transform.position);
            }
        }
        ResetTransition();
        boss.BossState = EUnitState.Battle;
    }
    private IEnumerator Co_ActiveTransition()
    {
        yield return new WaitForSeconds(1f);
        foreach (var item in transitions)
        {
            item.SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax),
                                       Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            item.transform.position = rand;
            item.transform.GetChild(0).GetComponent<Transform>().DOScale(1, 3f);
            posXMin += 2;
            posXMax += 3;
        }
        yield return new WaitForSeconds(3.1f);
        TransitionUnit();
    }
    private void Awake()
    {
        boss = GetComponent<Boss>();
        InitBomberMan();
    }
}
