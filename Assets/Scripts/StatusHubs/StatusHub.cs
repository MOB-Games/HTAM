using Core.Enums;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class StatusHub : MonoBehaviour
{
    public CombatantId id;
    public HpBarModifier hpBarModifier;
    public EnergyBarModifier energyBarModifier;
    public TextMeshProUGUI nameText;
    
    [CanBeNull] private CombatantEvents _combatantEvents;

    public void Connect(GameObject combatant)
    {
        nameText.text = combatant.name.Split("(")[0];
        var stats = combatant.GetComponent<StatModifier>().stats;
        _combatantEvents = combatant.GetComponent<CombatantEvents>();
        if (_combatantEvents != null) 
            _combatantEvents.OnStatChange += ModifyBars;
        if (CombatantInfo.Mirror)
            transform.position = Vector3.Scale(transform.position,new Vector3(-1,1,1));
        hpBarModifier.Init(stats.hp);
        energyBarModifier.Init(stats.energy);
    }

    private void ModifyBars(StatType stat, int delta)
    {
        switch (stat)
        {
            case StatType.Hp:
                hpBarModifier.Change(delta);
                break;
            case StatType.Energy:
                energyBarModifier.Change(delta);
                break;
            case StatType.Damage:
                break;
            case StatType.Defense:
                break;
            case StatType.Speed:
                break;
            default:
                return;
        }
    }

    private void OnDestroy()
    {
        if (_combatantEvents != null)
            _combatantEvents.OnStatChange -= ModifyBars;
    }
}
