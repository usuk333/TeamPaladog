using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour //인게임에서 플레이어와 상호작용하는 버튼들의 기능 스크립트
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
