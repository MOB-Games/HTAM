using System.Collections.Generic;
using Core.DataTypes;
using Core.Enums;
using UnityEngine;
using Core.SkillsAndConditions;
using UnityEngine.EventSystems;

public class ActiveSkills : MonoBehaviour
{
    [HideInInspector]
    public List<SkillWithLevel> offensiveSkills;
    [HideInInspector]
    public List<SkillWithLevel> defensiveSkills;

    private CombatantId _id;
    
    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        CombatEvents.OnStartTurn += StartTurn;
        CombatEvents.OnSkillChosen += UnregisterToClick;
    }

    private void StartTurn(CombatantId turnId)
    {
        if (_id != turnId) return;
        RegisterToClick();
    }

    private void RegisterToClick()
    {
        CombatEvents.OnClick += TargetClicked;
    }

    private void UnregisterToClick(CombatantId targetId, Skill skill, int level)
    {
        CombatEvents.OnClick -= TargetClicked;
    }

    private void TargetClicked(PointerEventData eventData)
    {
        var targetId = eventData.pointerPress.GetComponent<CombatId>().id;
        CombatEvents.OpenMenu(_id, targetId,
            eventData.pointerPress.TryGetComponent(out EnemyBehavior _) ? offensiveSkills : defensiveSkills);
    }
    

    private void OnDestroy()
    {
        CombatEvents.OnStartTurn -= StartTurn;
        CombatEvents.OnSkillChosen -= UnregisterToClick;
    }
}
