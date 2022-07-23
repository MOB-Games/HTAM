using System.Data;
using Core.SkillsAndConditions;

public class PacifyCondition : Condition
{
    private void OnValidate()
    {
        if (parametersPerLevel.Count == 0)
            throw new ConstraintException($"{name}: A Condition must have at least 1 level");
        if (parametersPerLevel.Exists(p => p.duration <= 0))
            throw new ConstraintException($"{name}: A condition duration must be positive for all levels");
        if (recurring)
            throw new ConstraintException($"{name}: Pacify condition can't be recurring");
        if (!offensive)
            throw new ConstraintException($"{name}: Pacify condition can't defensive");
    }
    
    public new string GetDescription(int level)
    {
        return $"<u>{id}</u>: No damage is taken to hp for {parametersPerLevel[level].duration} turns";
    }

    public new string GetLevelupDescription(int level)
    {
        return level == parametersPerLevel.Count - 1 ? "" :
            $"Duration: {parametersPerLevel[level].duration} -> {parametersPerLevel[level + 1].duration}\n";
    }
}