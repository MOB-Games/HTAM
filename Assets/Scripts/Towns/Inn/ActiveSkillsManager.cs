using System;
using System.Collections.Generic;
using Core.SkillsAndConditions;
using TMPro;
using Unity.VisualScripting;
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

    private static event Action<string> OnSkillDescChanged;
    
    public GameObject skillToolTip;
    private TextMeshProUGUI _skillToolTipText;
    
    private CharacterTownInfo _selectedCharacterTownInfo;

    private void Start()
    {
        _skillToolTipText = skillToolTip.GetComponentInChildren<TextMeshProUGUI>();
        TownEvents.OnOpenInn += RegisterForSelectedCharacter;
        TownEvents.OnCloseInn += UnregisterForSelectedCharacter;
        TownEvents.OnSlotUnlocked += ShowActiveSkills;
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
    }

    private void ShowActiveSkills()
    {
        var offensiveSkills = _selectedCharacterTownInfo.State.activeOffensiveSkills;
        var defensiveSkills = _selectedCharacterTownInfo.State.activeDefensiveSkills;
        for (var i = 0; i < 7; i++)
        {
            offensiveSkillAppearances[i].description.desc = "";
            defensiveSkillAppearances[i].description.desc = "";
            // offense
            if (i < offensiveSkills.Count)
            {
                if (offensiveSkills[i].skillGo != null)
                {
                    offensiveSkillAppearances[i].image.sprite = offensiveSkills[i].skillGo.GetComponent<Image>().sprite;
                    offensiveSkillAppearances[i].description.desc =
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
                    defensiveSkillAppearances[i].description.desc =
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
        if (offensive)
        {
            var offensiveSkills = _selectedCharacterTownInfo.State.activeOffensiveSkills;
            if (index >= offensiveSkills.Count) return;
            offensiveSkills[index] = skillTreeNode.skillWithLevel;
        }
        else
        {
            var defensiveSkills = _selectedCharacterTownInfo.State.activeDefensiveSkills;
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
        TownEvents.OnAddSkillToActive -= AddSkillToActive;
        OnSkillDescChanged -= ShowSkillDesc;
    }
}
