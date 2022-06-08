using Core.Stats;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithStatModifier : MonoBehaviour
{
    public Button weaponBulkUpButton;
    public Button weaponStripDownButton;
    public Button armorBulkUpButton;
    public Button armorStripDownButton;
    
    private BlacksmithInfo _blacksmithInfo;
    private CharacterTownInfo _selectedCharacterTownInfo;

    private void Start()
    {
        TownEvents.OnPublishTownInfo += RegisterBlacksmithInfo;
        TownEvents.OnOpenBlacksmith += RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith += UnregisterForSelectedCharacter;
    }

    private void RegisterBlacksmithInfo(TownInfo townInfo, string _)
    {
        _blacksmithInfo = townInfo.blacksmithInfo;
    }

    private void RegisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected += CharacterSelected;
    }

    private void UpdateActiveButtons()
    {
        if (_selectedCharacterTownInfo == null || _selectedCharacterTownInfo.Gold.value < _blacksmithInfo.price)
        {
            weaponBulkUpButton.interactable = false;
            weaponStripDownButton.interactable = false;
            armorBulkUpButton.interactable = false;
            armorStripDownButton.interactable = false;
            return;
        }
        weaponBulkUpButton.interactable =
            _selectedCharacterTownInfo.Stats.damage.value + _blacksmithInfo.statIncrement <=
            _blacksmithInfo.maxDamage &&
            _selectedCharacterTownInfo.Stats.energyEfficiency.value - _blacksmithInfo.statDecrement >=
            _blacksmithInfo.minEnergyEfficiency;
        weaponStripDownButton.interactable = 
            _selectedCharacterTownInfo.Stats.energyEfficiency.value + _blacksmithInfo.statIncrement <=
            _blacksmithInfo.maxEnergyEfficiency
            && _selectedCharacterTownInfo.Stats.damage.value - _blacksmithInfo.statDecrement >= 0;
        armorBulkUpButton.interactable =
            _selectedCharacterTownInfo.Stats.defense.value + _blacksmithInfo.statIncrement <= _blacksmithInfo.maxDefence
            && _selectedCharacterTownInfo.Stats.speed.value - _blacksmithInfo.statDecrement >= 0;
        armorStripDownButton.interactable =
            _selectedCharacterTownInfo.Stats.speed.value + _blacksmithInfo.statIncrement <= _blacksmithInfo.maxSpeed
            && _selectedCharacterTownInfo.Stats.defense.value - _blacksmithInfo.statDecrement >= 0;
    }

    private void CharacterSelected(CharacterTownInfo characterTownInfo)
    {
        _selectedCharacterTownInfo = characterTownInfo;
        UpdateActiveButtons();
    }
    
    private void UnregisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected -= CharacterSelected;
    }
    
    private void Pay(){
        _selectedCharacterTownInfo.Gold.value -= _blacksmithInfo.price;
    }

    private void IncStat(StatSO stat)
    {
        stat.value += _blacksmithInfo.statIncrement;
        stat.baseValue += _blacksmithInfo.statIncrement;
    }
    
    private void DecStat(StatSO stat)
    {
        stat.value -= _blacksmithInfo.statDecrement;
        stat.baseValue -= _blacksmithInfo.statDecrement;
    }

    private void ModifyStats(StatSO statToInc, StatSO statToDec)
    {
        Pay();
        IncStat(statToInc);
        DecStat(statToDec);
        UpdateActiveButtons();
    }

    public void BulkUpWeapon()
    {
        var stats = _selectedCharacterTownInfo.Stats;
        ModifyStats(stats.damage, stats.energyEfficiency);
    }

    public void StripDownWeapon()
    {
        var stats = _selectedCharacterTownInfo.Stats;
        ModifyStats(stats.energyEfficiency, stats.damage);
    }
    
    public void BulkUpArmor()
    {
        var stats = _selectedCharacterTownInfo.Stats;
        ModifyStats(stats.defense, stats.speed);
    }

    public void StripDownArmor()
    {
        var stats = _selectedCharacterTownInfo.Stats;
        ModifyStats(stats.speed, stats.defense);
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= RegisterBlacksmithInfo;
        TownEvents.OnOpenBlacksmith -= RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith -= UnregisterForSelectedCharacter;
    }
}
