using System;
using Core.Enums;
using UnityEngine;

public class CombatantEvents : MonoBehaviour
{
    public event Action<StatType, int, bool> OnStatChange;
    public event Action<CombatantId> OnMoveToTarget;
    public event Action OnReturn;
    public event Action OnFinishedMoving;
    public event Action<SkillAnimation> OnAnimateSkill;
    public event Action OnHurt;
    public event Action<GameObject, ConditionId> OnConditionAdded;
    public event Action<ConditionId> OnConditionRemoved;
    public event Action OnEndTurn;
    public event Action OnDied;

    private CombatantId _id;

    private void Start()
    {
        _id = GetComponent<CombatId>().id;
    }

    public void StatChange(StatType affectedStat, int delta, bool percentage)
    {
        OnStatChange?.Invoke(affectedStat, delta, percentage);
    }
 
    public void MoveToTarget(CombatantId targetId)
    {
        OnMoveToTarget?.Invoke(targetId);
    }

    public void Return()
    {
        OnReturn?.Invoke();
    }

    public void FinishedMoving()
    {
        OnFinishedMoving?.Invoke();
    }

    public void AnimateSkill(SkillAnimation skillAnimation)
    {
        OnAnimateSkill?.Invoke(skillAnimation);
    }
    
    public void Hurt()
    {
        OnHurt?.Invoke();
    }

    public void AddCondition(GameObject condition, ConditionId conditionId)
    {
        OnConditionAdded?.Invoke(condition, conditionId);
    }
    
    public void RemoveCondition(ConditionId conditionId)
    {
        OnConditionRemoved?.Invoke(conditionId);
    }
    
    public void EndTurn()
    {
        OnEndTurn?.Invoke();
    }

    public void Died() 
    {
        OnDied?.Invoke();
        CombatEvents.CombatantDied(_id);
    }

    private void OnDestroy()
    {
        OnStatChange = null;
        OnHurt = null;
        OnReturn = null;
        OnAnimateSkill = null;
        OnConditionAdded = null;
        OnConditionRemoved = null;
        OnEndTurn = null;
        OnFinishedMoving = null;
        OnMoveToTarget = null;
        OnDied = null;
    }
}
