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
        public bool speedBasedEvasion;
        public int energyCost;
        public int hpCost;
        public TargetType targetType;
        public SkillAnimation skillAnimation;
        [Header("Stats For Skill")]
        [EnumOrder("2,3,4,5")]
        public StatType attackStat;
        [EnumOrder("2,3,4,5")]
        public StatType defenseStat;
        [EnumOrder("0,1,5")]
        public StatType affectedStat;
        public List<SkillParameters> parametersPerLevel;
        [CanBeNull] public GameObject visualEffect;
        [CanBeNull] public GameObject conditionGo;
        [CanBeNull] public ConditionRemover conditionRemover;

        [CanBeNull] private Condition _condition = null;

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
            if (conditionGo != null && conditionGo.GetComponent<Condition>().parametersPerLevel.Count !=
                parametersPerLevel.Count)
                throw new ConstraintException($"{name}: Mismatch between skill levels and condition levels");
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
            var desc = $"<u>{name.Split('(')[0]}</u>: {description}\n";
            if (affectedStat != StatType.None)
            {
                desc += offensive ? "Attacks " : "Heals ";
                switch (targetType)
                {
                    case TargetType.Single:
                        desc += "the target"; 
                        break;
                    case TargetType.Group:
                        desc += "the group";
                        break;
                    case TargetType.All:
                        desc += "everyone";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
                if (attackStat != StatType.None && attackerPercent != 0)
                    desc += $"{attackerPercent}% of users {attackStat} ";
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
                desc += $"Has a chance to inflict {_condition.GetDescription(level)}\n"; 
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
                desc += _condition.GetLevelupDescription(level);
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
            var chanceToHit = parametersPerLevel[level].accuracy + 
                              (speedBasedEvasion ? attackerStats.speed.value - defenderStats.speed.value : 0);
            if (Random.Range(0, 100) > chanceToHit)
                return new SkillResult();
            
            var delta = parametersPerLevel[level].baseEffectValue +
                        parametersPerLevel[level].attackMultiplier * attackerStats.GetStatValue(attackStat) +
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
