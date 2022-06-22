using System;
using System.Collections.Generic;
using Core.DataTypes;
using Core.Enums;
using Core.Stats;
using TMPro;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpButton;
    public GameObject cancelButton;
    public GameObject doneButton;
    
    public GameObject hpUpButton;
    public GameObject energyUpButton;
    public GameObject damageUpButton;
    public GameObject defenceUpButton;
    public GameObject speedUpButton;

    public GameObject unlockOffSlot;
    public GameObject unlockDefSlot;
    public GameObject unlockTextGo;
    public TextMeshProUGUI unlockText;
    
    private TextMeshProUGUI _damageChangeText;
    private TextMeshProUGUI _defenceChangeText;
    private TextMeshProUGUI _speedChangeText;

    private static readonly List<int> ExpForLevel = new() { 0, 40, 100 };

    private const int StatPtsPerLevel = 3;
    private const int VitalityPtsPerLevel = 2;
    private const int SkillPtsPerLevel = 3;

    private int _statPts;
    private int _vitalityPts;
    private int _skillPts;
    
    private int _damageChange;
    private int _defenceChange;
    private int _speedChange;

    private int _offensiveSkillSlotsUnlocked;
    private int _defensiveSkillSlotsUnlocked;
    private bool _unlockOffensive;

    private readonly StatBlock _stats = new StatBlock();

    private CharacterTownInfo _selectedCharacterTownInfo;
    private CharacterState _selectedCharacterState;

    private void Start()
    {
        _damageChangeText = damageUpButton.GetComponentInChildren<TextMeshProUGUI>();
        _defenceChangeText = defenceUpButton.GetComponentInChildren<TextMeshProUGUI>();
        _speedChangeText = speedUpButton.GetComponentInChildren<TextMeshProUGUI>();
        TownEvents.OnOpenInn += RegisterForSelectedCharacter;
        TownEvents.OnCloseInn += UnregisterForSelectedCharacter;
    }

    private static int ConvertExpToLevel(int exp)
    {
        for (var i = 0; i < ExpForLevel.Count - 1; i++)
        {
            if (ExpForLevel[i] <= exp && exp < ExpForLevel[i + 1])
                return i;
        }

        return ExpForLevel.Count - 1;
    }

    private static int LevelsToProgress(int level, int exp)
    {
        return ConvertExpToLevel(exp) - level;
    }
    
    private void RegisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected += CharacterSelected;
    }

    private int GetCostOfSlot()
    {
        var numSlots = _selectedCharacterState.activeOffensiveSkills.Count +
                       _selectedCharacterState.activeDefensiveSkills.Count;
        var cost = ((numSlots - 3) / 3) + 1;
        return cost <= 3 ? cost : -1;
    }
    
    private void CharacterSelected(CharacterTownInfo characterTownInfo)
    {
        if (_selectedCharacterState != null)
            UndoSkillSlotChanges();
        cancelButton.SetActive(false);
        doneButton.SetActive(false);
        _statPts = 0;
        _vitalityPts = 0;
        _skillPts = 0;
        _selectedCharacterTownInfo = characterTownInfo;
        _selectedCharacterState = characterTownInfo.State;
        levelUpButton.SetActive(LevelsToProgress(_selectedCharacterState.level, _selectedCharacterState.exp) > 0);
        SetStatButtons();
        SetVitalityButtons();
        SetUnlockButtons();
    }
    
    private void UnregisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected -= CharacterSelected;
    }

    private void SetVitalityButtons()
    {
        var active = _vitalityPts > 0;
        hpUpButton.SetActive(active);
        energyUpButton.SetActive(active);
    }

    private void SetStatButtons()
    {
        var active = _statPts > 0;
        damageUpButton.SetActive(active);
        defenceUpButton.SetActive(active);
        speedUpButton.SetActive(active);
    }

    private void SetUnlockButtons()
    {
        var active = _skillPts >= GetCostOfSlot();
        unlockOffSlot.SetActive(active);
        unlockDefSlot.SetActive(active);
    }

    public void LevelUp()
    {
        var levelUps = LevelsToProgress(_selectedCharacterState.level, _selectedCharacterState.exp);
        _statPts = StatPtsPerLevel * levelUps;
        _vitalityPts = VitalityPtsPerLevel * levelUps;
        _skillPts = SkillPtsPerLevel * levelUps;
        _offensiveSkillSlotsUnlocked = _defensiveSkillSlotsUnlocked = 0;
        var advantage = _selectedCharacterState.stats.advantage;
        var disadvantage = _selectedCharacterState.stats.disadvantage;
        _damageChange = GameManager.GetStatIncrement(StatType.Damage, advantage, disadvantage);
        _defenceChange = GameManager.GetStatIncrement(StatType.Defence, advantage, disadvantage);
        _speedChange = GameManager.GetStatIncrement(StatType.Speed, advantage, disadvantage);
        _damageChangeText.text = $"+{_damageChange}";
        _defenceChangeText.text = $"+{_defenceChange}";
        _speedChangeText.text = $"+{_speedChange}";
        _selectedCharacterState.stats.LoadStats(_stats);
        
        levelUpButton.SetActive(false);
        cancelButton.SetActive(true);
        SetVitalityButtons();
        SetStatButtons();
        SetUnlockButtons();
    }

    private bool DoneLevelingUp()
    {
        return _statPts == 0 && _vitalityPts == 0 && _skillPts == 0;
    }

    private void IncStat(StatType stat)
    {
        var change = 0;
        switch (stat)
        {
            case StatType.Hp:
                change = 5;
                _stats.hp.baseValue += 5;
                _vitalityPts--;
                SetVitalityButtons();
                break;
            case StatType.Energy:
                change = 5;
                _stats.energy.baseValue += 5;
                _vitalityPts--;
                SetVitalityButtons();
                break;
            case StatType.Damage:
                change = _damageChange;
                _stats.damage.baseValue += _damageChange;
                _statPts--;
                SetStatButtons();
                break;
            case StatType.Defence:
                change = _defenceChange;
                _stats.defence.baseValue += _defenceChange;
                _statPts--;
                SetStatButtons();
                break;
            case StatType.Speed:
                change = _speedChange;
                _stats.speed.baseValue += _speedChange;
                _statPts--;
                SetStatButtons();
                break;
            case StatType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }
        TownEvents.StatChange(stat, change);
        doneButton.SetActive(DoneLevelingUp());
    }

    public void IncStat(int statNumber)
    {
        IncStat((StatType)statNumber);
    }

    private void UndoSkillSlotChanges()
    {
        if (_offensiveSkillSlotsUnlocked > 0)
        {
            _selectedCharacterState.activeOffensiveSkills.RemoveRange(
                _selectedCharacterState.activeOffensiveSkills.Count - _offensiveSkillSlotsUnlocked,
                _offensiveSkillSlotsUnlocked);
            _offensiveSkillSlotsUnlocked = 0;
        }

        if (_defensiveSkillSlotsUnlocked > 0)
        {
            _selectedCharacterState.activeDefensiveSkills.RemoveRange(
                _selectedCharacterState.activeDefensiveSkills.Count,
                _defensiveSkillSlotsUnlocked);
            _defensiveSkillSlotsUnlocked = 0;
        }
    }

    public void AskUnlock(bool offensive)
    {
        var cost = GetCostOfSlot();
        if (cost < 0) return;
        _unlockOffensive = offensive;
        unlockText.text = $"Use {cost} skill points to unlock this skill slot?";
        unlockTextGo.SetActive(true);
    }

    public void Unlock( )
    {
        _skillPts -= GetCostOfSlot();
        if (_unlockOffensive)
        {
            _offensiveSkillSlotsUnlocked++;
            _selectedCharacterState.activeOffensiveSkills.Add(new SkillWithLevel());
        }
        else
        {
            _defensiveSkillSlotsUnlocked++;
            _selectedCharacterState.activeDefensiveSkills.Add(new SkillWithLevel());
        }
        unlockTextGo.SetActive(false);
        TownEvents.SlotUnlocked();
        SetUnlockButtons();
        doneButton.SetActive(DoneLevelingUp());
    }

    public void CancelUnlock()
    {
        unlockTextGo.SetActive(false);
    }

    public void CancelLevelUp()
    {
        UndoSkillSlotChanges();
        levelUpButton.SetActive(true);
        cancelButton.SetActive(false);
        doneButton.SetActive(false);
        TownEvents.CharacterSelected(_selectedCharacterTownInfo);
    }

    public void SubmitLevelUp()
    {
        _selectedCharacterState.level = ConvertExpToLevel(_selectedCharacterState.exp);
        levelUpButton.SetActive(false);
        cancelButton.SetActive(false);
        doneButton.SetActive(false);
        _selectedCharacterState.stats.SaveStats(_stats);
        _selectedCharacterState.stats.Reset();
        _offensiveSkillSlotsUnlocked = _defensiveSkillSlotsUnlocked = 0;
        TownEvents.CharacterSelected(_selectedCharacterTownInfo);
    }

    private void OnDestroy()
    {
        TownEvents.OnOpenInn -= RegisterForSelectedCharacter;
        TownEvents.OnCloseInn -= UnregisterForSelectedCharacter;
    }
}
