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

    private void Start()
    {
        _id = GetComponent<ID>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnSkillUsed += ConditionInflicted;
        _combatantEvents.OnEndTurn += Tick;
    }
    
    private IEnumerator PlayVisualEffect(GameObject visualEffect)
    {
        var inst = Instantiate(visualEffect, GetComponent<Renderer>().bounds.center, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(inst);
    }
    
    private void ConditionEvoked(ConditionEffect effect)
    {
        if (effect.VisualEffect != null)
        {
            StartCoroutine(PlayVisualEffect(effect.VisualEffect));
        }
        _combatantEvents.StatChange(effect.AffectedStat, effect.Delta, effect.IsPercentBased);
    }

    private void ConditionInflicted(CombatantId targetId, SkillResult result)
    {
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
            StartCoroutine(PlayVisualEffect(condition.GetInitialEffect(result.Level).VisualEffect));
        }
    }

    private void Tick()
    {
        for (var i = conditions.Count - 1; i >= 0; i--)
        {
            var conditionWithLevel = conditions[i];
            var condition = conditionWithLevel.condition;
            ConditionEvoked(condition.GetRecurringEffect(conditionWithLevel.level));
            conditionWithLevel.ticks++;
            if (condition.Expired(conditionWithLevel.ticks, conditionWithLevel.level))
            {
                conditions.RemoveAt(i);
                _combatantEvents.RemoveCondition(conditionWithLevel.condition.id);
                ConditionEvoked(condition.GetRevertEffect(conditionWithLevel.level));
            }
        }
        CombatEvents.EndTurn();
    }

    private void OnDestroy()
    {
        CombatEvents.OnSkillUsed -= ConditionInflicted;
        _combatantEvents.OnEndTurn -= Tick;
    }
}
