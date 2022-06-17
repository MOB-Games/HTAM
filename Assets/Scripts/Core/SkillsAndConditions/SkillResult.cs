using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;


namespace Core.SkillsAndConditions
{
    public class SkillResult
    {
        public readonly bool Hit;
        public readonly bool AnimateAttacked;
        public readonly bool IsPercentBased;
        public readonly StatType AffectedStat;
        public readonly int Delta;
        public readonly int Level;
        [CanBeNull] public readonly GameObject VisualEffect;
        [CanBeNull] public readonly GameObject Condition;
        [CanBeNull] public readonly ConditionRemover ConditionRemover;

        public SkillResult()
        {
            Hit = false;
            AnimateAttacked = false;
            IsPercentBased = false;
            AffectedStat = StatType.None;
            Delta = 0;
            Level = 0;
        }

        public SkillResult(bool animateAttacked, bool isPercentBased,  StatType affectedStat, int delta, 
            [CanBeNull] GameObject visualEffect = null, int level = 0, [CanBeNull] GameObject condition = null,
            [CanBeNull] ConditionRemover conditionRemover = null)
        {
            Hit = true;
            AnimateAttacked = animateAttacked;
            IsPercentBased = isPercentBased;
            AffectedStat = affectedStat;
            Delta = delta;
            Level = level;
            VisualEffect = visualEffect;
            Condition = condition;
            ConditionRemover = conditionRemover;
        }
    }
}
