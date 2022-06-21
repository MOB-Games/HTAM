using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.SkillsAndConditions
{
    public class ConditionEffect
    {
        public readonly StatType AffectedStat;
        public readonly int TotalDelta;
        [CanBeNull] public readonly GameObject VisualEffect;

        public ConditionEffect(StatType affectedStat, int totalDelta, GameObject visualEffect = null)
        {
            AffectedStat = affectedStat;
            TotalDelta = totalDelta;
            VisualEffect = visualEffect;
        }

        public ConditionEffect(GameObject visualEffect = null)
        {
            AffectedStat = StatType.Hp;
            TotalDelta = 0;
            VisualEffect = visualEffect;
        }
    }
}
