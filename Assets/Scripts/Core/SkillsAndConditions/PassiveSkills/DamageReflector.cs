using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Core.SkillsAndConditions.PassiveSkills
{
    [Serializable]
    public class DamageReflectorParameters
    {
        [Range(0,100)]
        public int reflectChance;
        [Range(0,100)]
        public int percentOfIncomingDamage;
        public float damageMultiplier;
    }
    public class DamageReflector : SkillBase
    {
        [Multiline] public string description;
        public GameObject visualEffect;
        public List<DamageReflectorParameters> parametersPerLevel;

        private void OnValidate()
        {
            if (parametersPerLevel.Any(p => p.percentOfIncomingDamage != 0) &&
                parametersPerLevel.Any(p => p.damageMultiplier != 0))
                throw new ConstraintException($"{name}: Cannot reflect and counter");
        }

        public override string GetDescription(int level)
        {
            var desc = $"<u>{name.Split('(')[0]} (lvl.{level})</u>: {description}\n" +
                       $"Every time an enemy attacks with a melee attack there is a " +
                       $"{parametersPerLevel[level].reflectChance}% chance to ";
            if (parametersPerLevel[level].percentOfIncomingDamage != 0)
                desc += $"reflect {parametersPerLevel[level].percentOfIncomingDamage}% of the damage back at the attacker";
            if (parametersPerLevel[level].damageMultiplier != 0)
                desc += $"counter attack dealing {parametersPerLevel[level].damageMultiplier * 100}% of the users damage";

            return desc;
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
            if (currentParams.reflectChance != nextParams.reflectChance)
                desc +=
                    $"Chance To {(currentParams.percentOfIncomingDamage == 0 ? "Reflect" : "Counter")}: " +
                    $"{currentParams.reflectChance} -> {nextParams.reflectChance}";
            if (currentParams.percentOfIncomingDamage != nextParams.percentOfIncomingDamage)
                desc +=
                    $"Percent Of Damage Reflected: {currentParams.percentOfIncomingDamage} -> {nextParams.percentOfIncomingDamage}";
            else 
                desc +=
                    $"Counter Damage Multiplier: {currentParams.damageMultiplier} -> {nextParams.damageMultiplier}";
            
            return desc;
        }

        public override int GetMaxLevel()
        {
            return parametersPerLevel.Count - 1;
        }
    }
}

