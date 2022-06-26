using System;
using Core.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.SkillsAndConditions.PassiveSkills
{
    public class DefensivePassiveSkillsResult
    {
        public bool Reduce;
        public bool Counter;

        public DefensivePassiveSkillsResult()
        {
            Reduce = false;
            Counter = false;
        }
    }
    
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

        public void Init()
        {
            damageAdder.addChance = 0;
            damageAdder.damageMultiplier = 0;
            conditionAdder.addChance = 0;
            conditionToAdd = null;
            levelOfAddedCondition = -1;
            damageReducer.reductionChance = 0;
            damageReducer.reductionPercent = 0;
            damageReflector.reflectChance = 0;
            damageReflector.damageMultiplier= 0;
            damageReflector.percentOfIncomingDamage = 0;
            conditionReflector.reflectChance = 0;
            conditionToReflect = null;
            levelOfReflectedCondition = -1;
        }
        
        public DefensivePassiveSkillsResult ActivateDefensivePassiveSkills(SkillResult incomingSkill, int damage)
        {
            var result = new DefensivePassiveSkillsResult();
            if (incomingSkill.AffectedStat != StatType.Hp || incomingSkill.Delta >= 0)
                return result;
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
                {
                    CombatEvents.Reflected(reflectedDamage, reflectedCondition, levelOfReflectedCondition);
                    result.Counter = true;
                }
            }

            if (Random.Range(0, 100) < damageReducer.reductionChance)
            {
                incomingSkill.Delta *= damageReducer.reductionPercent / 100;
                result.Reduce = true;
            }

            return result;
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

        public PassiveSkills Copy()
        {
            return new PassiveSkills
            {
                damageAdder = new DamageAdderParameters()
                {
                    addChance = damageAdder.addChance, damageMultiplier = damageAdder.damageMultiplier
                },
                conditionAdder = new ConditionAdderParameters()
                {
                    addChance = conditionAdder.addChance
                },
                conditionToAdd = null, 
                levelOfAddedCondition = -1,
                damageReducer = new DamageReducerParameters()
                {
                    reductionChance = damageReducer.reductionChance,
                    reductionPercent = damageReducer.reductionPercent
                },
                damageReflector = new DamageReflectorParameters()
                {
                    reflectChance = damageReflector.reflectChance,
                    percentOfIncomingDamage = damageReflector.percentOfIncomingDamage,
                    damageMultiplier = damageReflector.damageMultiplier
                },
                conditionReflector = new ConditionReflectorParameters()
                {
                    reflectChance = conditionReflector.reflectChance
                },
                conditionToReflect = null,
                levelOfReflectedCondition = -1
            };
        }
    }
}