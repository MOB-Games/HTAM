using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterVitalityStatsPresenter : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI energyText;
    
    private void Start()
    {
        TownEvents.OnCharacterSelected += PresentVitalityStats;
    }
    
    private void PresentVitalityStats(CharacterTownInfo character)
    {
        var stats = character.State.stats;
        hpText.text = stats.hp.value.ToString();
        energyText.text = stats.energy.value.ToString();
    }

    private void OnDestroy()
    {
        TownEvents.OnCharacterSelected -= PresentVitalityStats;
    }
}
