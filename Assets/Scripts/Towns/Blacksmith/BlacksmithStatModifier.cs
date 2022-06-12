using Core.DataTypes;
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
        weaponBulkUpButton.interactable =
            _selectedCharacterTownInfo.Stats.damage.value + _blacksmithInfo.statIncrement <=
            _blacksmithInfo.maxDamage &&
            _selectedCharacterTownInfo.Stats.energyEfficiency.value - _blacksmithInfo.statDecrement >=
            _blacksmithInfo.minEnergyEfficiency;
        _weaponBulkUpText.text = weaponBulkUpButton.interactable ? "BULK-UP" : "Maxed";
        weaponStripDownButton.interactable = 
            _selectedCharacterTownInfo.Stats.energyEfficiency.value + _blacksmithInfo.statIncrement <=
            _blacksmithInfo.maxEnergyEfficiency
            && _selectedCharacterTownInfo.Stats.damage.value - _blacksmithInfo.statDecrement >= 0;
        _weaponStripDownText.text = weaponStripDownButton.interactable ? "strip-down" : "Maxed";
        armorBulkUpButton.interactable =
            _selectedCharacterTownInfo.Stats.defense.value + _blacksmithInfo.statIncrement <= _blacksmithInfo.maxDefence
            && _selectedCharacterTownInfo.Stats.speed.value - _blacksmithInfo.statDecrement >= 0;
        _armorBulkUpText.text = armorBulkUpButton.interactable ? "BULK-UP" : "Maxed";
        armorStripDownButton.interactable =
            _selectedCharacterTownInfo.Stats.speed.value + _blacksmithInfo.statIncrement <= _blacksmithInfo.maxSpeed
            && _selectedCharacterTownInfo.Stats.defense.value - _blacksmithInfo.statDecrement >= 0;
        _armorStripDownText.text = armorStripDownButton.interactable ? "strip-down" : "Maxed";
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
        TownEvents.GoldSpent(_blacksmithInfo.price);
        IncStat(statToInc);
        DecStat(statToDec);
        TownEvents.CharacterSelected(_selectedCharacterTownInfo);
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
