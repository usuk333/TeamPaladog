using System.Collections;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;

//패턴 충돌처리 로직 수정해야함 (기존 PatternCollision -> CollisionArray[n] 으로)
public class Beast : MonoBehaviour
{
    private bool isCrush;
    private bool isShout;
    [SerializeField] private BossUtility bossUtility;
    [SerializeField] private GameObject[] crushs;
    [SerializeField] private ParticleSystem backEffect;
    [SerializeField] private ParticleSystem frontEffect;
    [SerializeField] private Image warningImage;
    [SerializeField] private Text warningText;
    
    public BeastStatus beastStatus;

    private void Start()
    {
        StartCoroutine(Co_FirstPattern());
        StartCoroutine(Co_SecondPattern());
        StartCoroutine(Co_ForthPattern());
        int rand = Random.Range(0, 2);
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
    private void FirstPattern()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(beastStatus.firstPatternDamage);
            }
        }
        if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(beastStatus.firstPatternDamage);
        }
    }
    private void SecondPattern()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[1].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(beastStatus.secondPatternDamage);
            }
        }
        if (InGameManager.Instance.Boss.CollisionsArray[1].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(beastStatus.secondPatternDamage);
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
                rand = Random.Range(beastStatus.firstPatternMinTime, beastStatus.firstPatternMaxTime);
                print(rand);
                yield return new WaitForSeconds(rand);
                Shouting();
                InGameManager.Instance.Boss.IsPattern = true;
                crushs[0].SetActive(true);
                yield return new WaitForSeconds(5.6f);
                FirstPattern();
                crushs[0].SetActive(false);
                InGameManager.Instance.Boss.CollisionsArray[0].Clear();
                yield return new WaitForSeconds(1f);
                isShout = false;
                InGameManager.Instance.Boss.IsPattern = false;
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
                rand = Random.Range(beastStatus.secondPatternMinTime, beastStatus.secondPatternMaxTime);
                print(rand);
                yield return new WaitForSeconds(rand);
                InGameManager.Instance.Boss.skeletonAnimation.AnimationState.SetAnimation(0, "FrontAttack", false);
                InGameManager.Instance.Boss.skeletonAnimation.AnimationState.AddAnimation(0, "Idle", false, 4);
                InGameManager.Instance.Boss.IsPattern = true;
                frontEffect.Play();
                yield return new WaitForSeconds(2f);
                crushs[1].SetActive(true);
                yield return new WaitForSeconds(1f);
                SecondPattern();
                crushs[1].SetActive(false);
                InGameManager.Instance.Boss.CollisionsArray[1].Clear();
                yield return new WaitForSeconds(1f);
                isCrush = false;
                InGameManager.Instance.Boss.IsPattern = false;
                yield return new WaitForSeconds(5f);
                isShout = true;
            }
            yield return null;
        }
    }
    private IEnumerator Co_ForthPattern()
    {
        float rand = Random.Range(beastStatus.forthPatternPercentage - 5, beastStatus.forthPatternPercentage + 5);
        yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= InGameManager.Instance.Boss.CommonStatus.MaxHp * rand / 100);
        warningImage.gameObject.SetActive(true);
        warningImage.DOFade(1, 2f);
        warningText.DOFade(1, 2f);
        yield return new WaitForSeconds(2f);
        InGameManager.Instance.Boss.skeletonAnimation.Skeleton.SetColor(new Color(1f, 0.8f, 0.8f));
        warningImage.DOFade(0, 1f);
        warningText.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        warningImage.gameObject.SetActive(false);
        InGameManager.Instance.Boss.CommonStatus.CurrentAttackDamage = InGameManager.Instance.Boss.CommonStatus.CurrentAttackDamage * beastStatus.forthPatternAtkValue;
        InGameManager.Instance.Boss.CommonStatus.AttackSpeed = InGameManager.Instance.Boss.CommonStatus.AttackSpeed / beastStatus.forthPatternAtksValue;
    }
}
