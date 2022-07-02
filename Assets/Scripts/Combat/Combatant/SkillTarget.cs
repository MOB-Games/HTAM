using Core.SkillsAndConditions;
using Core.Enums;
using Core.SkillsAndConditions.PassiveSkills;
using Core.Stats;
using UnityEngine;

public class SkillTarget : MonoBehaviour
{
    private CombatantId _id;
    private CombatantEvents _combatantEvents;
    private Vector3 _center;
    private PassiveSkills _passiveSkills;
    private Stat _damage;

    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        _passiveSkills = TryGetComponent<MemoryManager>(out var memoryManager) ? 
            memoryManager.state.passiveSkills : GetComponent<EnemyPassiveSkills>().passiveSkills;
        _damage = GetComponent<StatModifier>().stats.damage;
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
        var defensivePassiveResult = _passiveSkills.ActivateDefensivePassiveSkills(result, _damage.value);
        if (defensivePassiveResult.Reduce)
            _combatantEvents.DamageReduced();
        else if (result.AnimateAttacked)
            _combatantEvents.Hurt();
        if (defensivePassiveResult.Counter)
        {
            StartCoroutine(GameManager.PlayVisualEffect(defensivePassiveResult.vfx, _center));
            _combatantEvents.AnimateSkill(SkillAnimation.Attack);
        }
        StartCoroutine(GameManager.PlayVisualEffect(result.VisualEffect, _center));
        _combatantEvents.StatChange(result.AffectedStat, result.Delta);
    }

    private void OnDestroy()
    {
        CombatEvents.OnSkillUsed -= SkillUsed;
        CombatEvents.OnStartCombat -= RegisterCenter;
    }
}
