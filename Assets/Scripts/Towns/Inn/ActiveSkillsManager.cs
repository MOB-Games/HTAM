using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillsManager : MonoBehaviour
{
    public Sprite lockedSkillHolder;
    public Sprite emptySkillHolder;
    public List<Image> offensiveSkillSprites;
    public List<Image> defensiveSkillSprites;

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
                offensiveSkillSprites[i].sprite = offensiveSkills[i].skillGo != null
                    ? offensiveSkills[i].skillGo.GetComponent<Image>().sprite
                    : emptySkillHolder;
            }
            else
                offensiveSkillSprites[i].sprite = lockedSkillHolder;
            
            // defence
            if (i < defensiveSkills.Count)
            {
                defensiveSkillSprites[i].sprite = defensiveSkills[i].skillGo != null
                    ? defensiveSkills[i].skillGo.GetComponent<Image>().sprite
                    : emptySkillHolder;
            }
            else
                defensiveSkillSprites[i].sprite = lockedSkillHolder;
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
