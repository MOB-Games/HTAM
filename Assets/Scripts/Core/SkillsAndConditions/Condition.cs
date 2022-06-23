using System;
using System.Collections.Generic;
using System.Data;
using Core.Enums;
using Core.Stats;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.SkillsAndConditions
{
    [Serializable]
    public class ConditionParameters
    {
        public int duration;
        public int delta;
        public int percentDelta;
    }

    public class  Condition : MonoBehaviour
    {
        public ConditionId id;
        public bool recurring;
        public bool offensive;
        public StatType affectedStat;
        [CanBeNull] public GameObject visualEffect;
        public List<ConditionParameters> parametersPerLevel;

        private void OnValidate()
        {
            if (parametersPerLevel.Count == 0)
                throw new ConstraintException($"{name}: A Condition must have at least 1 level");
            if (parametersPerLevel.Exists(p => p.duration <= 0))
                throw new ConstraintException($"{name}: A condition duration must be positive for all levels");
            if (recurring && affectedStat is not (StatType.Hp or StatType.Energy))
                throw new ConstraintException(
                    $"{name}: A recurring condition can only affect Hp or Energy, not {affectedStat}");
        }

        public string GetDescription(int level)
        {
            var desc = $"<u>{id}</u>: ";
            var delta = Math.Abs(parametersPerLevel[level].delta);
            var percentDelta = Math.Abs(parametersPerLevel[level].percentDelta);
            var duration = parametersPerLevel[level].duration;
            if (recurring)
            {
                desc += $"Target {(offensive ? "loses" : "recovers")} ";
                if (delta > 0)
                    desc += $"{delta} ";
                if (delta > 0 && percentDelta > 0)
                    desc += "+ ";
                if (percentDelta > 0)
                    desc += $"{percentDelta}% ";
                desc += $"{affectedStat} every turn for {duration} turns";
            }
            else
            {
                desc += $"Targets {affectedStat} is {(offensive ? "decreased" : "increased")} by ";
                if (delta > 0)
                    desc += $"{delta} ";
                if (delta > 0 && percentDelta > 0)
                    desc += "+ ";
                if (percentDelta > 0)
                    desc += $"{percentDelta}% ";
                desc += $"for {duration} turns";
            }
            return desc;
        }

        public string GetLevelupDescription(int level)
        {
            var desc = "";
            if (level == parametersPerLevel.Count - 1)
                return desc;
            var currentParams = parametersPerLevel[level];
            var nextParams = parametersPerLevel[level + 1];
            if (currentParams.duration != nextParams.duration)
                desc += $"Duration: {currentParams.duration} --> {nextParams.duration}\n";
            if (currentParams.delta != nextParams.delta)
                desc += $"Fixed Change: {currentParams.delta} --> {nextParams.delta}\n";
            if (currentParams.percentDelta != nextParams.percentDelta)
                desc += $"Percentage Change: {currentParams.percentDelta} --> {nextParams.percentDelta}\n";
            if (desc == "")
                desc += "No Change";
            return desc;
        }

        public ConditionEffect GetInitialEffect(int level, StatBlock statBlock)
        {
            return recurring
                ? new ConditionEffect(visualEffect)
                : new ConditionEffect(affectedStat, 
                    GameManager.CalculateTotalDelta(parametersPerLevel[level].delta,
                        parametersPerLevel[level].percentDelta, statBlock.GetStatBaseValue(affectedStat)),
                    visualEffect);
        }

        public ConditionEffect GetRecurringEffect(int level, StatBlock statBlock)
        {
            return recurring ? new ConditionEffect(affectedStat, 
                    GameManager.CalculateTotalDelta(parametersPerLevel[level].delta,
                        parametersPerLevel[level].percentDelta, statBlock.GetStatBaseValue(affectedStat))) : 
                new ConditionEffect();
        }

        public bool Expired(int ticks, int level)
        {
            return ticks >= parametersPerLevel[level].duration;
        }

        public int TurnsLeft(int ticks, int level)
        {
            return parametersPerLevel[level].duration - ticks;
        }

        public ConditionEffect GetRevertEffect(int level, StatBlock statBlock)
        {
            return recurring ? new ConditionEffect()
                : new ConditionEffect(affectedStat, 
                    GameManager.CalculateTotalDelta(parametersPerLevel[level].delta,
                        parametersPerLevel[level].percentDelta, statBlock.GetStatBaseValue(affectedStat)));
        }
    }
}
