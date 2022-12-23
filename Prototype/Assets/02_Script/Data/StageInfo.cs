using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageInfo
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    public static int bossIndex;
    public static Difficulty difficulty;

    public static int GetBossBGM()
    {
        return bossIndex + 3;
    }
}
