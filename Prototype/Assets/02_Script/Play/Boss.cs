using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{ 
    private enum EBossState
    {
        Idle,
        Attack
    }
    public bool isPattern { get; set; }
    private Player player;
    [SerializeField] private int arrayCount;
    [SerializeField] private List<List<GameObject>> collisionsArray = new List<List<GameObject>>();
    [SerializeField] private List<GameObject> patternCollisions = new List<GameObject>();
    [SerializeField] private Unit currentUnit;
    [SerializeField] private EBossState eBossState;
    [SerializeField] private CommonStatus commonStatus = new CommonStatus();
    public CommonStatus CommonStatus { get => commonStatus; set => commonStatus = value; }
    public Player Player { get => player; }
    public List<GameObject> PatternCollisions { get => patternCollisions; set => patternCollisions = value; }
    public List<List<GameObject>> CollisionsArray { get => collisionsArray; set => collisionsArray = value; }

    public void InitializeBossStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
    }
    private void Attack() //보스 평타
    {
        if(currentUnit == null) //유닛 다 죽으면 플레이어 공격
        {
            player.DecreaseHp(commonStatus.AttackDamage);
            return;
        }
        currentUnit.CommonStatus.DecreaseHp(commonStatus.AttackDamage);
    }
    private IEnumerator Co_UpdateUnitReference()
    {
        //인게임매니저의 유닛 리스트 인덱스를 각각 할당시켜주면 될 듯 (0,1,2,3 순서로 탱커, 근딜, 마법사, 원거리 딜러)
        int i = 0;
        currentUnit = InGameManager.Instance.Units[i];
        while (true)
        {
            if(currentUnit.CommonStatus.CurrentHp <= 0)
            {
                if(i >= 3)
                {
                    currentUnit = null;
                    yield break;
                }
                currentUnit = InGameManager.Instance.Units[++i];
            }
            yield return null;
        }
    }
    private IEnumerator Co_UpdateState() // 보스 유한상태기계
    {
        while (true)
        {
            yield return null;
            if (isPattern)
            {
                continue;
            }
            switch (eBossState)
            {
                case EBossState.Idle:
                    yield return new WaitForSeconds(commonStatus.AttackSpeed);
                    eBossState = EBossState.Attack;
                    break;
                case EBossState.Attack:
                    Attack();
                    eBossState = EBossState.Idle;
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()//임시로 초기화
    {
        player = FindObjectOfType<Player>();
        commonStatus.CurrentHp = commonStatus.MaxHp;
        for (int i = 0; i < arrayCount; i++)
        {
            collisionsArray.Add(new List<GameObject>());
        }
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateUnitReference());
        StartCoroutine(Co_UpdateState());
    }
}
