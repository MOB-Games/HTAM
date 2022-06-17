using Core.SkillsAndConditions;using System.Collections;
using Core.Enums;
using UnityEngine;

public class SkillTarget : MonoBehaviour
{
    private CombatantId _id;
    private CombatantEvents _combatantEvents;
    private Vector3 _center;

    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnSkillUsed += SkillUsed;
        CombatEvents.OnStartCombat += RegisterCenter;
    }

    private void RegisterCenter()
    {
        _center = GetComponent<BoxCollider2D>().bounds.center;
    }
    
    private void SkillUsed(CombatantId targetId, SkillResult result)
    {
        if (targetId != _id) return;
        if (result.AnimateAttacked)
            _combatantEvents.Hurt();
        if (result.VisualEffect != null)
        {
            StartCoroutine(GameManager.PlayVisualEffect(result.VisualEffect, _center));
        }
        _combatantEvents.StatChange(result.AffectedStat, result.Delta, result.IsPercentBased);
    }

    private void OnDestroy()
    {
        CombatEvents.OnSkillUsed -= SkillUsed;
        CombatEvents.OnStartCombat -= RegisterCenter;
    }
}
