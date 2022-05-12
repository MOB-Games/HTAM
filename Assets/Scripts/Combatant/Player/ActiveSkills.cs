using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveSkills : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> offensiveSkills;
    [HideInInspector]
    public List<GameObject> defensiveSkills;
    public Dictionary<SkillId, int> SkillLevels;

    private CombatantId _id;
    
    private void Start()
    {
        _id = GetComponent<ID>().id;
        CombatEvents.OnStartTurn += StartTurn;
        CombatEvents.OnSkillChosen += UnregisterToClick;
    }

    private void StartTurn(CombatantId turnId)
    {
        if (_id != turnId) return;
        RegisterToClick();
        SetSkillLevels();
    }

    private void RegisterToClick()
    {
        CombatEvents.OnClick += TargetClicked;
    }

    private void UnregisterToClick(CombatantId targetId, Skill skill)
    {
        CombatEvents.OnClick -= TargetClicked;
    }

    private void SetSkillLevels()
    {
        foreach (var skill in offensiveSkills.Select(skillGo => skillGo.GetComponent<Skill>()))
        {
            skill.level = SkillLevels[skill.id];
        }

        foreach (var skill in defensiveSkills.Select(skillGo => skillGo.GetComponent<Skill>()))
        {
            skill.level = SkillLevels[skill.id];
        }
    }

    private void TargetClicked(PointerEventData eventData)
    {
        var targetId = eventData.pointerPress.GetComponent<ID>().id;
        CombatEvents.OpenMenu(targetId,
            eventData.pointerPress.TryGetComponent(out EnemyBehavior _) ? offensiveSkills : defensiveSkills);
    }
    

    private void OnDestroy()
    {
        CombatEvents.OnStartTurn -= StartTurn;
        CombatEvents.OnSkillChosen -= UnregisterToClick;
    }
}
