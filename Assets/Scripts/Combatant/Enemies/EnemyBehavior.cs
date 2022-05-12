using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySkill
{
    public GameObject prefab;
    public int level;
}

public class EnemyBehavior : MonoBehaviour
{
    public List<EnemySkill> skills;

    [Range(0, 100)] 
    public List<int> skillProbabilities;

    [Range(0,100)]
    public int probabilityToTargetPlayer;
    
    
    private CombatantId _id;
    private CombatantEvents _combatantEvents;
    private void Start()
    {
        _id = GetComponent<ID>().id;
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnStartTurn += PlayTurn;
    }

    private void OnValidate()
    {
        if (skills.Count == 0)
            throw new ConstraintException("Enemy has no skills");
        if (skillProbabilities.Count != skills.Count - 1)
            throw new ConstraintException("Number of probabilities for skills must be number of skills minus 1");
        if (skillProbabilities.Sum() > 100)
            throw new ConstraintException("Sum of probabilities exceeds 100");
    }

    private CombatantId ChooseTarget()
    {
        var probabilityToTargetParty = 100 - probabilityToTargetPlayer;
        List<CombatantId> partyMembers = new List<CombatantId>();
        if (CombatantInfo.CombatantIsActive(CombatantId.PartyMemberTop))
            partyMembers.Add(CombatantId.PartyMemberTop);
        if (CombatantInfo.CombatantIsActive(CombatantId.PartyMemberBottom))
            partyMembers.Add(CombatantId.PartyMemberBottom);
        
        var random = Random.Range(0, 100);
        return partyMembers.Count switch
        {
            1 when random < probabilityToTargetParty => partyMembers[0],
            2 when random < probabilityToTargetParty / 2 => CombatantId.PartyMemberBottom,
            2 when random < probabilityToTargetPlayer => CombatantId.PartyMemberTop,
            _ => CombatantId.Player
        };
    }

    private EnemySkill ChooseSkill()
    {
        var random = Random.Range(0, 100);
        var chance = 0;
        for (var i = 0; i < skills.Count - 1; i++)
        {
            chance += skillProbabilities[i];
            if (chance < random)
                return skills[i];
        }
        return skills.Last();
    }

    private void PlayTurn(CombatantId turnId)
    {
        if (turnId != _id) return;
        var chosenSkill = ChooseSkill();
        var skill = chosenSkill.prefab.GetComponent<Skill>();
        skill.level = chosenSkill.level;
        CombatEvents.SkillChosen(ChooseTarget(), skill);
    }

    private void OnDestroy()
    {
        CombatEvents.OnStartTurn -= PlayTurn;
    }
}
