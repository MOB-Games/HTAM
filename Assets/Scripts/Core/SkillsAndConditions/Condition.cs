using System;
using System.Collections.Generic;
using System.Data;
using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.SkillsAndConditions
{
    [Serializable]
    public class ConditionParameters
    {
        public int duration;
        public int delta;
    }

    public class  Condition : MonoBehaviour
    {
        public ConditionId id;
        public bool isBuff;
        public bool isPercentBased;
        public StatType affectedStat;
        [CanBeNull] public GameObject visualEffect;
        public List<ConditionParameters> parametersPerLevel;

        private void OnValidate()
        {
            if (parametersPerLevel.Count == 0)
                throw new ConstraintException("A Condition must have at least 1 level");
            if (parametersPerLevel.Exists(p => p.duration <= 0))
                throw new ConstraintException("A condition duration must be positive for all levels");
        }

        public string GetDescription()
        {
            var desc = $"<u>{id}</u>: ";
            desc +=isBuff ? 
                $"Targets {affectedStat} is {(parametersPerLevel[0].delta > 0 ? "increased" : "decreased")}" : 
                $"Target {(parametersPerLevel[0].delta > 0 ? "gains" : "loses")} {affectedStat} every turn";
            return desc;
        }

        public ConditionEffect GetInitialEffect(int level)
        {
            return isBuff
                ? new ConditionEffect(isPercentBased, affectedStat,
                    parametersPerLevel[level] .delta, visualEffect)
                    : new ConditionEffect(visualEffect);
        }

        public ConditionEffect GetRecurringEffect(int level)
        {
            return isBuff ? new ConditionEffect() : 
                new ConditionEffect(isPercentBased, affectedStat, 
                    parametersPerLevel[level].delta);
        }

        public bool Expired(int ticks, int level)
        {
            return ticks >= parametersPerLevel[level].duration;
        }

        public ConditionEffect GetRevertEffect(int level)
        {
            return isBuff ? new ConditionEffect(isPercentBased, affectedStat, 
                    -parametersPerLevel[level].delta)
                : new ConditionEffect();
        }
    }
}
