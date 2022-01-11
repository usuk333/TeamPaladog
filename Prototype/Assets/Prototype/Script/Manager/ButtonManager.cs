using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour //�ΰ��ӿ��� �÷��̾�� ��ȣ�ۿ��ϴ� ��ư���� ��� ��ũ��Ʈ
{
    private Player player;
    public void BtnEvt_InstantiateUnit(int index)
    {
        InGameManager.Instance.InstantiateUnit(index);
    }
    public void BtnEvt_UseSkill(int index)
    {
        player.UseSkill(index);
    }
    public void BtnEvt_Left()
    {
        player.IsLeft = !player.IsLeft;
    }
    public void BtnEvt_Right()
    {
        player.IsRight = !player.IsRight;
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
}
