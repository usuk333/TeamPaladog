using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public static class DataEquation
{
    private static int unitSize = 10000;
    private static string[] units = { null, "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극" };

    private static (int value, int idx, int point) GetSize(BigInteger value) // 빅인티저로 받아온 값을 계산해 만 단위마다 단위 표기를 바꿔줌
    {
        var currentValue = value;
        var idx = 0;
        var lastValue = 0;
        while (currentValue > unitSize - 1)
        {
            var predCurrentValue = currentValue / unitSize;
            if (predCurrentValue <= unitSize - 1) lastValue = (int)currentValue;
            currentValue = predCurrentValue;
            idx++;
        }
        int point = (lastValue % 10000) / 1000;
        return ((int)currentValue, idx, point);
    }
    public static string GetUnit(BigInteger value) // 숫자를 단위 표기로 바꿀 때 호출, 알맞는 단위값을 리턴함
    {
        var sizeStruct = GetSize(value);
        return $"{sizeStruct.value}.{sizeStruct.point}{units[sizeStruct.idx]}";
    }
    public static int UnitMaxAtkEquationToLevel(string unitName)
    {
        int a;
        int level = System.Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{unitName}Level"]);
        switch (unitName)
        {
            case "Warrior":
                a = 380;
                a = a + (16 * level);
                break;
            case "Assassin":
                a = 400;
                a = a + (16 * level);
                break;
            case "Magician":
                a = 400;
                a = a + (24 * level);
                break;
            case "Archor":
                a = 380;
                a = a + (20 * level);
                break;
            default:
                a = 0;
                break;
        }
        return a;
    }
    public static int UnitMaxHpEquationToLevel(string unitName)
    {
        int a;
        int level = System.Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{unitName}Level"]);
        switch (unitName)
        {
            case "Warrior":
                a = 1200;
                a = a + (150 * level);
                break;
            case "Assassin":
                a = 1000;
                a = a + (100 * level);
                break;
            case "Magician":
                a = 800;
                a = a + (80 * level);
                break;
            case "Archor":
                a = 800;
                a = a + (80 * level);
                break;
            default:
                a = 0;
                break;
        }
        return a;
    }
    public static int UnitSkillConditionToLevel(string unitName)
    {
        int a;
        int level = System.Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{unitName}Level"]);
        int count = level / 10;
        switch (unitName)
        {
            case "Warrior":
                a = 10;
                a = a - count;
                break;
            case "Assassin":
                a = 10;
                a = a + (4 * count);
                break;
            case "Magician":
                a = 10;
                a = a - count;
                break;
            case "Archor":
                a = 10;
                a = a + (6 * count);
                break;
            default:
                a = 0;
                break;
        }
        return a;
    }
}
