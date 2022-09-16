using System.Collections;
using UnityEngine;
using Spine.Unity;

//패턴 충돌처리 로직 수정해야함 (기존 PatternCollision -> CollisionArray[n] 으로)
public class Beast : MonoBehaviour
{
    private bool isCrush;
    private bool isShout;
    [SerializeField] private BossUtility bossUtility;
    [SerializeField] private GameObject[] crushs;
    [SerializeField] private ParticleSystem backEffect;
    [SerializeField] private ParticleSystem frontEffect;

    public float firstPatternDamage;
    public float firstPatternMinTime;
    public float firstPatternMaxTime;
    public float secondPatternDamage;
    public float secondPatternMinTime;
    public float secondPatternMaxTime;
    public float thirdPatternDamage;
    public float thirdPatternMinTime;
    public float thirdPatternMaxTime;
    public float forthPatternPercentage;
    public float forthPatternAtkValue;
    public float forthPatternAtksValue;

    private Color c = new Color(1f, 0.8f, 0.8f);

    private void Start()
    {
        StartCoroutine(Co_FirstPattern());
        StartCoroutine(Co_SecondPattern());
        StartCoroutine(Co_ForthPattern());
        int rand = Random.Range(0, 2);
        print(rand);
        switch (rand)
        {
            case 0:
                isShout = true;
                break;
            case 1:
                isCrush = true;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Shouting();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isCrush = true;
        }
    }
    private void FirstPattern()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(firstPatternDamage);
            }
        }
        if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(firstPatternDamage);
        }
    }
    private void SecondPattern()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[1].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(secondPatternDamage);
            }
        }
        if (InGameManager.Instance.Boss.CollisionsArray[1].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(secondPatternDamage);
        }
    }
    private void Shouting()
    {
        bossUtility.KnockBack();
        backEffect.Play();
        InGameManager.Instance.Boss.skeletonAnimation.AnimationState.SetAnimation(0, "Backattack", false);
        InGameManager.Instance.Boss.skeletonAnimation.AnimationState.AddAnimation(0, "Idle", false, 6.6f);
    }
    private IEnumerator Co_FirstPattern()
    {
        float rand;
        while (true)
        {
            if (isShout)
            {
                rand = Random.Range(firstPatternMinTime, firstPatternMaxTime);
                print(rand);
                yield return new WaitForSeconds(rand);
                Shouting();
                InGameManager.Instance.Boss.isPattern = true;
                crushs[0].SetActive(true);
                yield return new WaitForSeconds(5.6f);
                FirstPattern();
                crushs[0].SetActive(false);
                InGameManager.Instance.Boss.CollisionsArray[0].Clear();
                yield return new WaitForSeconds(1f);
                isShout = false;
                InGameManager.Instance.Boss.isPattern = false;
                yield return new WaitForSeconds(5f);
                isCrush = true;
            }
            yield return null;
        }
    }
    private IEnumerator Co_SecondPattern()
    {
        float rand;
        while (true)
        {
            if (isCrush)
            {
                rand = Random.Range(secondPatternMinTime, secondPatternMaxTime);
                print(rand);
                yield return new WaitForSeconds(rand);
                InGameManager.Instance.Boss.skeletonAnimation.AnimationState.SetAnimation(0, "FrontAttack", false);
                InGameManager.Instance.Boss.skeletonAnimation.AnimationState.AddAnimation(0, "Idle", false, 4);
                InGameManager.Instance.Boss.isPattern = true;
                frontEffect.Play();
                yield return new WaitForSeconds(2f);
                crushs[1].SetActive(true);
                yield return new WaitForSeconds(1f);
                SecondPattern();
                crushs[1].SetActive(false);
                InGameManager.Instance.Boss.CollisionsArray[1].Clear();
                yield return new WaitForSeconds(1f);
                isCrush = false;
                InGameManager.Instance.Boss.isPattern = false;
                yield return new WaitForSeconds(5f);
                isShout = true;
            }
            yield return null;
        }
    }
    private IEnumerator Co_ForthPattern()
    {
        float rand = Random.Range(forthPatternPercentage - 5, forthPatternPercentage + 5);
        print("체력 퍼센트는" + rand + ", 체력 실제 수치는" + InGameManager.Instance.Boss.CommonStatus.CurrentHp * rand / 100);
        yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= InGameManager.Instance.Boss.CommonStatus.CurrentHp * rand / 100);
        InGameManager.Instance.Boss.skeletonAnimation.Skeleton.SetColor(c);
        InGameManager.Instance.Boss.CommonStatus.CurrentAttackDamage = InGameManager.Instance.Boss.CommonStatus.CurrentAttackDamage * forthPatternAtkValue;
        InGameManager.Instance.Boss.CommonStatus.AttackSpeed = InGameManager.Instance.Boss.CommonStatus.AttackSpeed / forthPatternAtksValue;
    }
    public void InitStatus(BeastStatus bs)
    {
        firstPatternDamage = bs.firstPatternDamage;
        firstPatternMinTime = bs.firstPatternMinTime;
        firstPatternMaxTime = bs.firstPatternMaxTime;
        secondPatternDamage = bs.secondPatternDamage;
        secondPatternMinTime = bs.secondPatternMinTime;
        secondPatternMaxTime = bs.secondPatternMaxTime;
        thirdPatternDamage = bs.thirdPatternDamage;
        thirdPatternMinTime = bs.thirdPatternMinTime;
        thirdPatternMaxTime = bs.thirdPatternMaxTime;
        forthPatternPercentage = bs.forthPatternPercentage;
        forthPatternAtkValue = bs.forthPatternAtkValue;
        forthPatternAtksValue = bs.forthPatternAtksValue;
        var fallingObj = GetComponentInChildren<FallingObjectPool>();
        fallingObj.damage = thirdPatternDamage;
        fallingObj.spawnIntervalMin = thirdPatternMinTime;
        fallingObj.spawnIntervalMax = thirdPatternMaxTime;
    }
    public void Test_BtnEvt_Shout()
    {
        Shouting();
    }
    public void Test_BtnEvt_Crush()
    {
        isCrush = true;
    }
}
