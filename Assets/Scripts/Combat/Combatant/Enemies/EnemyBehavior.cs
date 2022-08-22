using System;
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

public class SkillComponentWithLevel
{
    public readonly Skill Skill;
    public readonly int Level;

    public SkillComponentWithLevel(Skill skill, int level = 0)
    {
        Skill = skill;
        Level = level;
    }
}

[Serializable]
public class EnemySkill
{
    public SkillWithLevel skillWithLevel;
    [HideInInspector] 
    public Skill skill;
    [Range(0,100)]
    public int probability;
    public int cooldown;
    [HideInInspector] 
    public int cooldownLeft;
}

public class EnemyBehavior : MonoBehaviour
{
    public List<EnemySkill> specialSkills;
    public Skill defaultSkill;

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
        _combatantEvents.OnEndTurn += Tick;
        CombatEvents.OnStartTurn += PlayTurn;

        foreach (var specialSkill in specialSkills)
        {
            specialSkill.skill = specialSkill.skillWithLevel.skillGo.GetComponent<Skill>();
        }
    }

    private void OnValidate()
    {
        if (defaultSkill == null)
            throw new ConstraintException($"{name}: Enemy must have default skill");
        if (specialSkills.Sum(ss => ss.probability) > 100)
            throw new ConstraintException($"{name}: Sum of probabilities exceeds 100");
        if (defaultSkill.energyCost != 0 || defaultSkill.hpCost != 0)
            throw new ConstraintException( $"{name}: Default skill of enemy must be without cost");
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

    private SkillComponentWithLevel ChooseSkill()
    {
        var usableSpecialSkills = specialSkills.FindAll(specialSkill =>
            specialSkill.skill.energyCost <= _stats.energy.value &&
            specialSkill.skill.hpCost < _stats.hp.value &&
            specialSkill.cooldownLeft == 0);
        if (_silence < 0)
            usableSpecialSkills =
                usableSpecialSkills.FindAll(specialSkill => specialSkill.skill.skillAnimation != SkillAnimation.Spell);
        var numUsableSpecialSkills = usableSpecialSkills.Count;
        if (numUsableSpecialSkills == 0)
            return new SkillComponentWithLevel(defaultSkill);
        var unusableSpecialSkills = specialSkills.Except(usableSpecialSkills);
        var additionalChance = unusableSpecialSkills.Sum(unusableSpecialSkill => unusableSpecialSkill.probability / (numUsableSpecialSkills + 1));
        
        var random = Random.Range(0, 100);
        var chance = 0;
        foreach (var specialSkill in usableSpecialSkills)
        {
            chance += specialSkill.probability + additionalChance;
            if (random < chance)
            {
                specialSkill.cooldownLeft = specialSkill.cooldown;
                return new SkillComponentWithLevel(specialSkill.skill, specialSkill.skillWithLevel.level);
            }
        }

        return new SkillComponentWithLevel(defaultSkill);
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
        StartCoroutine(DelayedSkillChosen(ChooseTarget(), chosenSkill.Skill, chosenSkill.Level));
    }

    private void Tick()
    {
        foreach (var specialSkill in specialSkills.Where(specialSkill => specialSkill.cooldownLeft > 0))
        {
            specialSkill.cooldownLeft--;
        }
    }

    private void OnDestroy()
    {
        _combatantEvents.OnMobilizationChanged -= MobilizationChanged;
        _combatantEvents.OnSilenceChanged -= SilenceChanged;
        _combatantEvents.OnEndTurn -= Tick;
        CombatEvents.OnStartTurn -= PlayTurn;
    }
}
