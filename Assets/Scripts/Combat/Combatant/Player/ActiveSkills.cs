using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    
    private int _mobilization;
    private CombatantId _id;
    private CombatantEvents _combatantEvents;
    private List<SkillWithLevel> _allySkills;
    
    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        _allySkills = defensiveSkills.Where(s => s.skillGo.GetComponent<Skill>().targetType != TargetType.Self)
            .ToList();
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnMobilizationChanged += MobilizationChanged;
        CombatEvents.OnStartTurn += StartTurn;
        CombatEvents.OnSkillChosen += UnregisterToClick;
    }
    
    private void MobilizationChanged(bool immobilized)
    {
        _mobilization += immobilized ? -1 : 1;
    }
    
    private IEnumerator DelayedSkipTurn()
    {
        yield return new WaitForSeconds(0.5f);
        CombatEvents.SkillChosen(CombatantId.None, GameManager.Instance.GetSkipTurnSkill(), 0);
    }

    private void StartTurn(CombatantId turnId)
    {
        if (_id != turnId) return;
        if (_mobilization < 0)
        {
            StartCoroutine(DelayedSkipTurn());
            return;
        }
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
        List<SkillWithLevel> skillsToShow = null;
        if (eventData.pointerPress.TryGetComponent(out EnemyBehavior _))
            skillsToShow = offensiveSkills;
        else if (targetId != _id)
            skillsToShow = _allySkills;
        else
            skillsToShow = defensiveSkills;

        CombatEvents.OpenMenu(_id, targetId, skillsToShow);
    }
    

    private void OnDestroy()
    {
        _combatantEvents.OnMobilizationChanged -= MobilizationChanged;
        CombatEvents.OnStartTurn -= StartTurn;
        CombatEvents.OnSkillChosen -= UnregisterToClick;
    }
}
