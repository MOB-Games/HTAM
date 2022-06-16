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
    public class StatsForSkill
    {
        public StatType attackStat;
        public StatType defenceStat;
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
        public double defenceMultiplier;
        
        [Range(0,100)]
        public int chanceToInflict;
    }

    public class Skill : MonoBehaviour
    {
        public SkillId id;
        [Multiline]
        public string description;
        public bool melee;
        public bool offensive;
        public bool animateAttacked;
        public bool isPercentBased;
        public bool costIsPercentBased;
        public int energyCost;
        public int hpCost;
        public TargetType targetType;
        public SkillAnimation skillAnimation;
        public StatsForSkill statsForSkill;
        public List<SkillParameters> parametersPerLevel;
        [CanBeNull] public GameObject visualEffect;
        [CanBeNull] public GameObject conditionGo;

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
        }

        private void Start()
        {
            if (conditionGo != null)
                _condition = conditionGo.GetComponent<Condition>();
        }

        public string GetDescription()
        {
            var desc = $"<u>{id}</u>: {description}\n";
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
                    $"Tried to use skill {id} with level {level}, but it has a max level of {parametersPerLevel.Count - 1}");
            var attackerStats = CombatantInfo.GetStatBlock(attackerId);
            var defenderStats = CombatantInfo.GetStatBlock(defenderId);
            var chanceToHit = parametersPerLevel[level].baseHitChance + parametersPerLevel[level].hitMultiplier * attackerStats.speed.value +
                              parametersPerLevel[level].missMultiplier * defenderStats.speed.value;
            if (Random.Range(0, 100) > chanceToHit)
                return new SkillResult();
            
            var delta = parametersPerLevel[level].baseEffectValue +
                        parametersPerLevel[level].attackMultiplier * attackerStats.GetStatValue(statsForSkill.attackStat) +
                        parametersPerLevel[level].defenceMultiplier * defenderStats.GetStatValue(statsForSkill.defenceStat);
            
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
            return new SkillResult(animateAttacked, isPercentBased, statsForSkill.affectedStat, (int)delta, 
                visualEffect, level, InflictedCondition(level));
        }

        private GameObject InflictedCondition(int level)
        {
            return Random.Range(0, 100) <= parametersPerLevel[level].chanceToInflict ? conditionGo : null;
        }
    }
}
