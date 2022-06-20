using Core.DataTypes;
using Core.Enums;
using Core.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithStatModifier : MonoBehaviour
{
    public Button weaponBulkUpButton;
    public Button weaponStripDownButton;
    public Button armorBulkUpButton;
    public Button armorStripDownButton;

    private IntegerVariable _gold;
    private BlacksmithInfo _blacksmithInfo;
    private CharacterTownInfo _selectedCharacterTownInfo;
    private TextMeshProUGUI _weaponBulkUpText;
    private TextMeshProUGUI _weaponStripDownText;
    private TextMeshProUGUI _armorBulkUpText;
    private TextMeshProUGUI _armorStripDownText;

    private void Start()
    {
        _gold = GameManager.Instance.gold;
        _weaponBulkUpText = weaponBulkUpButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _weaponStripDownText = weaponStripDownButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _armorBulkUpText = armorBulkUpButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _armorStripDownText = armorStripDownButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        TownEvents.OnPublishTownInfo += RegisterBlacksmithInfo;
        TownEvents.OnOpenBlacksmith += RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith += UnregisterForSelectedCharacter;
    }

    private void RegisterBlacksmithInfo(TownInfo townInfo, bool _, string __)
    {
        _blacksmithInfo = townInfo.blacksmithInfo;
        UpdateActiveButtonsByPrice();
    }

    private void RegisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected += CharacterSelected;
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

    private bool UpdateActiveButtonsByPrice()
    {
        if (_gold.value < _blacksmithInfo.price)
        {
            weaponBulkUpButton.interactable = false;
            _weaponBulkUpText.text = "Not Enough Gold";
            weaponStripDownButton.interactable = false;
            _weaponStripDownText.text = "Not Enough Gold";
            armorBulkUpButton.interactable = false;
            _armorBulkUpText.text = "Not Enough Gold";
            armorStripDownButton.interactable = false;
            _armorStripDownText.text = "Not Enough Gold";
            return true;
        }

        return false;
    }

    private void UpdateActiveButtons()
    {
        if (UpdateActiveButtonsByPrice()) return;
        if (_selectedCharacterTownInfo == null)
        {
            weaponBulkUpButton.interactable = false;
            weaponStripDownButton.interactable = false;
            armorBulkUpButton.interactable = false;
            armorStripDownButton.interactable = false;
            return;
        }

        var stats = _selectedCharacterTownInfo.State.stats;
        var dec = GameManager.GetStatDecrement();
        var inc = GameManager.GetStatIncrement(StatType.Damage, stats.advantage, stats.disadvantage);
        weaponBulkUpButton.interactable =
            stats.damage.value + inc <= _blacksmithInfo.maxDamage &&
            stats.energyEfficiency.value - dec >= _blacksmithInfo.minEnergyEfficiency;
        _weaponBulkUpText.text = weaponBulkUpButton.interactable ? "BULK-UP" : "Maxed";
        
        inc = GameManager.GetStatIncrement(StatType.EnergyEfficiency, stats.advantage, stats.disadvantage);
        weaponStripDownButton.interactable = 
            stats.energyEfficiency.value + inc <= _blacksmithInfo.maxEnergyEfficiency && stats.damage.value - dec >= 0;
        _weaponStripDownText.text = weaponStripDownButton.interactable ? "strip-down" : "Maxed";
        
        
        inc = GameManager.GetStatIncrement(StatType.Defence, stats.advantage, stats.disadvantage);
        armorBulkUpButton.interactable =
            stats.defence.value + inc <= _blacksmithInfo.maxDefence && stats.speed.value - dec >= 0;
        _armorBulkUpText.text = armorBulkUpButton.interactable ? "BULK-UP" : "Maxed";
        
        
        inc = GameManager.GetStatIncrement(StatType.Speed, stats.advantage, stats.disadvantage);
        armorStripDownButton.interactable =
            stats.speed.value + inc <= _blacksmithInfo.maxSpeed && stats.defence.value - dec >= 0;
        _armorStripDownText.text = armorStripDownButton.interactable ? "strip-down" : "Maxed";
    }

    private void IncStat(StatType statType, StatSO stat)
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        var inc = GameManager.GetStatIncrement(statType, stats.advantage, stats.disadvantage);
        stat.value += inc;
        stat.baseValue += inc;
    }
    
    private void DecStat(StatSO stat)
    {
        var dec = GameManager.GetStatDecrement();
        stat.value -= dec;
        stat.baseValue -= dec;
    }

    private void ModifyStats(StatType statToIncType, StatSO statToInc, StatSO statToDec)
    {
        TownEvents.GoldSpent(_blacksmithInfo.price);
        IncStat(statToIncType, statToInc);
        DecStat(statToDec);
        TownEvents.CharacterSelected(_selectedCharacterTownInfo);
    }

    public void BulkUpWeapon()
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        ModifyStats(StatType.Damage,stats.damage, stats.energyEfficiency);
    }

    public void StripDownWeapon()
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        ModifyStats(StatType.EnergyEfficiency,stats.energyEfficiency, stats.damage);
    }
    
    public void BulkUpArmor()
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        ModifyStats(StatType.Defence,stats.defence, stats.speed);
    }

    public void StripDownArmor()
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        ModifyStats(StatType.Speed,stats.speed, stats.defence);
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= RegisterBlacksmithInfo;
        TownEvents.OnOpenBlacksmith -= RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith -= UnregisterForSelectedCharacter;
    }
}
