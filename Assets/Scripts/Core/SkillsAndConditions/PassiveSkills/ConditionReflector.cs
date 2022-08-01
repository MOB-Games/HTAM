using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SkillsAndConditions.PassiveSkills
{
    [Serializable]
    public class ConditionReflectorParameters
    {
        [Range(0,100)]
        public int reflectChance;
    }
    public class ConditionReflector : SkillBase
    {
        [Multiline] public string description;
        public GameObject conditionGo;
        public List<ConditionReflectorParameters> parametersPerLevel;
        
        private Condition _condition;
        
        public override string GetDescription(int level)
        {
            if (_condition == null && conditionGo != null)
                _condition = conditionGo.GetComponent<Condition>();
            return $"<u>{name.Split('(')[0]} (lvl.{level})</u>: {description}\n" +
                   $"Every time an enemy attacks with a melee attack there is a " +
                   $"{parametersPerLevel[level].reflectChance}% chance to inflict the attacking enemy with" +
                   $" {_condition.GetDescription(level)}";
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
            if (parametersPerLevel[level].reflectChance != parametersPerLevel[level + 1].reflectChance)
                desc +=
                    $"Chance To Inflict Condition: {parametersPerLevel[level].reflectChance} -> {parametersPerLevel[level + 1].reflectChance}";
            desc += _condition.GetLevelupDescription(level);

            return desc;
        }
        
        public override int GetMaxLevel()
        {
            return parametersPerLevel.Count - 1;
        }
    }
}
