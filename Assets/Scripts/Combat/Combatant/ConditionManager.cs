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

    private void ConditionEvoked(ConditionEffect effect)
    {
        StartCoroutine(GameManager.PlayVisualEffect(effect.VisualEffect, _center));
        _combatantEvents.StatChange(effect.AffectedStat, effect.TotalDelta);
    }

    private void TurnSkipConditionEvoked(bool start, GameObject visualEffect)
    {
        if (start)
        {
            StartCoroutine(GameManager.PlayVisualEffect(visualEffect, _center));
            _combatantEvents.MobilizationChanged(true);
        }
        else 
            _combatantEvents.MobilizationChanged(false);
    }
    
    private void SilenceConditionEvoked(bool start, GameObject visualEffect)
    {
        if (start)
        {
            StartCoroutine(GameManager.PlayVisualEffect(visualEffect, _center));
            _combatantEvents.SilenceChanged(true);
        }
        else 
            _combatantEvents.SilenceChanged(false);
    }

    private void ConditionRemoved(int index, ConditionWithLevel conditionWithLevel)
    {
        conditions.RemoveAt(index);
        _combatantEvents.RemoveCondition(conditionWithLevel.condition.id);
        switch (conditionWithLevel.condition)
        {
            case TurnSkipCondition:
                TurnSkipConditionEvoked(false, null);
                break;
            case SilenceCondition:
                SilenceConditionEvoked(false, null);
                break;
            default:
                ConditionEvoked(conditionWithLevel.condition.GetRevertEffect(conditionWithLevel.level, _statBlock));
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
            switch (condition)
            {
                case TurnSkipCondition:
                    TurnSkipConditionEvoked(true, condition.visualEffect);
                    break;
                case SilenceCondition:
                    SilenceConditionEvoked(true, condition.visualEffect);
                    break;
                default:
                    ConditionEvoked(condition.GetInitialEffect(level, _statBlock));
                    break;
            }
        }
        else
        {
            conditionWithLevel.ticks = 0;
            StartCoroutine(GameManager.PlayVisualEffect(condition.GetInitialEffect(level, _statBlock).VisualEffect,
                _center));
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
            ConditionEvoked(conditionWithLevel.condition.GetRecurringEffect(conditionWithLevel.level, _statBlock));
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
