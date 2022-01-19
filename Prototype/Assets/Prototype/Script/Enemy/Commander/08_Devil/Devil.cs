using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Devil : MonoBehaviour
{
    private bool isPageChange;
    private int posXMin = 2;
    private int posXMax = 3;
    private Boss boss;
    private Player player;
    private List<GameObject> collisions = new List<GameObject>();
    private List<GameObject> crushCollisions = new List<GameObject>();
    [SerializeField] private GameObject crushPrefab;
    [SerializeField] private List<GameObject> crushs = new List<GameObject>();
    [SerializeField] private Image annihilationImage;
    [SerializeField] private Text text;
    [SerializeField] private GameObject[] swords;
    [SerializeField] private float swordDamage;
    [SerializeField] private float crushDamage;
    [SerializeField] private int crushCount;

    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public bool IsPageChange { get => isPageChange; }
    public List<GameObject> CrushCollisions { get => crushCollisions; set => crushCollisions = value; }

    public void ChangePage()
    {
        StartCoroutine(Co_Annihilation());
        isPageChange = true;
    }
    public void ActiveCrush()
    {
        StartCoroutine(Co_ActiveCrush());
    }
    private void InitCrush()
    {
        for (int i = 1; i < crushCount + 1; i++)
        {
            var obj = Instantiate(crushPrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3(-i - (i * 0.1f), 0);
            obj.SetActive(false);
            crushs.Add(obj);
        }
    }
    private void AttackSword()
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            if (collisions[i] != null)
            {
                if (collisions[i].GetComponent<Unit>())
                {
                    collisions[i].GetComponent<Unit>().DecreaseHp(swordDamage);
                }
                else if (collisions[i].GetComponent<Player>())
                {
                    collisions[i].GetComponent<Player>().DecreaseHp(swordDamage);
                }
            }
        }
        ResetSword();
    }
    private void ResetSword()
    {
        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            swords[i].SetActive(false);
            swords[i].transform.SetParent(transform);
        }
        posXMin = 2;
        posXMax = 3;
        collisions.Clear();
    }
    private void AttackCrush()
    {
        foreach (var item in crushCollisions)
        {
            if (item.GetComponent<Unit>())
            {
                item.GetComponent<Unit>().DecreaseHp(crushDamage);
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().DecreaseHp(crushDamage);
            }
        }
    }
    private void ResetCrush(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.GetChild(0).GetComponent<Transform>().localScale = new Vector3(0, 1);
    }
    private IEnumerator Co_ActiveCrush()
    {
        yield return new WaitForSeconds(1f);
        foreach (var item in crushs)
        {
            item.SetActive(true);
            item.transform.GetChild(0).GetComponent<Transform>().DOScale(1, 1f);
            yield return new WaitForSeconds(1.1f);
            AttackCrush();
            ResetCrush(item);
        }
        boss.BossState = EUnitState.Battle;
    }
    private IEnumerator Co_Annihilation()
    {
        annihilationImage.DOFade(1, 3);
        yield return new WaitForSeconds(3.1f);
        foreach (var item in InGameManager.Instance.UnitList)
        {
            item.GetComponent<Unit>().DecreaseHp(item.GetComponent<Unit>().CurrentHp);
        }
        player.DecreaseHp(player.CurrentHp);
        annihilationImage.color = new Color(255,0,0,0);
        InitCrush();
        yield return new WaitForSeconds(1);
        text.text = "Àü ¿ë»ç(¸¶¿Õ)";
        boss.IncreaseHp(boss.MaxHp);
        boss.BossState = EUnitState.Battle;
    }
    private IEnumerator Co_ActiveSword()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax),
                                       Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            swords[i].transform.position = rand;
            swords[i].transform.SetParent(transform.parent);
            swords[i].transform.GetChild(0).GetComponent<Transform>().DOScale(1, 3f);
            posXMin += 2;
            posXMax += 3;
        }
        yield return new WaitForSeconds(3.1f);
        AttackSword();
    }
    private IEnumerator Co_Sword()
    {
        float time = 0;
        while (true)
        {
            Debug.Log(time);
            time += Time.deltaTime;
            if (time >= 5)
            {
                StartCoroutine(Co_ActiveSword());
                time = 0;
            }
            yield return null;
        }            
    }
    
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
        player = FindObjectOfType<Player>();
        StartCoroutine(Co_Sword());
    }
}
