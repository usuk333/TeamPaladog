using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public enum LoginType
    {
        None = 0,
        anony = 1,
        google = 2
    }
    public string Nickname;
    public int Gold;
    public int Level;
    public int EXP;
    public float Speed;
    public float HP;
    public float MP;
    public float ATKPower;
    public string UID;

    public int SkillPoints;
    public int UnitPoints;

    public int[,] Skill = new int[7, 2];

    public int[,] Unit = new int[8, 2];

    public int[,] Stage = new int[6, 3];
}

public class PlayerDataManager : MonoBehaviour
{
    public PlayerData[] Playerdatas;
    public PlayerData.LoginType[] playerlogintype;
}
