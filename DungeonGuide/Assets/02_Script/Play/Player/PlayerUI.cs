using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUI : MonoBehaviour
{
    private Player player;
    [SerializeField] private Image hpBar; //�÷��̾� Hp�� �̹���
    [SerializeField] private Image mpBar; //�÷��̾� Mp�� �̹���
    [SerializeField] private Image shield;
    [SerializeField] private Image castingBar;

    [SerializeField] private Image[] skillImageAray;
    [SerializeField] private GameObject[] skillCantImageArray;
    public void ShowSkillCoolTime(int index, float value)
    {
        Debug.Log("��ų��Ÿ�� �Լ� ȣ��");
        skillImageAray[index].fillAmount = 1;
        skillImageAray[index].DOFillAmount(0, value);
    }
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
            shield.fillAmount = 1 / player.Shield * player.CurrentShield;
            yield return null;
        }
    }
    public void ActiveCantImage(bool isActive)
    {
        for (int i = 0; i < skillCantImageArray.Length; i++)
        {
            skillCantImageArray[i].SetActive(isActive);
        }
    }
    private void Awake() //�ش� �κе� �̴ϼȶ���¡ �Լ��� �����ұ�?
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdatePlayerUI());
    }


}
