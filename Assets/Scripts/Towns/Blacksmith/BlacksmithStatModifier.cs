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

    private int _baseDefense = 0;
    private int _baseDamage = 6;
    private int _baseSpeed = 10;
    private int _blacksmithStatInc = 2;
    private int _blacksmithStatDec = 1;
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
        var maxed = stats.damage.value + _blacksmithStatInc > _blacksmithInfo.maxStat + _baseDamage;
        weaponBulkUpButton.interactable = !maxed && stats.speed.value - _blacksmithStatDec >= 0;
        _weaponBulkUpText.text = weaponBulkUpButton.interactable ? "BULK-UP" : maxed ? "Maxed" : "Blocked";
        
        
        maxed = stats.defense.value + _blacksmithStatInc > _blacksmithInfo.maxStat + _baseDefense;
        armorBulkUpButton.interactable = !maxed && stats.speed.value - _blacksmithStatDec >= 0;
        _armorBulkUpText.text = armorBulkUpButton.interactable ? "BULK-UP" : maxed ? "Maxed" : "Blocked";
        
        
        maxed = stats.speed.value + _blacksmithStatInc > _blacksmithInfo.maxStat + _baseSpeed;
        weaponStripDownButton.interactable = !maxed && stats.damage.value - _blacksmithStatDec >= 0;
        _weaponStripDownText.text = weaponStripDownButton.interactable ? "strip-down" : maxed ? "Maxed" : "Blocked";
        
        
        armorStripDownButton.interactable = !maxed && stats.defense.value - _blacksmithStatDec >= 0;
        _armorStripDownText.text = armorStripDownButton.interactable ? "strip-down" : maxed ? "Maxed" : "Blocked";
    }

    private static void IncStat(StatType statType, StatSO stat)
    {
        stat.value += 2;
        stat.baseValue += 2;
    }
    
    private static void DecStat(StatSO stat)
    {
        stat.value -= 1;
        stat.baseValue -= 1;
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
