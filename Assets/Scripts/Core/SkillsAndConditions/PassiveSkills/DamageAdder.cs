using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SkillsAndConditions.PassiveSkills
{
    [Serializable]
    public class DamageAdderParameters
    {
        [Range(0, 100)] 
        public int addChance;
        [Range(0,100)]
        public int damageMultiplier;
    }
    public class DamageAdder : MonoBehaviour
    {
        public List<DamageAdderParameters> parametersPerLevel;
        
        public string GetDescription(int level)
        {
            return $"<u>{name.Split('(')[0]}</u>: Every attack there is a {parametersPerLevel[level].addChance}% " +
                   $"chance to multiply the damage by {parametersPerLevel[level].damageMultiplier}";
        }

        public string GetLevelupDescription(int level)
        {
            var desc = GetDescription(level);
            if (level == parametersPerLevel.Count - 1)
                return desc + "\n\n<b>Level Maxed</b>";
            desc += "\n\n<u>Next Level</u>:\n";
            var currentParams = parametersPerLevel[level];
            var nextParams = parametersPerLevel[level + 1];
            if (currentParams.addChance != nextParams.addChance)
                desc +=
                    $"Chance To Multiply Damage: {currentParams.addChance} -> {nextParams.addChance}";
            if (currentParams.damageMultiplier != nextParams.damageMultiplier)
                desc +=
                    $"Multiply Damage By: {currentParams.damageMultiplier} -> {nextParams.damageMultiplier}";
            
            return desc;
        }
    }
}
