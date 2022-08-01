using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SkillsAndConditions.PassiveSkills
{
    [Serializable]
    public class ConditionAdderParameters
    {
        [Range(0,100)]
        public int addChance;
    }
    public class ConditionAdder : SkillBase
    {
        [Multiline] public string description;
        public GameObject conditionGo;
        public List<ConditionAdderParameters> parametersPerLevel;
        
        private Condition _condition;

        public override string GetDescription(int level)
        {
            if (_condition == null && conditionGo != null)
                _condition = conditionGo.GetComponent<Condition>();
            return $"<u>{name.Split('(')[0]} (lvl.{level})</u>: {description}\n" +
                   $"Every attack there is a " +
                       $"{parametersPerLevel[level].addChance}% chance to inflict the target with {_condition.GetDescription(level)}";
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
            if (parametersPerLevel[level].addChance != parametersPerLevel[level + 1].addChance)
                desc +=
                    $"Chance To Inflict Condition: {parametersPerLevel[level].addChance} -> {parametersPerLevel[level + 1].addChance}";
            desc += _condition.GetLevelupDescription(level);

            return desc;
        }
        
        public override int GetMaxLevel()
        {
            return parametersPerLevel.Count - 1;
        }
    }
}
