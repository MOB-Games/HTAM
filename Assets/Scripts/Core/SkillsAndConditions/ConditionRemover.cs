using System.Collections.Generic;
using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.SkillsAndConditions
{
    [CreateAssetMenu]
    public class ConditionRemover : ScriptableObject
    {
        public bool removeAllBuffs;
        public bool removeAllDebuffs;
        public List<ConditionId> specificConditionsToRemove = new List<ConditionId>();
        [CanBeNull] public GameObject visualEffect;

        public bool Removes(ConditionWithLevel conditionWithLevel)
        {
            if (removeAllBuffs && removeAllDebuffs)
                return true;
            if (removeAllBuffs && !conditionWithLevel.condition.offensive && !conditionWithLevel.condition.recurring)
                return true;
            if (removeAllDebuffs && conditionWithLevel.condition.offensive && !conditionWithLevel.condition.recurring)
                return true;
            if (specificConditionsToRemove.Contains(conditionWithLevel.condition.id))
                return true;
            return false;
        }
    }
}

