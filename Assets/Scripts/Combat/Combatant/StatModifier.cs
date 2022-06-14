using System;
using Core.Enums;
using Core.Stats;
using UnityEngine;

public class StatModifier : MonoBehaviour
{
    public StatBlock stats;
    private CombatantEvents _combatantEvents;

    private void Start()
    {
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnStatChange += ModifyStat;
    }

    private void ModifyStat(StatType affectedStat, int delta, bool percentage)
    {
        switch (affectedStat)
        {
            case StatType.Hp:
                delta = GameManager.CalculateStatDelta(delta, percentage, stats.hp.baseValue);
                stats.hp.value += delta;
                if (stats.hp.value <= 0)
                {
                    stats.hp.value = 0;
                    _combatantEvents.Died();
                }
                else if (stats.hp.value > stats.hp.baseValue)
                    stats.hp.value = stats.hp.baseValue;
                break;
            case StatType.Energy:
                delta = GameManager.CalculateStatDelta(delta, percentage, stats.energy.baseValue, stats.energyEfficiency.value);
                stats.energy.value += delta;
                if (stats.energy.value <= 0)
                    stats.energy.value = 0;
                else if (stats.energy.value > stats.energy.baseValue)
                    stats.energy.value = stats.energy.baseValue;
                break;
            case StatType.EnergyEfficiency:
                delta = GameManager.CalculateStatDelta(delta, percentage, stats.energyEfficiency.baseValue);
                stats.energyEfficiency.value += delta;
                if (stats.energyEfficiency.value <= 0)
                    stats.energyEfficiency.value = 0;
                else if (stats.energyEfficiency.value > stats.energyEfficiency.baseValue)
                    stats.energyEfficiency.value = stats.energyEfficiency.baseValue;
                break;
            case StatType.Damage:
                delta = GameManager.CalculateStatDelta(delta, percentage, stats.damage.baseValue);
                stats.damage.value += delta;
                if (stats.damage.value <= 0)
                    stats.damage.value = 0;
                break;
            case StatType.Defence:
                delta = GameManager.CalculateStatDelta(delta, percentage, stats.defence.baseValue);
                stats.defence.value += delta;
                if (stats.defence.value <= 0)
                    stats.defence.value = 0;
                break;
            case StatType.Speed:
                delta = GameManager.CalculateStatDelta(delta, percentage, stats.speed.baseValue);
                stats.speed.value += delta;
                if (stats.speed.value <= 0)
                    stats.speed.value = 0;
                break;
            case StatType.None:
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(affectedStat), affectedStat, null);
        }
    }

    private void OnDestroy()
    {
        _combatantEvents.OnStatChange -= ModifyStat;
    }
}
