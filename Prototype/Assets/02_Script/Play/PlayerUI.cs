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
    public void ActiveCastingBar()
    {
        castingBar.transform.parent.gameObject.SetActive(true);
        //star
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
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdatePlayerUI());
    }

}
