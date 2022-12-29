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
        Gargoyle,
        Dummy,
        Other
    }
    [SerializeField]
    private EBossKind eBossKind;
    [SerializeField] private bool isPattern = false;
    public bool IsPattern { get => isPattern; set => isPattern = value; }
    [SerializeField] private int arrayCount;
    [SerializeField] private List<List<GameObject>> collisionsArray = new List<List<GameObject>>();
    [SerializeField] private Unit currentUnit;
    [SerializeField] private EBossState eBossState;
    [SerializeField] private CommonStatus commonStatus = new CommonStatus();
    [SerializeField] private float damageDelay;
    [SerializeField] private float attackAnimDelay;
    [SerializeField] private ParticleSystem attackEffect;
    public SkeletonAnimation skeletonAnimation;
    public CommonStatus CommonStatus { get => commonStatus; set => commonStatus = value; }
    public List<List<GameObject>> CollisionsArray { get => collisionsArray; set => collisionsArray = value; }

    public void InitializeBossStatus()//해당 함수는 InGameManager의 Awake에서 실행될 코루틴에서 호출
    {
        //초기화 필요한 변수들 초기화
    }
    private void Attack() //보스 평타
    {
        if(InGameManager.Instance.Units.Count > 0)
        {
            currentUnit = InGameManager.Instance.Units[0];
            if (attackEffect != null)
            {
                attackEffect.transform.position = currentUnit.transform.position;
            }
            currentUnit.CommonStatus.DecreaseHp(commonStatus.CurrentAttackDamage);
        }
        else
        {
            if (attackEffect != null)
            {
                attackEffect.transform.position = InGameManager.Instance.Player.transform.position;
            }
            InGameManager.Instance.Player.DecreaseHp(commonStatus.CurrentAttackDamage);
        }
    }
    private IEnumerator Co_UpdateState() // 보스 유한상태기계
    {
        yield return new WaitForSeconds(3f);
        while (eBossKind != EBossKind.Dummy)
        {
            yield return null;
            if (isPattern)
            {
                Debug.Log("패턴 중");
                continue;
            }
            switch (eBossState)
            {
                case EBossState.Idle:
                    yield return new WaitForSeconds(commonStatus.AttackSpeed);
                    eBossState = EBossState.Attack;
                    break;
                case EBossState.Attack:
                    if (eBossKind == EBossKind.Gargoyle)
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "grondpunch", false);
                    }
                    else
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
                    }
                    if (eBossKind != EBossKind.Mushroom)
                    {
                        skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, attackAnimDelay);
                    }
                    else
                    {
                        skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, attackAnimDelay);
                    }
                    if (attackEffect != null)
                    {
                        attackEffect.Play();
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
    private void Awake()//임시로 초기화
    {
        for (int i = 0; i < arrayCount; i++)
        {
            collisionsArray.Add(new List<GameObject>());
        }
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if(eBossKind != EBossKind.Mushroom)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }
        else
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        }
        //GetComponentInChildren<Canvas>().renderMode = RenderMode.
    }
    private void Start()
    {
        commonStatus.CurrentHp = commonStatus.MaxHp;
        commonStatus.CurrentAttackDamage = commonStatus.AttackDamage;
        currentUnit = InGameManager.Instance.Units[0];
        //StartCoroutine(Co_UpdateUnitReference());
        StartCoroutine(Co_UpdateState());
    }
    private void Update()
    {
        if (eBossKind == EBossKind.Dummy) return;
        if(commonStatus.CurrentHp <= 0)
        {
            if(eBossKind == EBossKind.Gargoyle)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "dead", false);
            }
            else
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Death", false);
            }
            InGameManager.Instance.SetGameClear();
            this.enabled = false;
        }        
    }
}
