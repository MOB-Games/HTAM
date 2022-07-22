using System;
using System.Collections.Generic;
using System.Data;
using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.SkillsAndConditions
{
    [Serializable]
    public class SkillParameters
    {
        [Range(0,100)]
        public int accuracy;
        
        // variables for calculating effect of skill
        public int baseEffectValue;
        public double attackMultiplier;
        public double defenseMultiplier;
        
        [Range(0,100)]
        public int chanceToInflict;
    }

    public class Skill : SkillBase
    {
        [Multiline]
        public string description;
        public bool melee;
        public bool offensive;
        public bool animateAttacked;
        public bool speedBasedAccuracy;
        public bool speedBasedEvasion;
        public bool remainingEnergySkill;
        public bool deficitSkill;
        public int energyCost;
        public int hpCost;
        public TargetType targetType;
        public SkillAnimation skillAnimation;
        [Header("Stats For Skill")]
        public StatType attackStat;
        [EnumOrder("2,3,4,5")]
        public StatType defenseStat;
        [EnumOrder("0,1,5")]
        public StatType affectedStat;
        public List<SkillParameters> parametersPerLevel;
        [CanBeNull] public GameObject visualEffect;
        [CanBeNull] public GameObject conditionGo;
        [CanBeNull] public ConditionRemover conditionRemover;

        [CanBeNull] private Condition _condition;

        private void OnValidate()
        {
            if (targetType != TargetType.Single && melee)
                throw new ConstraintException($"{name}: Skill cannot be melee if it targets more than 1 target");
            if (parametersPerLevel.Count == 0)
                throw new ConstraintException($"{name}: Skill must have at least 1 level");
            if (parametersPerLevel.Exists(p => p.chanceToInflict > 0) && conditionGo == null)
                throw new ConstraintException($"{name}: Skill can't have a chance to inflict but not have a condition to inflict");
            if (energyCost > 0 && hpCost > 0)
                throw new ConstraintException($"{name}: Skill can't cost both energy and hp");
            if (conditionGo != null && conditionGo.GetComponent<Condition>().parametersPerLevel.Count <
                parametersPerLevel.Count)
                throw new ConstraintException($"{name}: Mismatch between skill levels and condition levels");
            if (remainingEnergySkill && deficitSkill)
                throw new ConstraintException($"{name}: Skill can't be both deficit based and remaining energy based");
            if (remainingEnergySkill && energyCost > 0)
                throw new ConstraintException($"{name}: Remaining energy skill can't have an energy cost");
        }

        private void Awake()
        {
            if (conditionGo != null)
                _condition = conditionGo.GetComponent<Condition>();
        }

        public override string GetDescription(int level)
        {
            if (_condition == null && conditionGo != null)
                _condition = conditionGo.GetComponent<Condition>();
            var desc = $"<u>{name.Split('(')[0]} (lvl.{level})</u>: {description}\n";
            if (affectedStat != StatType.None)
            {
                desc += offensive ? "Attacks " : "Heals ";
                desc += targetType switch
                {
                    TargetType.Single => "the target",
                    TargetType.Group => "the group",
                    TargetType.All => "everyone",
                    TargetType.Self => "self",
                    _ => throw new ArgumentOutOfRangeException()
                };
                if (affectedStat == StatType.Energy)
                    desc += "s Energy";
                desc += offensive ? " With " : " by ";
                var baseValue = Math.Abs(parametersPerLevel[level].baseEffectValue);
                var attackerPercent = Math.Abs(parametersPerLevel[level].attackMultiplier) * 100;
                var defensePercent = Math.Abs(parametersPerLevel[level].defenseMultiplier) * 100;
                if (baseValue != 0)
                    desc += $"{baseValue} ";
                if (baseValue != 0 && attackerPercent != 0)
                    desc += "+ ";
                if ((attackStat != StatType.None && attackerPercent != 0) || remainingEnergySkill)
                {
                    desc += $"{attackerPercent}% of ";
                    if (deficitSkill)
                        desc += "the damage done to ";
                    desc += $"users {attackStat} ";
                    if (remainingEnergySkill)
                        desc += "in addiotion to all the users remaining energy";
                }
                if (defenseStat != StatType.None && defensePercent != 0)
                {
                    if (parametersPerLevel[level].defenseMultiplier > 0)
                        desc += "opposed";
                    else
                        desc += "in addition";
                    desc += $" to {defensePercent}% of targets {defenseStat}\n";
                }
                else if (offensive)
                    desc += "while ignoring the targets defenses";
            }

            if (_condition != null)
            {
                desc += "Has a chance to inflict ";
                desc += _condition switch
                {
                    TurnSkipCondition turnSkipCondition => turnSkipCondition.GetDescription(level),
                    SilenceCondition silenceCondition => silenceCondition.GetDescription(level),
                    _ => _condition.GetDescription(level)
                };
                desc += "\n";
            }
            if (energyCost > 0)
                desc += $"\nEnergy Cost: {energyCost}\n";
            if (hpCost > 0)
                desc += $"\nHP Cost: {hpCost}\n";
            return desc;
        }

        private static int MultiplierToPercent(double multiplier)
        {
            return (int)(Math.Abs(multiplier) * 100);
        }

        public override string GetLevelupDescription(int level)
        {
            if (level == -1)
            {
                return "<u>First Level</u>:\n" + GetDescription(0);
            }
            var desc = GetDescription(level);
            if (level == parametersPerLevel.Count - 1)
                return desc + "\n\n<b>Level Maxed</b>";
            desc += "\n\n<u>Next Level</u>:\n";
            var currentParams = parametersPerLevel[level];
            var nextParams = parametersPerLevel[level + 1];
            if (currentParams.accuracy != nextParams.accuracy)
                desc += $"Accuracy: {currentParams.accuracy} -> {nextParams.accuracy}\n";
            if (Math.Abs(currentParams.baseEffectValue - nextParams.baseEffectValue) > 0.0001)
                desc += $"Fixed Change: {currentParams.baseEffectValue} -> {nextParams.baseEffectValue}\n";
            if (Math.Abs(currentParams.attackMultiplier - nextParams.attackMultiplier) > 0.0001)
                desc +=
                    $"User Percent: {MultiplierToPercent(currentParams.attackMultiplier)}% -> {MultiplierToPercent(nextParams.attackMultiplier)}%\n";
            if (Math.Abs(currentParams.defenseMultiplier - nextParams.defenseMultiplier) > 0.0001)
                desc +=
                    $"Target Percent: {MultiplierToPercent(currentParams.attackMultiplier)}% -> {MultiplierToPercent(nextParams.attackMultiplier)}%\n";
            if (Math.Abs(currentParams.chanceToInflict - nextParams.chanceToInflict) > 0.0001)
                desc += $"Chance to inflict condition: {currentParams.chanceToInflict}% -> {nextParams.chanceToInflict}%\n";

            if (_condition != null)
            {
                desc += "\nCondition:\n";
                desc += "Has a chance to inflict ";
                desc += _condition switch
                {
                    TurnSkipCondition turnSkipCondition => turnSkipCondition.GetLevelupDescription(level),
                    SilenceCondition silenceCondition => silenceCondition.GetLevelupDescription(level),
                    _ => _condition.GetLevelupDescription(level)
                };
                desc += "\n";
            }

            return desc;
        }
        
        public override int GetMaxLevel()
        {
            return parametersPerLevel.Count - 1;
        }

        public SkillResult GetResult(CombatantId attackerId, CombatantId defenderId, int level)
        {
            if (level >= parametersPerLevel.Count)
                throw new ArgumentOutOfRangeException(
                    $"Tried to use skill {name} with level {level}, but it has a max level of {parametersPerLevel.Count - 1}");
            var attackerStats = CombatantInfo.GetStatBlock(attackerId);
            var defenderStats = CombatantInfo.GetStatBlock(defenderId);
            var chanceToHit = parametersPerLevel[level].accuracy;
            if (speedBasedAccuracy)
                chanceToHit += attackerStats.speed.value;
            if (speedBasedEvasion)
                chanceToHit -= defenderStats.speed.value;
            if (Random.Range(0, 100) > chanceToHit)
                return new SkillResult();

            int attackerBaseValue;
            if (deficitSkill)
                attackerBaseValue = attackerStats.GetStatBaseValue(attackStat) - attackerStats.GetStatValue(attackStat);
            else
                attackerBaseValue = attackerStats.GetStatValue(attackStat);
            if (remainingEnergySkill)
                attackerBaseValue += attackerStats.GetStatValue(StatType.Energy);
            var delta = parametersPerLevel[level].baseEffectValue +
                        parametersPerLevel[level].attackMultiplier * attackerBaseValue +
                        parametersPerLevel[level].defenseMultiplier * defenderStats.GetStatValue(defenseStat);
            
            if (offensive)
            {
                if (delta >= 0)
                    delta = -1; // should always hurt
            }
            else
            {
                if (delta <= 0)
                    delta = 1; // should always help
            }
            return new SkillResult(animateAttacked, affectedStat, (int)delta, melee,
                visualEffect, level, InflictedCondition(level), conditionRemover);
        }

        private GameObject InflictedCondition(int level)
        {
            return Random.Range(0, 100) <= parametersPerLevel[level].chanceToInflict ? conditionGo : null;
        }
    }
}
