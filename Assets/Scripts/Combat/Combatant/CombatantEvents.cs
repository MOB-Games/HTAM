using System;
using Core.Enums;
using UnityEngine;

public class CombatantEvents : MonoBehaviour
{
    public event Action<StatType, int> OnStatChange;
    public event Action<CombatantId> OnMoveToTarget;
    public event Action OnReturn;
    public event Action OnFinishedMoving;
    public event Action<SkillAnimation> OnAnimateSkill;
    public event Action OnHurt;
    public event Action OnDied;

    private CombatantId _id;

    private void Start()
    {
        _id = GetComponent<ID>().id;
    }

    public void StatChange(StatType affectedStat, int delta)
    {
        OnStatChange?.Invoke(affectedStat, delta);
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

    public void AnimateSkill(SkillAnimation animation)
    {
        OnAnimateSkill?.Invoke(animation);
    }
    
    public void Hurt()
    {
        OnHurt?.Invoke();
    }

    public void Died() 
    {
        OnDied?.Invoke();
        CombatEvents.CombatantDied(_id);
    }
}
