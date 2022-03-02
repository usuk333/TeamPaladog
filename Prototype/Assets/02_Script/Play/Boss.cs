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

    public void InitializeBossStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
    }
    private void Attack() //���� ��Ÿ
    {
        if(currentUnit == null) //���� �� ������ �÷��̾� ����
        {
            player.DecreaseHp(commonStatus.AttackDamage);
            return;
        }
        currentUnit.CommonStatus.DecreaseHp(commonStatus.AttackDamage);
    }
    private IEnumerator Co_UpdateUnitReference()
    {
        //�ΰ��ӸŴ����� ���� ����Ʈ �ε����� ���� �Ҵ�����ָ� �� �� (0,1,2,3 ������ ��Ŀ, �ٵ�, ������, ���Ÿ� ����)
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
    private IEnumerator Co_UpdateState() // ���� ���ѻ��±��
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
    private void Awake()//�ӽ÷� �ʱ�ȭ
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
