using System;
using Core.Enums;
using UnityEngine;

public class SkillTarget : MonoBehaviour
{
    private CombatantId _id;
    private CombatantEvents _combatantEvents;
    
    private void Start()
    {
        _id = GetComponent<ID>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnSkillUsed += SkillUsed;
    }
    
    private void SkillUsed(CombatantId targetId, SkillResult result)
    {
        if (targetId != _id) return;
        switch (result.Effect)
        {
            case SkillEffect.Hurt:
                _combatantEvents.Hurt();
                break;
            case SkillEffect.Help:
                _combatantEvents.Helped();
                break;
            case SkillEffect.Miss:
                _combatantEvents.Dodged();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _combatantEvents.StatChange(result.AffectedStat, result.Delta);
    }

    private void OnDestroy()
    {
        CombatEvents.OnSkillUsed -= SkillUsed;
    }
}
