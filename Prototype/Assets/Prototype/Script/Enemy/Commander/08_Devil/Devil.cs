using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Devil : MonoBehaviour
{
    private bool isPageChange;
   
    private Boss boss;
    private Player player;
    private List<GameObject> collisions = new List<GameObject>();
    private List<GameObject> crushCollisions = new List<GameObject>();
    [SerializeField] private GameObject crushPrefab;
    [SerializeField] private List<GameObject> crushs = new List<GameObject>();
    [SerializeField] private Image annihilationImage;
    [SerializeField] private Text text;
    [Header("떨어지는 칼 생성 주기")]
    [SerializeField] private float swordSecond;
    [Header("떨어지는 칼 생성 후 직접 피해를 주기까지 시간")]
    [SerializeField] private float swordAttackSecond;
    [Header("떨어지는 칼들 사이의 간격")]
    [SerializeField] private int posXMin = 2;
    [SerializeField] private int posXMax = 3;
    [Header("떨어지는 칼 개수")]
    [SerializeField] private GameObject[] swords;
    [Header("떨어지는 칼 데미지")]
    [SerializeField] private float swordDamage;
    [Header("전멸 패턴 발동 시간")]
    [SerializeField] private float annihiationSecond;
    [Header("충격파 데미지")]
    [SerializeField] private float crushDamage;
    [Header("충격파 생성 후 직접 피해를 주기까지 시간")]
    [SerializeField] private float crushSecond;
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
        foreach (var item in collisions)
        {
            if (item.GetComponent<Unit>())
            {
                item.GetComponent<Unit>().DecreaseHp(swordDamage);
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().DecreaseHp(swordDamage);
            }
        }
        ResetSword();
    }
    private void ResetSword()
    {
        foreach (var item in swords)
        {
            item.transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            item.SetActive(false);
            item.transform.SetParent(transform);
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
            item.transform.GetChild(0).GetComponent<Transform>().DOScale(1, crushSecond);
            yield return new WaitForSeconds(crushSecond + 0.1f);
            AttackCrush();
            ResetCrush(item);
        }
        boss.BossState = EUnitState.Battle;
    }
    private IEnumerator Co_Annihilation()
    {
        annihilationImage.DOFade(1, annihiationSecond);
        yield return new WaitForSeconds(annihiationSecond + 0.1f);
        foreach (var item in InGameManager.Instance.UnitList)
        {
            item.GetComponent<Unit>().DecreaseHp(item.GetComponent<Unit>().CurrentHp);
        }
        player.DecreaseHp(player.CurrentHp);
        annihilationImage.color = new Color(255,0,0,0);
        InitCrush();
        yield return new WaitForSeconds(1);
        text.text = "전 용사(마왕)";
        boss.IncreaseHp(boss.MaxHp);
        boss.BossState = EUnitState.Battle;
    }
    private IEnumerator Co_ActiveSword()
    {
        yield return new WaitForSeconds(1f);
        foreach (var item in swords)
        {
            item.SetActive(true);
            Vector3 rand = new Vector3(Random.Range(transform.position.x - posXMin, transform.position.x - posXMax),
                                       Random.Range(transform.position.y, transform.position.y - 0.5f), transform.position.z);
            item.transform.position = rand;
            item.transform.SetParent(transform.parent);
            item.transform.GetChild(0).GetComponent<Transform>().DOScale(1, swordAttackSecond);
            posXMin += 2;
            posXMax += 3;
        }
        yield return new WaitForSeconds(swordAttackSecond + 0.1f);
        AttackSword();
    }
    private IEnumerator Co_Sword()
    {
        float time = 0;
        while (true)
        {
            Debug.Log(time);
            time += Time.deltaTime;
            if (time >= swordSecond)
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
