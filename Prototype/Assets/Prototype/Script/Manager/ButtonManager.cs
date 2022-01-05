using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private InGameManager inGameManager;
    private Player player;

    private void Awake()
    {
        inGameManager = GetComponent<InGameManager>();
        player = FindObjectOfType<Player>();
    }
    public void BtnEvt_InstantiateUnit(int index)
    {
        inGameManager.InstantiateUnit(index);
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
}
