using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNIT_KINDS
{
    WARRIOR,
    ARCHER,
    WIZARD,
    SHIELDER,
    PRIEST,
    ASSASSIN,
    SUMMONER,
    DRUID,
    SPECIAL
}
public enum UNIT_STATE
{
    BATTLE,
    NON_COMBAT,
    DIE
}
public class Unit : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float cost;

    private InGameManager inGameManager;

    [SerializeField] private UNIT_STATE unitState = UNIT_STATE.NON_COMBAT;
    [SerializeField] private UNIT_KINDS unitKinds = UNIT_KINDS.WARRIOR;

    private Vector3 targetPos;

    [SerializeField] private Enemy currentEnemy;

    public UNIT_STATE UnitState { get => unitState; set => unitState = value; }
    public Enemy CurrentEnemy { get => currentEnemy; set => currentEnemy = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Cost { get => cost; set => cost = value; }

    private void Awake()
    {
        inGameManager = FindObjectOfType<InGameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Co_CheckFirstTargetEnemy());
        StartCoroutine(Co_BATTLE());
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if(unitState == UNIT_STATE.NON_COMBAT)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        /*if (inGameManager.EnemyList.Count <= 0 ) return;
        //transform.position += Vector3.right * speed * Time.deltaTime;
        float dist = Vector3.Distance(transform.position, inGameManager.EnemyList[0].gameObject.transform.position);                 
        if(dist < attackDist)
        { 
            uNIT_STATE = UNIT_STATE.BATTLE;
        }*/
        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    IEnumerator Co_CheckFirstTargetEnemy()
    {
        while (true)
        {
            Debug.Log(System.Math.Truncate(transform.position.x*10)/10);
            yield return new WaitForSeconds(1f);
        }
    }
 
    IEnumerator Co_BATTLE()
    {
        while (true)
        {
            if (unitState == UNIT_STATE.BATTLE)
            {
                if (currentEnemy == null) unitState = UNIT_STATE.NON_COMBAT;
                switch (unitKinds)
                {
                    case UNIT_KINDS.WARRIOR:
                        //공격 애니메이션
                        Attack(unitKinds);
                        if(currentEnemy.CurrentHp > 0)
                        {
                            yield return new WaitForSeconds(attackSpeed);
                        }
                        else
                        {
                            unitState = UNIT_STATE.NON_COMBAT;
                        }
                        break;
                    case UNIT_KINDS.ARCHER:
                        Attack(unitKinds);
                        if(currentEnemy.CurrentHp > 0)
                        {
                            yield return new WaitForSeconds(attackSpeed);
                        }
                        else
                        {
                            unitState = UNIT_STATE.NON_COMBAT;
                        }
                        break;
                    case UNIT_KINDS.WIZARD:
                        break;
                    case UNIT_KINDS.SHIELDER:
                        break;
                    case UNIT_KINDS.PRIEST:
                        break;
                    case UNIT_KINDS.ASSASSIN:
                        break;
                    case UNIT_KINDS.SUMMONER:
                        break;
                    case UNIT_KINDS.DRUID:
                        break;
                    case UNIT_KINDS.SPECIAL:
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
        
    }
    void Attack(UNIT_KINDS unit_Kinds)
    {
        if (currentEnemy == null) return;
        switch (unit_Kinds)
        {
            case UNIT_KINDS.WARRIOR:
                currentEnemy.CurrentHp -= attackPower;               
                break;
            case UNIT_KINDS.ARCHER:
                currentEnemy.CurrentHp -= attackPower;
                break;
            case UNIT_KINDS.WIZARD:
                break;
            case UNIT_KINDS.SHIELDER:
                break;
            case UNIT_KINDS.PRIEST:
                break;
            case UNIT_KINDS.ASSASSIN:
                break;
            case UNIT_KINDS.SUMMONER:
                break;
            case UNIT_KINDS.DRUID:
                break;
            case UNIT_KINDS.SPECIAL:
                break;
            default:
                break;
        }
        currentEnemy.GetComponentInChildren<HpBar>().DecreaseUnitOrEnemyHpUI(false);
    }
}
