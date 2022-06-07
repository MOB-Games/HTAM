using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.SkillsAndConditions
{
    public class ConditionEffect
    {
        public readonly bool IsPercentBased;
        public readonly StatType AffectedStat;
        public readonly int Delta;
        [CanBeNull] public readonly GameObject VisualEffect;

        public ConditionEffect(bool isPercentBased, StatType affectedStat, int delta, GameObject visualEffect = null)
        {
            IsPercentBased = isPercentBased;
            AffectedStat = affectedStat;
            Delta = delta;
            VisualEffect = visualEffect;
        }

        public ConditionEffect(GameObject visualEffect = null)
        {
            IsPercentBased = false;
            AffectedStat = StatType.Hp;
            Delta = 0;
            VisualEffect = visualEffect;
        }
    }
}
