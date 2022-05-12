using System;
using Core.Enums;
using UnityEngine;

public class CombatantEvents : MonoBehaviour
{
    public event Action<StatType, int> OnStatChange;
    public event Action OnHpChange;
    public event Action OnEnergyChange;
    public event Action<Vector3> OnMoveToTarget;
    public event Action OnReturn;
    public event Action OnFinishedMoving;
    public event Action OnAttack;
    public event Action OnHurt;
    public event Action OnHelped;
    public event Action OnDodged;
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

    public void HpChange()
    {
        OnHpChange?.Invoke();
    }
    
    public void EnergyChange()
    {
        OnEnergyChange?.Invoke();
    }

    public void MoveToTarget(Vector3 target)
    {
        OnMoveToTarget?.Invoke(target);
    }

    public void Return()
    {
        OnReturn?.Invoke();
    }

    public void FinishedMoving()
    {
        OnFinishedMoving?.Invoke();
    }

    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void Hurt()
    {
        OnHurt?.Invoke();
    }

    public void Helped()
    {
        OnHelped?.Invoke();
    }
    
    public void Dodged()
    {
        OnDodged?.Invoke();
    }
    
    public void Died() 
    {
        OnDied?.Invoke();
        CombatEvents.CombatantDied(_id);
    }
}
