using System;
using System.Collections.Generic;
using Core.Enums;
using Core.SkillsAndConditions;
using Core.Stats;
using UnityEngine;

[Serializable]
public class ConditionWithLevel
{
    public GameObject conditionGo;
    public Condition condition;
    public int level;
    public int ticks;

    public ConditionWithLevel(GameObject conditionGo, Condition condition, int level)
    {
        this.conditionGo = conditionGo;
        this.condition = condition;
        this.level = level;
        ticks = 0;
    }
}

public class ConditionManager : MonoBehaviour
{
    [HideInInspector] public List<ConditionWithLevel> conditions = new();
    
    private CombatantId _id;
    private StatBlock _statBlock;
    private CombatantEvents _combatantEvents;
    private Vector3 _center;

    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _statBlock = GetComponent<StatModifier>().stats;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnSkillUsed += SkillUsed;
        _combatantEvents.OnConditionReflected += Reflection;
        _combatantEvents.OnEndTurn += Tick;
        CombatEvents.OnStartCombat += RegisterCenter;
    }
    
    private void RegisterCenter()
    {
        _center = GetComponent<BoxCollider2D>().bounds.center;
    }

    private void ConditionEvoked(Condition condition, int level)
    {
        var vfx = condition.visualEffect;
        StartCoroutine(GameManager.PlayVisualEffect(vfx, _center));
        switch (condition)
        {
            case SilenceCondition:
                _combatantEvents.SilenceChanged(true);
                break;
            case TurnSkipCondition:
                _combatantEvents.MobilizationChanged(true);
                break;
            case PacifyCondition:
                GameManager.PacificationChanged(true);
                break;
            default:
                var effect = condition.GetInitialEffect(level, _statBlock);
                _combatantEvents.StatChange(effect.AffectedStat, effect.TotalDelta);
                break;
        }
    }

    private void ConditionRemoved(int index, ConditionWithLevel conditionWithLevel)
    {
        conditions.RemoveAt(index);
        _combatantEvents.RemoveCondition(conditionWithLevel.condition.id);
        switch (conditionWithLevel.condition)
        {
            case SilenceCondition:
                _combatantEvents.SilenceChanged(false);
                break;
            case TurnSkipCondition:
                _combatantEvents.MobilizationChanged(false);
                break;
            case PacifyCondition:
                GameManager.PacificationChanged(false);
                break;
            default:
                var effect = conditionWithLevel.condition.GetRevertEffect(conditionWithLevel.level, _statBlock);
                _combatantEvents.StatChange(effect.AffectedStat, effect.TotalDelta);
                break;
        }
    }

    private void ConditionsChanged(GameObject conditionGo, int level)
    {
        if (conditionGo == null) return;
        var condition = conditionGo.GetComponent<Condition>();
        var conditionWithLevel = conditions.Find(c => c.condition.id == condition.id);
        _combatantEvents.AddCondition(conditionGo, condition.id, level);
        if (conditionWithLevel == null)
        {
            conditions.Add(new ConditionWithLevel(conditionGo, condition, level));
            ConditionEvoked(condition, level);
        }
        else
        {
            conditionWithLevel.ticks = 0;
            StartCoroutine(GameManager.PlayVisualEffect(condition.visualEffect, _center));
        }
    }

    private void SkillUsed(CombatantId targetId, SkillResult result)
    {
        if (targetId != _id) return;
        if (result.ConditionRemover != null)
        {
            StartCoroutine(GameManager.PlayVisualEffect(result.ConditionRemover.visualEffect, _center));
            for (var i = conditions.Count - 1; i >= 0; i--)
            {
                if (result.ConditionRemover.Removes(conditions[i]))
                    ConditionRemoved(i, conditions[i]);
            }
        }
        if (result.Condition == null) return;
        ConditionsChanged(result.Condition, result.ConditionLevel);
    }

    private void Reflection(GameObject condition, int level)
    {
        ConditionsChanged(condition, level);
    }

    private void Tick()
    {
        for (var i = conditions.Count - 1; i >= 0; i--)
        {
            var conditionWithLevel = conditions[i];
            var effect = conditionWithLevel.condition.GetRecurringEffect(conditionWithLevel.level, _statBlock);
            _combatantEvents.StatChange(effect.AffectedStat, effect.TotalDelta);
            conditionWithLevel.ticks++;
            if (conditionWithLevel.condition.Expired(conditionWithLevel.ticks, conditionWithLevel.level))
            {
                ConditionRemoved(i, conditionWithLevel);
            }
        }
        CombatEvents.EndTurn();
    }

    private void OnDestroy()
    {
        CombatEvents.OnSkillUsed -= SkillUsed;
        _combatantEvents.OnConditionReflected -= Reflection;
        _combatantEvents.OnEndTurn -= Tick;
        CombatEvents.OnStartCombat -= RegisterCenter;
    }
}
