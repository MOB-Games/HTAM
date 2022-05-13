using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

public class BarsConnector : MonoBehaviour
{
    public CombatantId id;
    
    private HpBarModifier _hpBarModifier;
    private EnergyBarModifier _energyBarModifier;
    [CanBeNull] private CombatantEvents _combatantEvents;

    public void Connect(GameObject combatant)
    {
        _hpBarModifier = GetComponentInChildren<HpBarModifier>();
        _energyBarModifier = GetComponentInChildren<EnergyBarModifier>();
        var stats = combatant.GetComponent<StatModifier>().stats;
        _hpBarModifier.Init(stats.hp);
        _energyBarModifier.Init(stats.energy);
        _combatantEvents = combatant.GetComponent<CombatantEvents>();
        if (_combatantEvents != null) 
            _combatantEvents.OnStatChange += ModifyBars;
    }

    private void ModifyBars(StatType stat, int delta)
    {
        switch (stat)
        {
            case StatType.Hp:
                _hpBarModifier.Change(delta);
                break;
            case StatType.Energy:
                _energyBarModifier.Change(delta);
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
