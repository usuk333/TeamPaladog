using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Player player;
    [SerializeField] private Image hpBar; //�÷��̾� Hp�� �̹���
    [SerializeField] private Image mpBar; //�÷��̾� Mp�� �̹���
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
    private IEnumerator Co_UpdatePlayerUI() //�÷��̾� UI �÷��̾� ������ �°� ����
    {
        while (true)
        {
            hpBar.fillAmount = 1 / player.MaxHp * player.CurrentHp;
            mpBar.fillAmount = 1 / player.MaxMp * player.CurrentMp;
            yield return null;
        }
    }
    private void Awake() //�ش� �κе� �̴ϼȶ���¡ �Լ��� �����ұ�?
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdatePlayerUI());
        //StartCoroutine(Co_UpdateCastingBar());
    }


}
