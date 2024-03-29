using System.Collections;
using System.Collections.Generic;
using Core.SkillsAndConditions;using Core.Enums;
using Core.SkillsAndConditions.PassiveSkills;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    private bool _moving;
    private CombatantId _id;
    private CombatantEvents _combatantEvents;
    private PassiveSkills _passiveSkills;

    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        _passiveSkills = TryGetComponent<MemoryManager>(out var memoryManager) ? 
            memoryManager.state.passiveSkills : GetComponent<EnemyPassiveSkills>().passiveSkills;
        _combatantEvents.OnFinishedMoving += FinishedMoving;
        CombatEvents.OnStartTurn += RegisterToSkill;
        CombatEvents.OnSkillChosen += UnregisterToSkill;
    }

    private void FinishedMoving()
    {
        _moving = false;
    }

    private void RegisterToSkill(CombatantId turnId)
    {
        if (turnId != _id) return;
        CombatEvents.OnSkillChosen += ExecuteSkill;
    }

    private void UnregisterToSkill(CombatantId targetId, Skill skill, int level)
    {
        CombatEvents.OnSkillChosen -= ExecuteSkill;
    }

    private static List<CombatantId> GetAllTargets(CombatantId targetId, TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.Self:
            case TargetType.Single:
                return new List<CombatantId>() { targetId };
            case TargetType.Group when targetId is CombatantId.Player or CombatantId.PartyMemberTop or CombatantId.PartyMemberBottom:
                return new List<CombatantId>()
                    { CombatantId.Player, CombatantId.PartyMemberTop, CombatantId.PartyMemberBottom };
            case TargetType.Group:
                return new List<CombatantId>() { CombatantId.EnemyCenter, CombatantId.EnemyTop, CombatantId.EnemyBottom };
            case TargetType.All:
            default:
                return new List<CombatantId>()
                {
                    CombatantId.Player, CombatantId.PartyMemberTop, CombatantId.PartyMemberBottom, CombatantId.EnemyCenter,
                    CombatantId.EnemyTop, CombatantId.EnemyBottom
                };
        }
    }
    
    private IEnumerator Execute(CombatantId targetId, Skill skill, int level)
    {
        if (skill.melee)
        {
            _moving = true;
            _combatantEvents.MoveToTarget(targetId);
            yield return new WaitUntil(() => !_moving);
            yield return new WaitForSeconds(0.2f);
        }
        _combatantEvents.AnimateSkill(skill.skillAnimation);
        yield return new WaitForSeconds(0.15f);
        foreach (var  id in GetAllTargets(targetId, skill.targetType))
        {
            if (!CombatantInfo.CombatantIsActive(id)) continue;
            var result = skill.GetResult(_id, id, level);
            if (result.Hit)
            {
                var vfx = _passiveSkills.ActivateOffensivePassiveSkills(result);
                StartCoroutine(GameManager.PlayVisualEffect(vfx, transform.position));
            }
            else
            {
                GameManager.Instance.PlayMissVisualEffect(transform.position);
            }
            CombatEvents.SkillUsed(id, result);
        }
        yield return new WaitForSeconds(0.8f);
        if (skill.melee)
        {
            var localScale = transform.localScale;
            transform.localScale = Vector3.Scale(localScale, new Vector3(-1, 1, 1));
            _moving = true;
            _combatantEvents.Return();
            yield return new WaitUntil(() => !_moving);
            transform.localScale = localScale;
            yield return new WaitForSeconds(0.2f);
        }
        _combatantEvents.StatChange(StatType.Hp, -skill.hpCost);
        _combatantEvents.StatChange(StatType.Energy, skill.remainingEnergySkill ? -int.MaxValue : -skill.energyCost);
        _combatantEvents.EndTurn();
    }

    private void ExecuteSkill(CombatantId targetId, Skill skill, int level)
    {
        StartCoroutine(Execute(targetId, skill, level));
    }

    private void OnDestroy()
    {
        CombatEvents.OnStartTurn -= RegisterToSkill;
        CombatEvents.OnSkillChosen -= UnregisterToSkill;
    }
}
