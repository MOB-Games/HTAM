using System;
using System.Collections.Generic;
using Core.DataTypes;
using Core.Enums;
using UnityEngine;
using Core.SkillsAndConditions;
using UnityEngine.EventSystems;

public static class CombatEvents
{
    public static event Action OnSpawnParty;
    public static event Action<GameObject> OnCombatantAdded;
    public static event Action<int, int> OnPublishStageNumber;
    public static event Action OnActivateStatusHubs;
    public static event Action OnStartCombat;
    public static event Action<CombatantId> OnStartTurn;
    public static event Action OnEndTurn;
    public static event Action<PointerEventData> OnClick;
    public static event Action<CombatantId, CombatantId, List<SkillWithLevel>, bool> OnOpenMenu;
    public static event Action<CombatantId, Skill, int> OnSkillChosen;
    public static event Action<CombatantId, SkillResult> OnSkillUsed;
    public static event Action<int, GameObject, int> OnReflection;
    public static event Action<CombatantId> OnCombatantDied;
    public static event Action<Drop> OnDrop;
    public static event Action<Drop> OnFinalDrop;
    public static event Action OnWin;
    public static event Action OnLose;

    public static void SpawnParty()
    {
        OnSpawnParty?.Invoke();
    }
    
    public static void CombatantAdded(GameObject combatant)
    {
        OnCombatantAdded?.Invoke(combatant);
    }
    
    public static void PublishStageNumber(int stageNumber, int pathLength)
    {
        OnPublishStageNumber?.Invoke(stageNumber, pathLength);
    }

    public static void ActivateStatusHubs()
    {
        OnActivateStatusHubs?.Invoke();
    }
    
    public static void StartCombat()
    {
        OnStartCombat?.Invoke();
    }
    
    public static void StartTurn(CombatantId id)
    {
        OnStartTurn?.Invoke(id);
    }
    
    public static void EndTurn()
    {
        OnEndTurn?.Invoke();
    }
    
    public static void Click(PointerEventData eventData)
    {
        OnClick?.Invoke(eventData);
    }
    
    public static void OpenMenu(CombatantId userId, CombatantId targetId, List<SkillWithLevel> skillsWithLevels, bool silenced)
    {
        OnOpenMenu?.Invoke(userId, targetId, skillsWithLevels, silenced);
    }
    
    public static void SkillChosen(CombatantId targetId, Skill skill, int level)
    {
        OnSkillChosen?.Invoke(targetId,skill, level);
    }
    
    public static void SkillUsed(CombatantId targetId, SkillResult result)
    {
        OnSkillUsed?.Invoke(targetId, result);
    }
    
    public static void Reflected(int damage, GameObject condition, int level)
    {
        OnReflection?.Invoke(damage, condition, level);
    }
    
    public static void CombatantDied(CombatantId id)
    {
        OnCombatantDied?.Invoke(id);
    }

    public static void Drop(Drop drop)
    {
        OnDrop?.Invoke(drop);
    }
    
    public static void FinalDrop(Drop drop)
    {
        OnFinalDrop?.Invoke(drop);
    }
    
    public static void Win()
    {
        OnWin?.Invoke();
    }
    
    public static void Lose()
    {
        OnLose?.Invoke();
    }
}
