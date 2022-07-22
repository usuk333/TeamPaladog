using System.Collections;
using UnityEngine;

//패턴 충돌처리 로직 수정해야함 (기존 PatternCollision -> CollisionArray[n] 으로)
public class Beast : MonoBehaviour
{
    private bool isCrush;
    private bool isShout;
    [SerializeField] private BossUtility bossUtility;
    [SerializeField] private float[] crushDamages;
    [SerializeField] private GameObject[] crushs;
    [SerializeField] private float[] conditionHp;

    private void Shouting()
    {
        bossUtility.KnockBack();
        isShout = true;
        InGameManager.Instance.Boss.skeletonAnimation.AnimationState.SetAnimation(0, "Backattack", false);
        InGameManager.Instance.Boss.skeletonAnimation.AnimationState.AddAnimation(0, "Idle", false, 6.6f);
    }
    private IEnumerator Co_Crush()
    {
        while (true)
        {
            if (isCrush)
            {
                InGameManager.Instance.Boss.skeletonAnimation.AnimationState.SetAnimation(0, "FrontAttack", false);
                InGameManager.Instance.Boss.skeletonAnimation.AnimationState.AddAnimation(0, "Idle", false, 4);
                InGameManager.Instance.Boss.isPattern = true;
                /* float rand = Random.Range(x, y); 패턴 반복 시간 (x~y 사이의 랜덤한 값마다 반복, 랜덤 값은 반복 할 때마다 변경됨)
                yield return new WaitForSeconds(rand);*/
                yield return new WaitForSeconds(2f);
                crushs[1].SetActive(true);
                yield return new WaitForSeconds(1f);
                Crush();
                crushs[1].SetActive(false);
                InGameManager.Instance.Boss.CollisionsArray[1].Clear();
                yield return new WaitForSeconds(1f);
                isCrush = false;
                InGameManager.Instance.Boss.isPattern = false;
            }
            yield return null;
        }
    }
    private IEnumerator Co_KnockBackCrush()
    {
        while (true)
        {
            if (isShout)
            {
                InGameManager.Instance.Boss.isPattern = true;
                crushs[0].SetActive(true);
                yield return new WaitForSeconds(5.6f);
                KnockBackCrush();
                crushs[0].SetActive(false);
                InGameManager.Instance.Boss.CollisionsArray[0].Clear();
                yield return new WaitForSeconds(1f);
                isShout = false;
                InGameManager.Instance.Boss.isPattern = false;
            }
            yield return null;
        }      
    }
    private IEnumerator Co_CheckHp()
    {
        for (int i = 0; i < conditionHp.Length; i++)
        {
            yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= conditionHp[i]);
            InGameManager.Instance.Boss.CommonStatus.CurrentAttackDamage = InGameManager.Instance.Boss.CommonStatus.CurrentAttackDamage * 1.2f;
            InGameManager.Instance.Boss.CommonStatus.AttackSpeed = InGameManager.Instance.Boss.CommonStatus.AttackSpeed / 1.1f;
        }
    }
    private void Crush()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[1].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(crushDamages[1]);
            }
        }
        if (InGameManager.Instance.Boss.CollisionsArray[1].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(crushDamages[1]);
        }
    }
    private void KnockBackCrush()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(crushDamages[0]);
            }
        }
        if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(crushDamages[0]);
        }
    }

    private void Start()
    {
        StartCoroutine(Co_KnockBackCrush());
        StartCoroutine(Co_Crush());
        StartCoroutine(Co_CheckHp());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shouting();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            isCrush = true;
        }
    }
}
