using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SkillsAndConditions.PassiveSkills
{
    [Serializable]
    public class DamageReducerParameters
    {
        [Range(0, 100)] 
        public int reductionChance;

        [Range(0, 100)] 
        public int reductionPercent;
    }
    public class DamageReducer : SkillBase
    {
        public List<DamageReducerParameters> parametersPerLevel;
        
        public override string GetDescription(int level)
        {
            var desc = $"<u>{name.Split('(')[0]}</u>: Every time an enemy attacks there is a" +
                   $" {parametersPerLevel[level].reductionChance}% chance to ";
            if (parametersPerLevel[level].reductionPercent == 0)
                desc += "avoid taking damage";
            else
                desc += $"take only {parametersPerLevel[level].reductionPercent}% of the damage";

            return desc;
        }

        public override string GetLevelupDescription(int level)
        {
            var desc = GetDescription(level);
            if (level == parametersPerLevel.Count - 1)
                return desc + "\n\n<b>Level Maxed</b>";
            desc += "\n\n<u>Next Level</u>:\n";
            var currentParams = parametersPerLevel[level];
            var nextParams = parametersPerLevel[level + 1];
            if (currentParams.reductionChance != nextParams.reductionChance)
                desc +=
                    $"Chance To {(currentParams.reductionPercent == 0 ? "Avoid" : "Reduce")} Damage: " +
                    $"{currentParams.reductionChance} -> {nextParams.reductionChance}";
            if (currentParams.reductionPercent != nextParams.reductionPercent)
                desc +=
                    $"Percent Of Damage Taken: {currentParams.reductionPercent} -> {nextParams.reductionPercent}";
            
            return desc;
        }
    }
}
