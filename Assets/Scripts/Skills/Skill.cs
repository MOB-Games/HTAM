using System;
using System.Collections.Generic;
using Core.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class StatsForSkill
{
    public StatType attackStat;
    public StatType defenseStat;
    public StatType affectedStat;
}

[Serializable]
public class SkillParameters
{
    // variables for calculating if skill hits or misses
    public int baseHitChance;
    public double hitMultiplier;
    public double missMultiplier;
    
    // variables for calculating effect of skill
    public int baseEffectValue;
    public double attackMultiplier;
    public double defenseMultiplier;
}

public class SkillResult
{
    public readonly SkillEffect Effect;
    public readonly StatType AffectedStat;
    public readonly int Delta;

    public SkillResult(SkillEffect effect, StatType affectedStat = StatType.Hp, int delta = 0)
    {
        Effect = effect;
        AffectedStat = affectedStat;
        Delta = delta;
    }
}

public class Skill : MonoBehaviour
{
    public SkillId id;
    public string skillName;
    public bool melee;
    public uint energyCost;
    public uint hpCost;
    [HideInInspector]
    public int level;
    public TargetType targetType;
    public SkillEffect effect;
    public StatsForSkill statsForSkill;
    public List<SkillParameters> parametersPerLevel;

    private void OnValidate()
    {
        if (level >= parametersPerLevel.Count)
            throw new ArgumentOutOfRangeException(
                $"Tried to use skill {id} with level {level}, but it has a max level of {parametersPerLevel.Count - 1}");
    }

    public SkillResult GetResult(CombatantId attackerId, CombatantId defenderId)
    {
        var attackerStats = CombatantInfo.GetStatBlock(attackerId);
        var defenderStats = CombatantInfo.GetStatBlock(defenderId);
        var chanceToHit = parametersPerLevel[level].baseHitChance + parametersPerLevel[level].hitMultiplier * attackerStats.speed.value -
                          parametersPerLevel[level].missMultiplier * defenderStats.speed.value;
        if (Random.Range(0, 100) > chanceToHit)
            return new SkillResult(SkillEffect.Miss);

        var delta = parametersPerLevel[level].baseEffectValue +
                    parametersPerLevel[level].attackMultiplier * attackerStats.GetStatValue(statsForSkill.attackStat) -
                    parametersPerLevel[level].defenseMultiplier * defenderStats.GetStatValue(statsForSkill.defenseStat);

        if (effect == SkillEffect.Hurt)
        {
            delta = -delta; // should reduce stat
            if (delta > 0)
                delta = 0; // should never help
        }
        else
        {
            if (delta < 0)
                delta = 0; // should never hurt
        }
        
        return new SkillResult(effect, statsForSkill.affectedStat, (int)delta);
    }
    
    
}
