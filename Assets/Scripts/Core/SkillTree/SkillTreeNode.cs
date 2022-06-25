using System.Collections.Generic;
using System.Linq;
using Core.DataTypes;
using Core.SkillsAndConditions;
using UnityEngine;
using UnityEngine.UI;



public class SkillTreeNode : MonoBehaviour
{
    public List<SkillTreeNode> parents;
    public SkillWithLevel skillWithLevel;
    public SkillBase content;

    private int _maxLevel;
    private Button _button;
    private Image _image;
    private SkillLevelupDescription _levelupDescription;
    
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _button.onClick.AddListener(() => TownEvents.LevelupSkill(this));

        _levelupDescription = GetComponent<SkillLevelupDescription>();
        _levelupDescription.desc = content.GetLevelupDescription(skillWithLevel.level);
        _maxLevel = content.GetMaxLevel();

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
