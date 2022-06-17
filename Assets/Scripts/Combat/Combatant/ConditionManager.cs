using System;
using System.Collections;
using System.Collections.Generic;
using Core.Enums;
using Core.SkillsAndConditions;
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
    [HideInInspector] public List<ConditionWithLevel> conditions = new List<ConditionWithLevel>();
    
    private CombatantId _id;
    private CombatantEvents _combatantEvents;
    private Vector3 _center;

    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnSkillUsed += ConditionsChanged;
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
        _combatantEvents.StatChange(effect.AffectedStat, effect.Delta, effect.IsPercentBased);
    }

    private void ConditionRemoved(int index, ConditionWithLevel conditionWithLevel)
    {
        conditions.RemoveAt(index);
        _combatantEvents.RemoveCondition(conditionWithLevel.condition.id);
        ConditionEvoked(conditionWithLevel.condition.GetRevertEffect(conditionWithLevel.level));
    }

    private void ConditionsChanged(CombatantId targetId, SkillResult result)
    {
        if (result.ConditionRemover != null)
        {
            StartCoroutine(GameManager.PlayVisualEffect(result.ConditionRemover.visualEffect, _center));
            for (var i = conditions.Count - 1; i >= 0; i--)
            {
                if (result.ConditionRemover.Removes(conditions[i]))
                    ConditionRemoved(i, conditions[i]);
            }
        }
        if (targetId != _id || result.Condition == null) return;
        var condition = result.Condition.GetComponent<Condition>();
        var conditionWithLevel = conditions.Find(c => c.condition.id == condition.id);
        if (conditionWithLevel == null)
        {
            _combatantEvents.AddCondition(result.Condition, condition.id);
            conditions.Add(new ConditionWithLevel(result.Condition, condition, result.Level));
            ConditionEvoked(condition.GetInitialEffect(result.Level));
        }
        else
        {
            conditionWithLevel.ticks = 0;
            StartCoroutine(GameManager.PlayVisualEffect(condition.GetInitialEffect(result.Level).VisualEffect,
                _center));
        }
    }

    private void Tick()
    {
        for (var i = conditions.Count - 1; i >= 0; i--)
        {
            var conditionWithLevel = conditions[i];
            ConditionEvoked(conditionWithLevel.condition.GetRecurringEffect(conditionWithLevel.level));
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
        CombatEvents.OnSkillUsed -= ConditionsChanged;
        _combatantEvents.OnEndTurn -= Tick;
        CombatEvents.OnStartCombat -= RegisterCenter;
    }
}
