using Core.Enums;

namespace Core.SkillsAndConditions
{
    public class ConditionEffect
    {
        public readonly StatType AffectedStat;
        public readonly int TotalDelta;

        public ConditionEffect(StatType affectedStat, int totalDelta)
        {
            AffectedStat = affectedStat;
            TotalDelta = totalDelta;
        }

        public ConditionEffect()
        {
            AffectedStat = StatType.Hp;
            TotalDelta = 0;
        }
    }
}
