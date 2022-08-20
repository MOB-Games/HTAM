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
    public TextMeshProUGUI defenseChangeText;
    public TextMeshProUGUI speedChangeText;
    
    private void Start()
    {
        TownEvents.OnOpenBlacksmith += RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith += UnregisterForSelectedCharacter;
    }

    private void RegisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected += CharacterSelected;
    }
    
    private void CharacterSelected(CharacterTownInfo characterTownInfo)
    {
        var stats = characterTownInfo.State.stats;
    }
    
    private void UnregisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected -= CharacterSelected;
    }

    public void Reset()
    {
        damageChangeText.text = "";
        defenseChangeText.text = "";
        speedChangeText.text = "";
    } 

    public void BulkUpWeapon()
    {
        if(!weaponBulkUpButton.interactable) return;
        damageChangeText.text = $"+2";
        speedChangeText.text = $"-1";
    }

    public void StripDownWeapon()
    {
        if(!weaponStripDownButton.interactable) return;
        speedChangeText.text = $"+2";
        damageChangeText.text = $"-1";
    }
    
    public void BulkUpArmor()
    {
        if(!armorBulkUpButton.interactable) return;
        defenseChangeText.text = $"+2";
        speedChangeText.text = $"-1";
    }

    public void StripDownArmor()
    {
        if(!armorStripDownButton.interactable) return;
        speedChangeText.text = $"+2";
        defenseChangeText.text = $"-1";
    }

    private void OnDestroy()
    {
        TownEvents.OnOpenBlacksmith -= RegisterForSelectedCharacter;
        TownEvents.OnCloseBlacksmith -= UnregisterForSelectedCharacter;
    }
}
