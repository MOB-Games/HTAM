using System;
using System.Collections.Generic;
using System.Data;
using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

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

    public SkillResult GetBuff(int level)
    {
        return isBuff
            ? new SkillResult(false, isPercentBased, affectedStat,
                parametersPerLevel[level] .delta, visualEffect)
                : new SkillResult();
    }

    public SkillResult GetRecurringEffect(int level)
    {
        return isBuff ? new SkillResult() : 
            new SkillResult(false, isPercentBased, affectedStat, 
                parametersPerLevel[level].delta, visualEffect);
    }

    public bool Expired(int ticks, int level)
    {
        return ticks > parametersPerLevel[level].duration;
    }

    public SkillResult GetRevertBuff(int level)
    {
        return isBuff ? new SkillResult(false, isPercentBased, affectedStat, 
                -parametersPerLevel[level].delta)
            : new SkillResult();
    }
}
