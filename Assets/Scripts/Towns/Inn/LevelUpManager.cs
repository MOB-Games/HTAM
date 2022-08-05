using System;
using System.Collections.Generic;
using Core.DataTypes;
using Core.Enums;
using Core.SkillsAndConditions.PassiveSkills;
using Core.Stats;
using TMPro;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpButton;
    public GameObject cancelButton;
    public GameObject doneButton;
    public TextMeshProUGUI pointsText;
    
    public GameObject hpUpButton;
    public GameObject energyUpButton;
    public GameObject damageUpButton;
    public GameObject defenseUpButton;
    public GameObject speedUpButton;

    public GameObject unlockOffSlot;
    public GameObject unlockDefSlot;
    public GameObject unlockTextGo;
    public TextMeshProUGUI unlockText;

    public GameObject areYouSure;
    public TextMeshProUGUI lostPoints;
    
    private TextMeshProUGUI _damageChangeText;
    private TextMeshProUGUI _defenseChangeText;
    private TextMeshProUGUI _speedChangeText;

    private static readonly List<int> ExpForLevel = new() { 0, 40, 100 };

    private const int StatPtsPerLevel = 3;
    private const int VitalityPtsPerLevel = 2;
    private const int SkillPtsPerLevel = 2;

    private int _potentialLevel;
    private int _statPts;
    private int _vitalityPts;
    private int _skillPts;
    
    private int _damageChange;
    private int _defenseChange;
    private int _speedChange;

    private int _offensiveSkillSlotsUnlocked;
    private int _defensiveSkillSlotsUnlocked;
    private bool _unlockOffensive;
    private readonly Dictionary<SkillWithLevel, int> _leveledUpSkills = new(3);

    private readonly StatBlock _stats = new();
    private PassiveSkills _passiveSkills;

    private CharacterTownInfo _selectedCharacterTownInfo;
    private CharacterState _selectedCharacterState;

    private void Start()
    {
        _statPts = _vitalityPts = _skillPts = 0;
        _damageChangeText = damageUpButton.GetComponentInChildren<TextMeshProUGUI>();
        _defenseChangeText = defenseUpButton.GetComponentInChildren<TextMeshProUGUI>();
        _speedChangeText = speedUpButton.GetComponentInChildren<TextMeshProUGUI>();
        TownEvents.OnOpenInn += RegisterForSelectedCharacter;
        TownEvents.OnSkillLevelUp += LevelupSkill;
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
            UndoChanges();
        areYouSure.SetActive(false);
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

    private void ShowPoints()
    {
        pointsText.text = $"Stat Points: {_statPts}\n" +
                          $"Vitality Points: {_vitalityPts}\n" +
                          $"Skill Points: {_skillPts}";
        pointsText.color = Color.white;
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
        defenseUpButton.SetActive(active);
        speedUpButton.SetActive(active);
    }

    private void SetUnlockButtons()
    {
        var cost = GetCostOfSlot();
        var active = _skillPts >= cost && cost >= 0;
        unlockOffSlot.SetActive(active);
        unlockDefSlot.SetActive(active);
    }

    public void LevelUp()
    {
        var levelUps = LevelsToProgress(_selectedCharacterState.level, _selectedCharacterState.exp);
        _potentialLevel = _selectedCharacterState.level + levelUps;
        _statPts = StatPtsPerLevel * levelUps;
        _vitalityPts = VitalityPtsPerLevel * levelUps;
        _skillPts = SkillPtsPerLevel * levelUps;
        _offensiveSkillSlotsUnlocked = _defensiveSkillSlotsUnlocked = 0;
        var advantage = _selectedCharacterState.stats.advantage;
        var disadvantage = _selectedCharacterState.stats.disadvantage;
        _damageChange = GameManager.GetStatIncrement(StatType.Damage, advantage, disadvantage);
        _defenseChange = GameManager.GetStatIncrement(StatType.Defense, advantage, disadvantage);
        _speedChange = GameManager.GetStatIncrement(StatType.Speed, advantage, disadvantage);
        _damageChangeText.text = $"+{_damageChange}";
        _defenseChangeText.text = $"+{_defenseChange}";
        _speedChangeText.text = $"+{_speedChange}";
        _selectedCharacterState.stats.LoadStats(_stats);
        _passiveSkills = _selectedCharacterState.passiveSkills.Copy();
        
        levelUpButton.SetActive(false);
        cancelButton.SetActive(true);
        doneButton.SetActive(true);
        SetVitalityButtons();
        SetStatButtons();
        SetUnlockButtons();
        ShowPoints();
        TownEvents.RefreshSkillTree(_potentialLevel);
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
            case StatType.Defense:
                change = _defenseChange;
                _stats.defense.baseValue += _defenseChange;
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
        ShowPoints();
    }

    public void IncStat(int statNumber)
    {
        IncStat((StatType)statNumber);
    }

    private void UndoChanges()
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
        
        foreach (var leveledUpSkill in _leveledUpSkills)
            leveledUpSkill.Key.level -= leveledUpSkill.Value;
        _leveledUpSkills.Clear();
        TownEvents.RefreshSkillTree(_selectedCharacterState.level);
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
        ShowPoints();
    }

    public void CancelUnlock()
    {
        unlockTextGo.SetActive(false);
    }

    private void LevelupSkill(SkillTreeNode skillTreeNode)
    {
        if (_skillPts == 0) return;
        var level = ++skillTreeNode.skillWithLevel.level;
        switch (skillTreeNode.content)
        {
            case DamageAdder damageAdder:
                _passiveSkills.damageAdder.addChance = damageAdder.parametersPerLevel[level].addChance;
                _passiveSkills.damageAdder.damageMultiplier = damageAdder.parametersPerLevel[level].damageMultiplier;
                _passiveSkills.damageAdderVFX = damageAdder.visualEffect;
                break;
            case ConditionAdder conditionAdder:
                _passiveSkills.conditionAdder.addChance = conditionAdder.parametersPerLevel[level].addChance;
                _passiveSkills.levelOfAddedCondition = level;
                if (level == 0)
                    _passiveSkills.conditionToAdd = conditionAdder.conditionGo; 
                break;
            case DamageReducer damageReducer:
                _passiveSkills.damageReducer.reductionChance = damageReducer.parametersPerLevel[level].reductionChance;
                _passiveSkills.damageReducer.reductionPercent = damageReducer.parametersPerLevel[level].reductionPercent;
                break;
            case DamageReflector damageReflector:
                _passiveSkills.damageReflector.reflectChance = damageReflector.parametersPerLevel[level].reflectChance;
                _passiveSkills.damageReflector.percentOfIncomingDamage =
                    damageReflector.parametersPerLevel[level].percentOfIncomingDamage;
                _passiveSkills.damageReflector.damageMultiplier =
                    damageReflector.parametersPerLevel[level].damageMultiplier;
                _passiveSkills.damageReflectorVFX = damageReflector.visualEffect;
                break;
            case ConditionReflector conditionReflector:
                _passiveSkills.conditionReflector.reflectChance = 
                    conditionReflector.parametersPerLevel[level].reflectChance;
                _passiveSkills.levelOfReflectedCondition = level;
                if (level == 0)
                    _passiveSkills.conditionToReflect = conditionReflector.conditionGo;  
                break;
        }

        if (_leveledUpSkills.ContainsKey(skillTreeNode.skillWithLevel))
            _leveledUpSkills[skillTreeNode.skillWithLevel]++;
        else
            _leveledUpSkills.Add(skillTreeNode.skillWithLevel, 1);

        _skillPts--;
        TownEvents.RefreshSkillTree(_potentialLevel);
        SetUnlockButtons();
        ShowPoints();
    }

    public void CancelLevelUp()
    {
        UndoChanges();
        _skillPts = _statPts = _vitalityPts = 0;
        levelUpButton.SetActive(true);
        cancelButton.SetActive(false);
        doneButton.SetActive(false);
        unlockOffSlot.SetActive(false);
        unlockDefSlot.SetActive(false);
        pointsText.color = Color.clear;
        TownEvents.CharacterSelected(_selectedCharacterTownInfo);
    }

    public void DoneLevelUp()
    {
        if (_skillPts == 0 && _statPts == 0 && _vitalityPts == 0)
            SubmitLevelUp();
        else
        {
            lostPoints.text = $"{_statPts} Stat Points\n" +
                              $"{_vitalityPts} Vitality Points\n" +
                              $"{_skillPts} Skill Points";
            areYouSure.SetActive(true);
        }
    }

    public void NotSure()
    {
        areYouSure.SetActive(false);
    }

    public void SubmitLevelUp()
    {
        areYouSure.SetActive(false);
        _selectedCharacterState.level = ConvertExpToLevel(_selectedCharacterState.exp);
        levelUpButton.SetActive(false);
        cancelButton.SetActive(false);
        doneButton.SetActive(false);
        pointsText.color = Color.clear;
        _selectedCharacterState.stats.SaveStats(_stats);
        _selectedCharacterState.stats.Reset();
        _selectedCharacterState.passiveSkills = _passiveSkills;
        _offensiveSkillSlotsUnlocked = _defensiveSkillSlotsUnlocked = 0;
        _skillPts = _vitalityPts = _statPts = 0;
        _leveledUpSkills.Clear();
        TownEvents.CharacterSelected(_selectedCharacterTownInfo);
    }

    private void OnDestroy()
    {
        TownEvents.OnOpenInn -= RegisterForSelectedCharacter;
        TownEvents.OnSkillLevelUp -= LevelupSkill;
        TownEvents.OnCloseInn -= UnregisterForSelectedCharacter;
    }
}
