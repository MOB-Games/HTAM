using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;


namespace Core.SkillsAndConditions
{
    public class SkillResult
    {
        public readonly bool Hit;
        public readonly bool AnimateAttacked;
        public readonly bool Melee;
        public readonly StatType AffectedStat;
        public int Delta;
        public int ConditionLevel;
        public readonly int Level;
        [CanBeNull] public readonly GameObject VisualEffect;
        [CanBeNull] public GameObject Condition;
        [CanBeNull] public readonly ConditionRemover ConditionRemover;

        public SkillResult()
        {
            Hit = false;
            AnimateAttacked = false;
            Melee = false;
            AffectedStat = StatType.None;
            Delta = 0;
            Level = ConditionLevel = 0;
        }

        public SkillResult(bool animateAttacked, StatType affectedStat, int delta, bool melee,
            [CanBeNull] GameObject visualEffect = null, int level = 0, [CanBeNull] GameObject condition = null,
            [CanBeNull] ConditionRemover conditionRemover = null)
        {
            Hit = true;
            AnimateAttacked = animateAttacked;
            AffectedStat = affectedStat;
            Delta = delta;
            Melee = melee;
            Level = ConditionLevel = level;
            VisualEffect = visualEffect;
            Condition = condition;
            ConditionRemover = conditionRemover;
        }
    }
}
