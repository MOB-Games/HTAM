using System;
using Core.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.SkillsAndConditions.PassiveSkills
{
    [Serializable]
    public class PassiveSkills
    {
        public DamageAdderParameters damageAdder;
        public ConditionAdderParameters conditionAdder;
        public GameObject conditionToAdd;
        public int levelOfAddedCondition;
        public DamageReducerParameters damageReducer;
        public DamageReflectorParameters damageReflector;
        public ConditionReflectorParameters conditionReflector;
        public GameObject conditionToReflect;
        public int levelOfReflectedCondition;
        
        public bool ActivateDefensivePassiveSkills(SkillResult incomingSkill, int damage)
        {
            if (incomingSkill.AffectedStat != StatType.Hp || incomingSkill.Delta >= 0)
                return false;
            // can only reflect melee attacks that target hp
            if (incomingSkill.Melee)
            {
                var reflectedDamage = 0;
                GameObject reflectedCondition = null;
                if (Random.Range(0, 100) < damageReflector.reflectChance)
                {
                    reflectedDamage = damageReflector.percentOfIncomingDamage / 100 * incomingSkill.Delta;
                    if (reflectedDamage != 0)
                        incomingSkill.Delta += reflectedDamage;
                    else
                        reflectedDamage = damageReflector.damageMultiplier / 100 * damage;
                }
                if (Random.Range(0, 100) < conditionReflector.reflectChance)
                    reflectedCondition = conditionToReflect;
                if (reflectedDamage != 0 || reflectedCondition != null)
                    CombatEvents.Reflected(reflectedDamage, reflectedCondition, levelOfReflectedCondition);
            }

            if (Random.Range(0, 100) < damageReducer.reductionChance)
            {
                incomingSkill.Delta *= damageReducer.reductionPercent / 100;
                return true;
            }

            return false;
        }

        public void ActivateOffensivePassiveSkills(SkillResult outgoingSkill)
        {
            if (outgoingSkill.AffectedStat != StatType.Hp || outgoingSkill.Delta > 0)
                return;
            if (Random.Range(0, 100) < damageAdder.addChance)
            {
                outgoingSkill.Delta *= damageAdder.damageMultiplier;
            }

            if (outgoingSkill.Condition != null && Random.Range(0, 100) < conditionAdder.addChance)
            {
                outgoingSkill.Condition = conditionToAdd;
                outgoingSkill.ConditionLevel = levelOfAddedCondition;
            }
        }
    }
}