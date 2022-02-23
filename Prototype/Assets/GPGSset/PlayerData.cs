using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Gold;
    public int EXP;
    public string Nickname;
    public string UID;
    public string LastLogin;

    public int SkillPoints;
    public int UnitPoints;

    public int[,] Skill = new int[7, 2];

    public int[,] Unit = new int[8, 2];
}
