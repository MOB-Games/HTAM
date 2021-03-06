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
        var maxed = stats.damage.value + inc > _blacksmithInfo.maxDamage;
        weaponBulkUpButton.interactable = !maxed && stats.speed.value - dec >= 0;
        _weaponBulkUpText.text = weaponBulkUpButton.interactable ? "BULK-UP" : maxed ? "Maxed" : "Blocked";
        
        
        inc = GameManager.GetStatIncrement(StatType.Defense, stats.advantage, stats.disadvantage);
        maxed = stats.defense.value + inc > _blacksmithInfo.maxDefense;
        armorBulkUpButton.interactable = !maxed && stats.speed.value - dec >= 0;
        _armorBulkUpText.text = armorBulkUpButton.interactable ? "BULK-UP" : maxed ? "Maxed" : "Blocked";
        
        
        inc = GameManager.GetStatIncrement(StatType.Speed, stats.advantage, stats.disadvantage);
        maxed = stats.speed.value + inc > _blacksmithInfo.maxSpeed;
        weaponStripDownButton.interactable = !maxed && stats.damage.value - dec >= 0;
        _weaponStripDownText.text = weaponStripDownButton.interactable ? "strip-down" : maxed ? "Maxed" : "Blocked";
        
        
        armorStripDownButton.interactable = !maxed && stats.defense.value - dec >= 0;
        _weaponStripDownText.text = weaponStripDownButton.interactable ? "strip-down" : maxed ? "Maxed" : "Blocked";
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
        ModifyStats(StatType.Damage,stats.damage, stats.speed);
    }

    public void StripDownWeapon()
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        ModifyStats(StatType.Speed,stats.speed, stats.damage);
    }
    
    public void BulkUpArmor()
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        ModifyStats(StatType.Defense,stats.defense, stats.speed);
    }

    public void StripDownArmor()
    {
        var stats = _selectedCharacterTownInfo.State.stats;
        ModifyStats(StatType.Speed,stats.speed, stats.defense);
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= RegisterBlacksmithInfo;
        TownEvents.OnOpenBlacksmith -= RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith -= UnregisterForSelectedCharacter;
    }
}
