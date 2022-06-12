using System;
using Core.DataTypes;
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
    private IntegerVariable _gold;

    private void Start()
    {
        _gold = GameManager.Instance.gold;
        TownEvents.OnPublishTownInfo += RegisterBlacksmithInfo;
    }
    
    private void RegisterBlacksmithInfo(TownInfo townInfo, bool _, string __)
    {
        _blacksmithInfo = townInfo.blacksmithInfo;
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
        damageChangeText.text = $"+{_blacksmithInfo.statIncrement}";
        energyEfficiencyChangeText.text = $"-{_blacksmithInfo.statDecrement}";
    }

    public void StripDownWeapon()
    {
        if(!weaponStripDownButton.interactable) return;
        energyEfficiencyChangeText.text = $"+{_blacksmithInfo.statIncrement}";
        damageChangeText.text = $"-{_blacksmithInfo.statDecrement}";
    }
    
    public void BulkUpArmor()
    {
        if(!armorBulkUpButton.interactable) return;
        defenceChangeText.text = $"+{_blacksmithInfo.statIncrement}";
        speedChangeText.text = $"-{_blacksmithInfo.statDecrement}";
    }

    public void StripDownArmor()
    {
        if(!armorStripDownButton.interactable) return;
        speedChangeText.text = $"+{_blacksmithInfo.statIncrement}";
        defenceChangeText.text = $"-{_blacksmithInfo.statDecrement}";
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= RegisterBlacksmithInfo;
    }
}
