using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private InGameManager inGameManager;
    private UNIT_STATE enemyState = UNIT_STATE.NON_COMBAT;

    // 인게임 매니저에다 hp증가, 감소 함수를 만들고 파라메터로 누구 hp를 감소시킬거냐
    [SerializeField] private float attackSpeed;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject currentUnit;


    public UNIT_STATE EnemyState { get => enemyState; set => enemyState = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public GameObject CurrentUnit { get => currentUnit; set => currentUnit = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }

    private void Awake()
    {
        inGameManager = FindObjectOfType<InGameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_BATTLE());
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState == UNIT_STATE.NON_COMBAT)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }       
        /*if (inGameManager.UnitList.Count <= 0) return;       
        float dist = Vector3.Distance(transform.position, inGameManager.UnitList[0].gameObject.transform.position);
        if (dist < attackDist)
        {
            uNIT_STATE = UNIT_STATE.BATTLE;
        }*/
        if(currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator Co_BATTLE() //배틀 -> 논컴뱃 전환에 딜레이가 약간 필요해 보임. (너무 빨리 다시 추적함)
    {
        while (true)
        {
            if(currentUnit != null)
            {
                if (currentUnit.GetComponent<Player>())
                {
                    Attack();
                    if (currentUnit.GetComponent<Player>().CurrentHp > 0)
                    {
                        yield return new WaitForSeconds(attackSpeed);
                    }
                    else
                    {
                        enemyState = UNIT_STATE.NON_COMBAT;
                    }
                    
                }
                else if (currentUnit.GetComponent<Unit>())
                {
                    Attack();
                    if (currentUnit.GetComponent<Unit>().CurrentHp > 0)
                    {
                        yield return new WaitForSeconds(attackSpeed);
                    }
                    else
                    {
                        enemyState = UNIT_STATE.NON_COMBAT;
                    }
                }              
            }
            yield return null;
        }
    }
    void Attack()
    {
        if (currentUnit != null)
        {
            if (currentUnit.GetComponent<Player>())
            {
                currentUnit.GetComponent<Player>().CurrentHp -= attackPower;
                inGameManager.DecreasePlayerUI(true);
            }
            else if (currentUnit.GetComponent<Unit>())
            {
                currentUnit.GetComponent<Unit>().CurrentHp -= attackPower;
                currentUnit.GetComponentInChildren<HpBar>().DecreaseUnitOrEnemyHpUI(true);
            }
        }
        
    }
}
