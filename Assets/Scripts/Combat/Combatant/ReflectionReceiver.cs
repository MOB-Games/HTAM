using Core.Enums;
using UnityEngine;

public class ReflectionReceiver : MonoBehaviour
{
    private CombatantId _id;
    private CombatantEvents _combatantEvents;

    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnStartTurn += RegisterToReflection;
        CombatEvents.OnEndTurn += UnregisterToReflection;
    }

    private void RegisterToReflection(CombatantId id)
    {
        if (id != _id) return;
        CombatEvents.OnReflection += ReceiveReflection;
    }

    private void ReceiveReflection(int damage, GameObject condition, int level)
    {
        _combatantEvents.StatChange(StatType.Hp, -damage);
        _combatantEvents.ConditionReflected(condition, level);
    }
    
    private void UnregisterToReflection()
    {
        CombatEvents.OnReflection -= ReceiveReflection;
    }

    private void OnDestroy()
    {
        CombatEvents.OnStartTurn -= RegisterToReflection;
        CombatEvents.OnEndTurn -= UnregisterToReflection;
    }
}
