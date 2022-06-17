using Core.DataTypes;
using Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithStatChangePresenter : MonoBehaviour
{
    public Button weaponBulkUpButton;
    public Button weaponStripDownButton;
    public Button armorBulkUpButton;
    public Button armorStripDownButton;
    public TextMeshProUGUI damageChangeText;
    public TextMeshProUGUI energyEfficiencyChangeText;
    public TextMeshProUGUI defenceChangeText;
    public TextMeshProUGUI speedChangeText;
    
    private BlacksmithInfo _blacksmithInfo;
    private StatType _advantage = StatType.None;
    private StatType _disadvantage = StatType.None;
    private IntegerVariable _gold;

    private void Start()
    {
        _gold = GameManager.Instance.gold;
        TownEvents.OnPublishTownInfo += RegisterBlacksmithInfo;
        TownEvents.OnOpenBlacksmith += RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith += UnregisterForSelectedCharacter;
    }
    
    private void RegisterBlacksmithInfo(TownInfo townInfo, bool _, string __)
    {
        _blacksmithInfo = townInfo.blacksmithInfo;
    }
    
    private void RegisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected += CharacterSelected;
    }
    
    private void CharacterSelected(CharacterTownInfo characterTownInfo)
    {
        var stats = characterTownInfo.State.stats;
        _advantage = stats.advantage;
        _disadvantage = stats.disadvantage;
    }
    
    private void UnregisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected -= CharacterSelected;
    }

    public void Reset()
    {
        damageChangeText.text = "";
        energyEfficiencyChangeText.text = "";
        defenceChangeText.text = "";
        speedChangeText.text = "";
    } 

    public void BulkUpWeapon()
    {
        if(!weaponBulkUpButton.interactable) return;
        damageChangeText.text = $"+{_blacksmithInfo.GetIncrement(StatType.Damage, _advantage, _disadvantage)}";
        energyEfficiencyChangeText.text = $"-{_blacksmithInfo.GetDecrement()}";
    }

    public void StripDownWeapon()
    {
        if(!weaponStripDownButton.interactable) return;
        energyEfficiencyChangeText.text = $"+{_blacksmithInfo.GetIncrement(StatType.EnergyEfficiency, _advantage, _disadvantage)}";
        damageChangeText.text = $"-{_blacksmithInfo.GetDecrement()}";
    }
    
    public void BulkUpArmor()
    {
        if(!armorBulkUpButton.interactable) return;
        defenceChangeText.text = $"+{_blacksmithInfo.GetIncrement(StatType.Defence, _advantage, _disadvantage)}";
        speedChangeText.text = $"-{_blacksmithInfo.GetDecrement()}";
    }

    public void StripDownArmor()
    {
        if(!armorStripDownButton.interactable) return;
        speedChangeText.text = $"+{_blacksmithInfo.GetIncrement(StatType.Speed, _advantage, _disadvantage)}";
        defenceChangeText.text = $"-{_blacksmithInfo.GetDecrement()}";
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= RegisterBlacksmithInfo;
        TownEvents.OnOpenBlacksmith -= RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith -= UnregisterForSelectedCharacter;
    }
}
