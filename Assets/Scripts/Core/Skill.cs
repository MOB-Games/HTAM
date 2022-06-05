using System;
using System.Collections.Generic;
using System.Data;
using Core.Enums;
using JetBrains.Annotations;
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
    public readonly bool Hit;
    public readonly bool AnimateAttacked;
    public readonly StatType AffectedStat;
    public readonly int Delta;
    [CanBeNull] public readonly GameObject VisualEffect;

    public SkillResult()
    {
        Hit = false;
        AnimateAttacked = false;
        AffectedStat = StatType.Hp;
        Delta = 0;
    }

    public SkillResult(bool animateAttacked,  StatType affectedStat, int delta, [CanBeNull] GameObject visualEffect = null)
    {
        Hit = true;
        AnimateAttacked = animateAttacked;
        AffectedStat = affectedStat;
        Delta = delta;
        VisualEffect = visualEffect;
    }
}

public class Skill : MonoBehaviour
{
    public SkillId id;
    public string skillName;
    public string description;
    public bool melee;
    public bool offensive;
    public bool animateAttacked;
    public int energyCost;
    public int hpCost;
    [HideInInspector]
    public TargetType targetType;
    public SkillAnimation skillAnimation;
    public StatsForSkill statsForSkill;
    public List<SkillParameters> parametersPerLevel;
    [CanBeNull] public GameObject visualEffect;

    private void OnValidate()
    {
        if (targetType != TargetType.Single && melee)
            throw new ConstraintException("A skill cannot be melee if it targets more than 1 target");
    }

    public string GetDescription()
    {
        return $"{skillName}\n" +
               $"{description}\n\n" +
               $"Energy Cost: {energyCost}\n" +
               $"Hp Cost: {hpCost}";
    }

    public SkillResult GetResult(CombatantId attackerId, CombatantId defenderId, int level)
    {
        if (level >= parametersPerLevel.Count)
            throw new ArgumentOutOfRangeException(
                $"Tried to use skill {id} with level {level}, but it has a max level of {parametersPerLevel.Count - 1}");
        var attackerStats = CombatantInfo.GetStatBlock(attackerId);
        var defenderStats = CombatantInfo.GetStatBlock(defenderId);
        var chanceToHit = parametersPerLevel[level].baseHitChance + parametersPerLevel[level].hitMultiplier * attackerStats.speed.value -
                          parametersPerLevel[level].missMultiplier * defenderStats.speed.value;
        if (Random.Range(0, 100) > chanceToHit)
            return new SkillResult();

        var delta = parametersPerLevel[level].baseEffectValue +
                    parametersPerLevel[level].attackMultiplier * attackerStats.GetStatValue(statsForSkill.attackStat) -
                    parametersPerLevel[level].defenseMultiplier * defenderStats.GetStatValue(statsForSkill.defenseStat);

        if (offensive)
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
        
        return new SkillResult(animateAttacked, statsForSkill.affectedStat, (int)delta, visualEffect);
    }
}
