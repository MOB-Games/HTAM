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
        // variables for calculating if skill hits or misses
        public int baseHitChance;
        
        // variables for calculating effect of skill
        public int baseEffectValue;
        public double attackMultiplier;
        public double defenceMultiplier;
        
        [Range(0,100)]
        public int chanceToInflict;
    }

    public class Skill : MonoBehaviour
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
        public StatType defenceStat;
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

        private void Start()
        {
            if (conditionGo != null)
                _condition = conditionGo.GetComponent<Condition>();
        }

        public string GetDescription()
        {
            var desc = $"<u>{name.Split('(')[0]}</u>: {description}\n";
            if (_condition != null)
                desc += $"Has a chance to inflict {_condition.GetDescription()}\n"; 
            if (energyCost > 0)
                desc += $"\nEnergy Cost: {energyCost}\n";
            if (hpCost > 0)
                desc += $"\nHP Cost: {hpCost}\n";
            return desc;
        }

        public SkillResult GetResult(CombatantId attackerId, CombatantId defenderId, int level)
        {
            if (level >= parametersPerLevel.Count)
                throw new ArgumentOutOfRangeException(
                    $"Tried to use skill {name} with level {level}, but it has a max level of {parametersPerLevel.Count - 1}");
            var attackerStats = CombatantInfo.GetStatBlock(attackerId);
            var defenderStats = CombatantInfo.GetStatBlock(defenderId);
            var chanceToHit = parametersPerLevel[level].baseHitChance + 
                              (speedBasedEvasion ? attackerStats.speed.value - defenderStats.speed.value : 0);
            if (Random.Range(0, 100) > chanceToHit)
                return new SkillResult();
            
            var delta = parametersPerLevel[level].baseEffectValue +
                        parametersPerLevel[level].attackMultiplier * attackerStats.GetStatValue(attackStat) +
                        parametersPerLevel[level].defenceMultiplier * defenderStats.GetStatValue(defenceStat);
            
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
            return new SkillResult(animateAttacked, affectedStat, (int)delta, 
                visualEffect, level, InflictedCondition(level), conditionRemover);
        }

        private GameObject InflictedCondition(int level)
        {
            return Random.Range(0, 100) <= parametersPerLevel[level].chanceToInflict ? conditionGo : null;
        }
    }
}
