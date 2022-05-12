using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Enums;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    private CombatantId _id;
    private CombatantEvents _combatantEvents;

    private void Start()
    {
        _id = GetComponent<ID>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnStartTurn += RegisterToSkill;
        CombatEvents.OnSkillChosen += UnregisterToSkill;
    }

    private void RegisterToSkill(CombatantId turnId)
    {
        if (turnId != _id) return;
        CombatEvents.OnSkillChosen += ExecuteSkill;
    }

    private void UnregisterToSkill(CombatantId targetId, Skill skill)
    {
        CombatEvents.OnSkillChosen -= ExecuteSkill;
    }

    private static List<CombatantId> GetAllTargets(CombatantId targetId, TargetType targetType)
    {
        switch (targetType)
        {
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
    
    private async Task Execute(CombatantId targetId, bool isMelee, SkillResult result)
    {
        if (isMelee)
        {
            _combatantEvents.MoveToTarget(CombatantInfo.GetLocation(targetId));
            await Task.Delay(TimeSpan.FromSeconds(1.2));
        }
        _combatantEvents.Attack(); // this should change to be according to result.effect in the future
        await Task.Delay(TimeSpan.FromSeconds(0.15));
        CombatEvents.SkillUsed(targetId, result);
        await Task.Delay(TimeSpan.FromSeconds(0.4));
        if (isMelee)
        {
            _combatantEvents.Return();
            await Task.Delay(TimeSpan.FromSeconds(1.2));
        }
        CombatEvents.EndTurn();
    }

    private void ExecuteSkill(CombatantId targetId, Skill skill)
    {
        foreach (var id in GetAllTargets(targetId, skill.targetType))
        {
            if (!CombatantInfo.CombatantIsActive(id)) continue;
            var result = skill.GetResult(_id, id);
            Execute(id, skill.melee, result).GetAwaiter();
        }
        _combatantEvents.StatChange(StatType.Hp, -skill.hpCost);
        _combatantEvents.StatChange(StatType.Energy, -skill.energyCost);
    }

    private void OnDestroy()
    {
        CombatEvents.OnStartTurn -= RegisterToSkill;
        CombatEvents.OnSkillChosen -= UnregisterToSkill;
    }
}
