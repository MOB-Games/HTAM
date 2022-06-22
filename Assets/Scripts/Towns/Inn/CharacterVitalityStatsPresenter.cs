using Core.Enums;
using TMPro;
using UnityEngine;

public class CharacterVitalityStatsPresenter : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI energyText;
    
    private void Start()
    {
        TownEvents.OnCharacterSelected += PresentVitalityStats;
        TownEvents.OnStatChange += ChangeStat;
    }
    
    private void PresentVitalityStats(CharacterTownInfo character)
    {
        var stats = character.State.stats;
        hpText.text = stats.hp.value.ToString();
        hpText.color = Color.black;
        energyText.text = stats.energy.value.ToString();
        energyText.color = Color.black;
    }

    private void ChangeStat(StatType stat, int change)
    {
        var text = stat switch
        {
            StatType.Hp => hpText,
            StatType.Energy => energyText,
            _ => null
        };
        if (text != null)
        {
            text.text = (int.Parse(text.text) + change).ToString();
            text.color = Color.green;
        }
    }

    private void OnDestroy()
    {
        TownEvents.OnCharacterSelected -= PresentVitalityStats;
        TownEvents.OnStatChange -= ChangeStat;
    }
}
