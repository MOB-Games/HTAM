using System;
using System.Collections;
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
        if (result.AnimateAttacked)
            _combatantEvents.Hurt();
        if (result.VisualEffect != null)
        {
            StartCoroutine(PlayVisualEffect(result.VisualEffect));
        }
        _combatantEvents.StatChange(result.AffectedStat, result.Delta);
    }

    private IEnumerator PlayVisualEffect(GameObject visualEffect)
    {
        var inst = Instantiate(visualEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(inst);
    }

    private void OnDestroy()
    {
        CombatEvents.OnSkillUsed -= SkillUsed;
    }
}
