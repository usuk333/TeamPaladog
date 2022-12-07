using System;
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
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{unitName}Level"]) - 1;
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
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{unitName}Level"]) - 1;
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
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{unitName}Level"]);
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
    public static (int,int,int) PlayerSkillAttackToLevel()
    {
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["Attack"]) - 1;
        int apple = 200 + (20 * level);
        int rock = 250 + (20 * level); 
        int bomb = 300 + (20 * level);

        return (apple,rock,bomb);
    }
    public static (float, float) PlayerSkillRageToLevel()
    {
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["PowerUp"]) - 1;
        float value = 3 + (0.4f * level);
        float mana = 6 + (0.8f * level);

        return (value, mana);
    }
    public static (int, float, int, int) PlayerSkillBarriorToLevel()
    {
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["Barrior"]) - 1;
        int value = 50 + (10 * level);
        float duration = 3 + (0.4f * level);
        int mana = 50 + (4 * level);
        int cool = 15 - (1 * level);

        return (value, duration, mana, cool);
    }
    public static (int, int, int) PlayerSkillHealToLevel()
    {
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["Heal"]) - 1;
        int value = 20 + (4 * level);
        int mana = 20 + (2 * level);
        int cool = 10 - (1 * level);

        return (value, mana, cool);
    }
}
