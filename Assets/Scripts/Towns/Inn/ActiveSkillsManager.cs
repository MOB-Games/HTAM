using System;
using System.Collections.Generic;
using Core.SkillsAndConditions;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillAppearance
{
    public Image image;
    [HideInInspector]
    public string desc;
}

public class ActiveSkillsManager : MonoBehaviour
{
    public Sprite lockedSkillHolder;
    public Sprite emptySkillHolder;
    public List<SkillAppearance> offensiveSkillAppearances;
    public List<SkillAppearance> defensiveSkillAppearances;

    private CharacterTownInfo _selectedCharacterTownInfo;

    // Start is called before the first frame update
    private void Start()
    {
        TownEvents.OnOpenInn += RegisterForSelectedCharacter;
        TownEvents.OnCloseInn += UnregisterForSelectedCharacter;
        TownEvents.OnSlotUnlcoked += ShowActiveSkills;
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
            // offence
            if (i < offensiveSkills.Count)
            {
                if (offensiveSkills[i].skillGo != null)
                {
                    offensiveSkillAppearances[i].image.sprite = offensiveSkills[i].skillGo.GetComponent<Image>().sprite;
                    offensiveSkillAppearances[i].desc =
                        offensiveSkills[i].skillGo.GetComponent<Skill>().GetDescription();
                }
                else
                {
                    offensiveSkillAppearances[i].image.sprite = emptySkillHolder;
                    offensiveSkillAppearances[i].desc = "";
                }
            }
            else
                offensiveSkillAppearances[i].image.sprite = lockedSkillHolder;
            
            // defence
            if (i < defensiveSkills.Count)
            {
                if (defensiveSkills[i].skillGo != null)
                {
                    defensiveSkillAppearances[i].image.sprite = defensiveSkills[i].skillGo.GetComponent<Image>().sprite;
                    defensiveSkillAppearances[i].desc =
                        defensiveSkills[i].skillGo.GetComponent<Skill>().GetDescription();
                }
                else
                {
                    defensiveSkillAppearances[i].image.sprite = emptySkillHolder;
                    defensiveSkillAppearances[i].desc = "";
                }
            }
            else
                defensiveSkillAppearances[i].image.sprite = lockedSkillHolder;
        }
    }
    
    private void UnregisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected -= CharacterSelected;
    }

    private void OnDestroy()
    {
        TownEvents.OnOpenInn -= RegisterForSelectedCharacter;
        TownEvents.OnCloseInn -= UnregisterForSelectedCharacter;
        TownEvents.OnSlotUnlcoked -= ShowActiveSkills;
    }
}
