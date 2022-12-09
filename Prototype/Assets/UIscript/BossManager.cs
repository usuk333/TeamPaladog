using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BossManager
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    public static GameObject BossSection;
    public static Difficulty difficulty;

    public static UnitStatus tanker;
    public static UnitStatus warrior;
    public static UnitStatus mage;
    public static UnitStatus archer;
    public static Passive hp;
    public static Passive mp;
    public static Barrior barrior;
    public static Heal heal;
    public static PowerUp powerUp;
    public static Attack attack;
    public static AudioClip bgm;

}
