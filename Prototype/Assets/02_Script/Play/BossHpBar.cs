using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    private Image hpBar; //보스 Hp바 이미지
    private Boss boss; //보스 클래스 레퍼런스
    private IEnumerator Co_UpdateBossHpBar() // 보스 Hp바를 갱신해줄 코루틴. Hp 변화가 많은 전투를 고려했을 때 코루틴으로 사용하는게 맞는 듯
    {
        while (true)
        {
            hpBar.fillAmount = 1 / boss.CommonStatus.MaxHp * boss.CommonStatus.CurrentHp;
            yield return null;
        }
    }
    private void Awake() //해당 부분도 이니셜라이징 함수로 빼야할까?
    {
        hpBar = GetComponent<Image>();
        boss = GetComponentInParent<Boss>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateBossHpBar());
    }
}
