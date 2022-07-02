using System;
using System.Collections.Generic;
using System.Linq;
using Core.SkillsAndConditions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillAppearance
{
    public Image image;
    public SkillLevelupDescription description;
}

public class ActiveSkillsManager : MonoBehaviour
{
    public Sprite lockedSkillHolder;
    public Sprite emptySkillHolder;
    public List<SkillAppearance> offensiveSkillAppearances;
    public List<SkillAppearance> defensiveSkillAppearances;
    public GameObject skillTree;

    private static event Action<string> OnSkillDescChanged;
    
    public GameObject skillToolTip;
    private TextMeshProUGUI _skillToolTipText;
    
    private CharacterTownInfo _selectedCharacterTownInfo;
    private GameObject _skillTreeInstance;

    private void Start()
    {
        _skillToolTipText = skillToolTip.GetComponentInChildren<TextMeshProUGUI>();
        TownEvents.OnOpenInn += RegisterForSelectedCharacter;
        TownEvents.OnCloseInn += UnregisterForSelectedCharacter;
        TownEvents.OnSlotUnlocked += ShowActiveSkills;
        TownEvents.OnSkillTreeRefresh += ShowActiveSkills;
        TownEvents.OnAddSkillToActive += AddSkillToActive;
        OnSkillDescChanged += ShowSkillDesc;
    }
    
    private void RegisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected += CharacterSelected;
    }

    private void CharacterSelected(CharacterTownInfo characterTownInfo)
    {
        _selectedCharacterTownInfo = characterTownInfo;
        ShowActiveSkills();
        Destroy(_skillTreeInstance);
        _skillTreeInstance = Instantiate(_selectedCharacterTownInfo.State.skillTree, skillTree.transform);
    }

    private void ShowActiveSkills()
    {
        var offensiveSkills = _selectedCharacterTownInfo.State.activeOffensiveSkills;
        var defensiveSkills = _selectedCharacterTownInfo.State.activeDefensiveSkills;
        for (var i = 0; i < 7; i++)
        {
            offensiveSkillAppearances[i].description.Desc = "";
            defensiveSkillAppearances[i].description.Desc = "";
            // offense
            if (i < offensiveSkills.Count)
            {
                if (offensiveSkills[i].skillGo != null)
                {
                    offensiveSkillAppearances[i].image.sprite = offensiveSkills[i].skillGo.GetComponent<Image>().sprite;
                    offensiveSkillAppearances[i].description.Desc =
                        offensiveSkills[i].skillGo.GetComponent<Skill>().GetLevelupDescription(offensiveSkills[i].level);
                }
                else
                    offensiveSkillAppearances[i].image.sprite = emptySkillHolder;
            }
            else
                offensiveSkillAppearances[i].image.sprite = lockedSkillHolder;

            // defense
            if (i < defensiveSkills.Count)
            {
                if (defensiveSkills[i].skillGo != null)
                {
                    defensiveSkillAppearances[i].image.sprite = defensiveSkills[i].skillGo.GetComponent<Image>().sprite;
                    defensiveSkillAppearances[i].description.Desc =
                        defensiveSkills[i].skillGo.GetComponent<Skill>().GetLevelupDescription(defensiveSkills[i].level);
                }
                else
                    defensiveSkillAppearances[i].image.sprite = emptySkillHolder;
            }
            else
                defensiveSkillAppearances[i].image.sprite = lockedSkillHolder;
        }
    }

    private void AddSkillToActive(SkillTreeNode skillTreeNode, int index, bool offensive)
    {
        var skillIsOffensive = ((Skill)(skillTreeNode.content)).offensive;
        if (offensive != skillIsOffensive) return;
        if (offensive)
        {
            var offensiveSkills = _selectedCharacterTownInfo.State.activeOffensiveSkills;
            var skillName = skillTreeNode.skillWithLevel.skillGo.name;
            if (offensiveSkills.Any(s => s.skillGo.name == skillName)) return;
            if (index >= offensiveSkills.Count) return;
            offensiveSkills[index] = skillTreeNode.skillWithLevel;
        }
        else
        {
            var defensiveSkills = _selectedCharacterTownInfo.State.activeDefensiveSkills;
            if (defensiveSkills.Contains(skillTreeNode.skillWithLevel)) return;
            if (index >= defensiveSkills.Count) return;
            defensiveSkills[index] = skillTreeNode.skillWithLevel;
        }
        
        ShowActiveSkills();
    }

    private void ShowSkillDesc(string desc)
    {
        if (string.IsNullOrEmpty(desc))
            skillToolTip.SetActive(false);
        else
        {
            _skillToolTipText.text = desc;
            skillToolTip.SetActive(true);
        }
    }
    
    private void UnregisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected -= CharacterSelected;
    }

    public static void SkillDescChanged(string desc)
    {
        OnSkillDescChanged?.Invoke(desc);
    }

    private void OnDestroy()
    {
        TownEvents.OnOpenInn -= RegisterForSelectedCharacter;
        TownEvents.OnCloseInn -= UnregisterForSelectedCharacter;
        TownEvents.OnSlotUnlocked -= ShowActiveSkills;
        TownEvents.OnSkillTreeRefresh -= ShowActiveSkills;
        TownEvents.OnAddSkillToActive -= AddSkillToActive;
        OnSkillDescChanged -= ShowSkillDesc;
    }
}
