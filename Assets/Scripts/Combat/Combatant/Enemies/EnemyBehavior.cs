using System.Collections;
using Core.DataTypes;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Enums;
using UnityEngine;
using Core.SkillsAndConditions;
using Core.Stats;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
{
    public List<SkillWithLevel> skillsWithLevels;

    [Range(0, 100)] 
    public List<int> skillProbabilities;

    [Range(0,100)]
    public int probabilityToTargetPlayer;

    private int _mobilization;
    private int _silence;
    private CombatantId _id;
    private StatBlock _stats;
    private CombatantEvents _combatantEvents;
    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _stats = GetComponent<StatModifier>().stats;
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnMobilizationChanged += MobilizationChanged;
        _combatantEvents.OnSilenceChanged += SilenceChanged;
        CombatEvents.OnStartTurn += PlayTurn;
    }

    private void OnValidate()
    {
        if (skillsWithLevels.Count == 0)
            throw new ConstraintException($"{name}: Enemy has no skills");
        if (skillProbabilities.Count != skillsWithLevels.Count - 1)
            throw new ConstraintException($"{name}: Number of probabilities for skills must be number of skills minus 1");
        if (skillProbabilities.Sum() > 100)
            throw new ConstraintException($"{name}: Sum of probabilities exceeds 100");
        var firstSkill = skillsWithLevels.First().skillGo.GetComponent<Skill>();
        if (firstSkill.energyCost != 0 || firstSkill.hpCost != 0)
            throw new ConstraintException( $"{name}: First skill of enemy must be without cost");
    }

    private void MobilizationChanged(bool immobilized)
    {
        _mobilization += immobilized ? -1 : 1;
    }
    
    private void SilenceChanged(bool silenced)
    {
        _silence += silenced ? -1 : 1;
    }

    private CombatantId ChooseTarget()
    {
        var probabilityToTargetParty = 100 - probabilityToTargetPlayer;
        var partyMembers = new List<CombatantId>();
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

    private SkillWithLevel ChooseSkill()
    {
        var random = Random.Range(0, 100);
        var chance = 0;
        for (var i = 0; i < skillsWithLevels.Count - 1; i++)
        {
            chance += skillProbabilities[i];
            if (random < chance)
                return skillsWithLevels[i];
        }
        return skillsWithLevels.Last();
    }
    
    private IEnumerator DelayedSkipTurn()
    {
        yield return new WaitForSeconds(0.5f);
        CombatEvents.SkillChosen(CombatantId.None, GameManager.Instance.skipTurnSkill, 0);
    }
    
    private IEnumerator DelayedSkillChosen(CombatantId targetId, Skill skill, int level)
    {
        yield return new WaitForSeconds(0.5f);
        CombatEvents.SkillChosen(targetId, skill, level);
    }

    private void PlayTurn(CombatantId turnId)
    {
        if (turnId != _id) return;
        if (_mobilization < 0)
        {
            StartCoroutine(DelayedSkipTurn());
            return;
        }
        var chosenSkill = ChooseSkill();
        var skill = chosenSkill.skillGo.GetComponent<Skill>();
        if (_stats.energy.value < skill.energyCost || _stats.hp.value < skill.hpCost ||
            (_silence < 0 && skill.skillAnimation == SkillAnimation.Spell))
            skill = skillsWithLevels.First().skillGo.GetComponent<Skill>();
        StartCoroutine(DelayedSkillChosen(ChooseTarget(), skill, chosenSkill.level));
    }

    private void OnDestroy()
    {
        _combatantEvents.OnMobilizationChanged -= MobilizationChanged;
        _combatantEvents.OnSilenceChanged -= SilenceChanged;
        CombatEvents.OnStartTurn -= PlayTurn;
    }
}
