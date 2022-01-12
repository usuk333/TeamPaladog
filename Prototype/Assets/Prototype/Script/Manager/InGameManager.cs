using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour //인게임 플레이어 데이터와 전체적인 게임의 흐름을 관리하는 스크립트
{
    private static InGameManager instance;
    private float timerSecond;
    private int timerMinute;
    private List<Priest> priests = new List<Priest>();
    private List<Priest> enemyPriests = new List<Priest>();
    private int unitListIndex = 0;
    private int enemyListIndex = 0;
    [SerializeField] private float enemySpawnDelay;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private BoxCollider2D area;
    [SerializeField] private Player player;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform unitParent;
    [SerializeField] private GameObject[] unitArray;
    [SerializeField] private GameObject[] enemyArray;
    [SerializeField] private List<GameObject> unitList = new List<GameObject>();
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] private Image resauceGraph;
    [SerializeField] private Text resauceText;
    [SerializeField] private Text timer;
    [SerializeField] private Image playerHp;
    [SerializeField] private Image playerMp;
    [SerializeField] private GameObject[] bossObjects;
    [SerializeField] private Transform bossSpawnPoint;
    public List<GameObject> UnitList { get => unitList; }
    public List<GameObject> EnemyList { get => enemyList; }
    public static InGameManager Instance { get => instance; }
    public Transform BossSpawnPoint { get => bossSpawnPoint; }
    public void InstantiateUnit(int i)// 유닛 배열 i번째의 유닛 생성 //추후 오브젝트 풀링으로 전환
    {
        if (player.CurrentResauce < unitArray[i].GetComponent<Unit>().Cost) return;
        var unit = UnitPool.GetUnit(i);
        unit.transform.SetParent(unitParent);
        player.CurrentResauce -= unit.Cost;
        SpawnRandomPoint(unit.gameObject,startPoint.position.x);
        AddList(unit.gameObject, true);
        if(i == 4)
        {
            priests.Add(unit.GetComponent<Priest>());
        }
    }
    public void InstantiateEnemy(int ArrayCount = 0) //추후 오브젝트 풀링으로 전환
    {
        int rand = Random.Range(0, enemyArray.Length);
        var enemy = EnemyPool.GetEnemy(ArrayCount);
        enemy.transform.SetParent(enemyParent);
        SpawnRandomPoint(enemy.gameObject, endPoint.position.x);
        AddList(enemy.gameObject, false);
        if (ArrayCount == 4)
        {
            enemyPriests.Add(enemy.GetComponent<Priest>());
        }
    }
    public void InstantiateBoss(int index = 0)
    {
        var enemy = Instantiate(bossObjects[index], bossSpawnPoint.position, Quaternion.identity);
        enemy.transform.SetParent(enemyParent);
        AddList(enemy.gameObject, false);
    }
    public void UpdatePlayerHpMpUI(bool isHp)
    {
        if (isHp)
        {
            playerHp.fillAmount = player.CurrentHp / player.MaxHp; // 추후 수정해야함. 플레이어 체력이 고정값이 아니기 때문.-> 수정 완료(21.12.21)
        }
        else
        {
            playerMp.fillAmount = player.CurrentMp / player.MaxMp; //얘도 마찬가지. -> 수정 완료(21.12.21)
        }
    }
    public void RemoveHealingList(Unit unit)
    {
        foreach (var priest in priests)
        {
            priest.UpdateList(unit);
        }
    }
    public void RemoveEnemyHealingList(Enemy enemy)
    {
        foreach (var priest in enemyPriests)
        {
            priest.UpdateEnemyList(enemy);
        }
    }
    public void UpdatePriestList(Priest priest)
    {
        priests.Remove(priest);
    }
    public void UpdateEnemyPriestList(Priest priest)
    {
        enemyPriests.Remove(priest);
    }
    private void SpawnRandomPoint(GameObject obj, float posX)
    {
        Vector2 size = area.size;
        float randRange = Random.Range(0, size.y);
        Debug.Log(randRange);
        Vector3 randomRange = new Vector3(posX, area.transform.position.y + randRange, 0);
        obj.transform.position = randomRange;
        SetSortingLayerOrder(obj, randRange);
    }
    private void AddList(GameObject obj, bool isUnit)
    {       
        if (isUnit == true)
        {
            unitList.Add(obj);
        }
        else
        {
            enemyList.Add(obj);
        }
    }
    private void SetSortingLayerOrder(GameObject obj,float rand)
    {       
        float i = rand * 1000;
        int j = (int)i;
        obj.GetComponent<SpriteRenderer>().sortingOrder = -j;
    }
    private IEnumerator Co_UpdateResauceUI()
    {
        while (true)
        {
            resauceGraph.fillAmount = player.CurrentResauce * (1 / player.MaxResauce); //추후 수정해야함. 자원이 고정값이 아니기 때문.
            resauceText.text = string.Format("{0:00} / {1:00}", System.Math.Truncate(player.CurrentResauce), player.MaxResauce);
            //Format 메소드 사용 시 버리는 소수점 단위를 반올림해버림. 그래서 자원 텍스트가 반올림되서 나타나서
            //예시로 실제로 자원이 2.6밖에 안모였는데 텍스트로 3으로 나타남. -> Math.Trucate 메소드로 현재 자원을 내림처리해서 텍스트에 표시
            yield return null;
        }
    }
    private IEnumerator Co_Timer()
    {
        while (true)
        {
            if ((int)timerSecond > 59)
            {
                timerSecond = 0;
                timerMinute++;
            }
            timerSecond += Time.deltaTime;
            timer.text = string.Format("{0:00}:{1:00}", timerMinute, timerSecond);
 
            yield return null;
        }
    }
    private void Awake()
    {
        instance = this;
        area = startPoint.GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Player>();
        resauceGraph = GameObject.Find("Resauce").transform.GetChild(0).GetComponent<Image>();
        resauceText = GameObject.Find("Resauce").transform.GetChild(1).GetComponent<Text>();
        timer = GameObject.Find("Timer").transform.GetChild(0).GetComponent<Text>();
        playerHp = GameObject.Find("PlayerHP").transform.GetChild(0).GetComponent<Image>();
        playerMp = GameObject.Find("PlayerMP").transform.GetChild(0).GetComponent<Image>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateResauceUI());
        StartCoroutine(Co_Timer());
    }
}
