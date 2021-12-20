using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private float enemySpawnDelay;
    [SerializeField] private Transform startPoint; 
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
    private float timerSecond;
    private int timerMinute;
    [SerializeField] private Image playerHp;
    [SerializeField] private Image playerMp;

    private int unitListIndex = 0;
    private int enemyListIndex = 0;
    public List<GameObject> UnitList { get => unitList; }
    public List<GameObject> EnemyList { get => enemyList; }
    

    private void Awake()
    {
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
        StartCoroutine(Co_InstantiateEnemy());
        StartCoroutine(Co_UpdateResauceUI());
        StartCoroutine(Co_Timer());
        //�ڷ�ƾ StartCoroutine(Co_UpdateList());
    }
    public void InstantiateUnit(int i)//���� ���� -> Unit unit)
    {
        if (player.CurrentResauce < unitArray[i].GetComponent<Unit>().Cost) return;
        var unit =  Instantiate(unitArray[i], unitParent,true);
        player.CurrentResauce -= unit.GetComponent<Unit>().Cost;
        SpawnRandomPoint(unit,-9.5f);
        AddList(unit, true);
      //  GetPosY(unit, true);
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
    public void InstatiateEnemy()
    {
        int rand = Random.Range(0, enemyArray.Length);
        var enemy = Instantiate(enemyArray[rand], enemyParent, true);
        SpawnRandomPoint(enemy, 9.5f);
        AddList(enemy, false);
       // GetPosY(enemy, false);
    }
   IEnumerator Co_InstantiateEnemy()
    {
        while (true)
        {
            InstatiateEnemy();
            yield return new WaitForSeconds(enemySpawnDelay);
        }
        
    }

    private void AddList(GameObject obj, bool isUnit)
    {       
        if (isUnit == true)
        {
            unitList.Insert(unitListIndex++, obj);
        }
        else
        {
            enemyList.Insert(enemyListIndex++, obj);
        }
    }
    private void SetSortingLayerOrder(GameObject obj,float rand)
    {       
        float i = rand * 1000;
        int j = (int)i;
        obj.GetComponent<SpriteRenderer>().sortingOrder = -j;
    }
    IEnumerator Co_UpdateResauceUI()
    {
        while (true)
        {
            resauceGraph.fillAmount = player.CurrentResauce * (1 / player.MaxResauce); //���� �����ؾ���. �ڿ��� �������� �ƴϱ� ����.
            resauceText.text = string.Format("{0:00} / {1:00}", System.Math.Truncate(player.CurrentResauce), player.MaxResauce);
            //Format �޼ҵ� ��� �� ������ �Ҽ��� ������ �ݿø��ع���. �׷��� �ڿ� �ؽ�Ʈ�� �ݿø��Ǽ� ��Ÿ����
            //���÷� ������ �ڿ��� 2.6�ۿ� �ȸ𿴴µ� �ؽ�Ʈ�� 3���� ��Ÿ��. -> Math.Trucate �޼ҵ�� ���� �ڿ��� ����ó���ؼ� �ؽ�Ʈ�� ǥ��
            yield return null;
        }
    }
    IEnumerator Co_Timer()
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
    public void DecreasePlayerUI(bool isHp)
    {
        if (isHp)
        {
            playerHp.fillAmount = player.CurrentHp * (1/player.MaxHp); // ���� �����ؾ���. �÷��̾� ü���� �������� �ƴϱ� ����.
        }
        else
        {
            playerMp.fillAmount = player.CurrentMp * (1/player.MaxMp); //�굵 ��������.
        }
    }
    
    
}
