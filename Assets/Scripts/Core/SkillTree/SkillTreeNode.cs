using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataTypes;
using Core.Enums;
using Core.SkillsAndConditions;
using Core.SkillsAndConditions.PassiveSkills;
using UnityEngine;
using UnityEngine.UI;



public class SkillTreeNode : MonoBehaviour
{
    public List<SkillTreeNode> parents;
    public SkillType type;
    public SkillWithLevel skillWithLevel;
    public SkillBase content;

    private int _maxLevel;
    private Button _button;
    private Image _image;
    private SkillLevelupDescription _levelupDescription;
    
    
    private void Start()
    {
        _button = GetComponentInChildren<Button>();
        _image = GetComponentInChildren<Image>();
        _button.onClick.AddListener(() => TownEvents.LevelupSkill(this));

        _levelupDescription = GetComponent<SkillLevelupDescription>();
        switch (type)
        {
            case SkillType.Normal:
                var skill = skillWithLevel.skillGo.GetComponent<Skill>();
                content = skill;
                _levelupDescription.desc = skill.GetLevelupDescription(skillWithLevel.level);
                _maxLevel = skill.parametersPerLevel.Count - 1;
                break;
            case SkillType.DamageAdder:
                var damageAdder = skillWithLevel.skillGo.GetComponent<DamageAdder>();
                content = damageAdder;
                _levelupDescription.desc = damageAdder.GetLevelupDescription(skillWithLevel.level);
                _maxLevel = damageAdder.parametersPerLevel.Count - 1;
                break;
            case SkillType.ConditionAdder:
                var conditionAdder = skillWithLevel.skillGo.GetComponent<ConditionAdder>();
                content = conditionAdder;
                _levelupDescription.desc = conditionAdder.GetLevelupDescription(skillWithLevel.level);
                _maxLevel = conditionAdder.parametersPerLevel.Count - 1;
                break;
            case SkillType.DamageReducer:
                var damageReducer = skillWithLevel.skillGo.GetComponent<DamageReducer>();
                content = damageReducer;
                _levelupDescription.desc = damageReducer.GetLevelupDescription(skillWithLevel.level);
                _maxLevel = damageReducer.parametersPerLevel.Count - 1;
                break;
            case SkillType.DamageReflector:
                var damageReflector = skillWithLevel.skillGo.GetComponent<DamageReflector>();
                content = damageReflector;
                _levelupDescription.desc = damageReflector.GetLevelupDescription(skillWithLevel.level);
                _maxLevel = damageReflector.parametersPerLevel.Count - 1;
                break;
            case SkillType.ConditionReflector:
                var conditionReflector = skillWithLevel.skillGo.GetComponent<ConditionReflector>();
                content = conditionReflector;
                _levelupDescription.desc = conditionReflector.GetLevelupDescription(skillWithLevel.level);
                _maxLevel = conditionReflector.parametersPerLevel.Count - 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        TownEvents.OnSkillTreeRefresh += Refresh;
    }

    private void SetDesc()
    {
        _levelupDescription.desc = content.GetLevelupDescription(skillWithLevel.level);
    }

    private void SetActivity()
    {
        if (parents.Any(stn => stn.skillWithLevel.level < 0))
        {
            _button.interactable = false;
            _image.color = new Color(1, 1, 1, 0.5f);
            return;
        }

        _button.interactable = skillWithLevel.level != _maxLevel;
        _image.color = Color.white;
    }

    private void Refresh()
    {
        SetDesc();
        SetActivity();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
        TownEvents.OnSkillTreeRefresh -= Refresh;
    }
}
