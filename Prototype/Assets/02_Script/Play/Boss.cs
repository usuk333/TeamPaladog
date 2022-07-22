using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Boss : MonoBehaviour
{ 
    private enum EBossState
    {
        Idle,
        Attack
    }
    private enum EBossKind
    {
        Mushroom,
        other
    }
    [SerializeField]
    private EBossKind eBossKind;
    public bool isPattern { get; set; }
    [SerializeField] private int arrayCount;
    [SerializeField] private List<List<GameObject>> collisionsArray = new List<List<GameObject>>();
    [SerializeField] private Unit currentUnit;
    [SerializeField] private EBossState eBossState;
    [SerializeField] private CommonStatus commonStatus = new CommonStatus();
    [SerializeField] private float damageDelay;
    [SerializeField] private float attackAnimDelay;
    public SkeletonAnimation skeletonAnimation;
    public CommonStatus CommonStatus { get => commonStatus; set => commonStatus = value; }
    public List<List<GameObject>> CollisionsArray { get => collisionsArray; set => collisionsArray = value; }

    public void InitializeBossStatus()//�ش� �Լ��� InGameManager�� Awake���� ����� �ڷ�ƾ���� ȣ��
    {
        //�ʱ�ȭ �ʿ��� ������ �ʱ�ȭ
    }
    private void Attack() //���� ��Ÿ
    {
        if(currentUnit == null) //���� �� ������ �÷��̾� ����
        {
            InGameManager.Instance.Player.DecreaseHp(commonStatus.CurrentAttackDamage);
            return;
        }
        currentUnit.CommonStatus.DecreaseHp(commonStatus.CurrentAttackDamage);
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
                    skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
                    if (eBossKind == EBossKind.other)
                    {
                        skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, attackAnimDelay);
                    }
                    else
                    {
                        skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, attackAnimDelay);
                    }
                    yield return new WaitForSeconds(damageDelay);
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
        commonStatus.CurrentHp = commonStatus.MaxHp;
        for (int i = 0; i < arrayCount; i++)
        {
            collisionsArray.Add(new List<GameObject>());
        }
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if(eBossKind == EBossKind.other)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }
        else
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        }
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateUnitReference());
        StartCoroutine(Co_UpdateState());
    }
}
