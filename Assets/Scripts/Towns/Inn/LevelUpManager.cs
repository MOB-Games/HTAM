using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    private static readonly List<int> ExpForLevel = new List<int>() { 0, 40 };

    private int _statPtsPerLevel = 3;
    private int _vitalityPtsPerLevel = 2;
    private int _skillPtsPerLevel = 3;

    private static int ConvertExpToLevel(int exp)
    {
        for (var i = 0; i < ExpForLevel.Count - 1; i++)
        {
            if (ExpForLevel[i] <= exp && exp < ExpForLevel[i + 1])
                return i;
        }

        return ExpForLevel.Count - 1;
    }

    private static int LevelsToProgress(int level, int exp)
    {
        return ConvertExpToLevel(exp) - level;
    }
}
