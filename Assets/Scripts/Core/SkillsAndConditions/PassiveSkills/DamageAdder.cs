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
        public float damageMultiplier;
    }
    public class DamageAdder : SkillBase
    {
        [Multiline] public string description;
        public GameObject visualEffect;
        public List<DamageAdderParameters> parametersPerLevel;
        
        public override string GetDescription(int level)
        {
            return $"<u>{name.Split('(')[0]} (lvl.{level})</u>: {description}\n" +
                   $"Every attack there is a {parametersPerLevel[level].addChance}% " +
                   $"chance to multiply the damage by {parametersPerLevel[level].damageMultiplier}";
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
            if (currentParams.addChance != nextParams.addChance)
                desc +=
                    $"Chance To Multiply Damage: {currentParams.addChance} -> {nextParams.addChance}";
            if (currentParams.damageMultiplier != nextParams.damageMultiplier)
                desc +=
                    $"Multiply Damage By: {currentParams.damageMultiplier} -> {nextParams.damageMultiplier}";
            
            return desc;
        }
        
        public override int GetMaxLevel()
        {
            return parametersPerLevel.Count - 1;
        }
    }
}
