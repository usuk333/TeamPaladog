using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Player player;
    [SerializeField] private Image hpBar; //플레이어 Hp바 이미지
    [SerializeField] private Image mpBar; //플레이어 Mp바 이미지
    [SerializeField] private Image shield;
    [SerializeField] private Image castingBar;
    public void DisableCastingBar()
    {
        castingBar.transform.parent.gameObject.SetActive(false);
        castingBar.fillAmount = 0;
    }
    public void ActiveCastingBar()
    {
        castingBar.transform.parent.gameObject.SetActive(true);
    }      
    public void UpdateCastingBar(float time, float progress)
    {
        castingBar.fillAmount = 1 / time * progress;
    }
    private IEnumerator Co_UpdatePlayerUI() //플레이어 UI 플레이어 정보에 맞게 갱신
    {
        while (true)
        {
            hpBar.fillAmount = 1 / player.MaxHp * player.CurrentHp;
            mpBar.fillAmount = 1 / player.MaxMp * player.CurrentMp;
            shield.fillAmount = 1 / player.Shield * player.CurrentShield;
            yield return null;
        }
    }
    private void Awake() //해당 부분도 이니셜라이징 함수로 빼야할까?
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdatePlayerUI());
        //StartCoroutine(Co_UpdateCastingBar());
    }


}
