using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public void BtnEvt_UseSkill(int index)
    {
        InGameManager.Instance.Player.UseSkill(index);
    }
}
