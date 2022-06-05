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
                delta = ChangeCalculator.Calculate(delta, percentage, stats.hp.baseValue);
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
                delta = ChangeCalculator.Calculate(delta, percentage, stats.energy.baseValue);
                stats.energy.value += delta;
                if (stats.energy.value <= 0)
                    stats.energy.value = 0;
                else if (stats.energy.value > stats.energy.baseValue)
                    stats.energy.value = stats.energy.baseValue;
                break;
            case StatType.Damage:
                delta = ChangeCalculator.Calculate(delta, percentage, stats.damage.baseValue);
                stats.damage.value += delta;
                if (stats.damage.value <= 0)
                    stats.damage.value = 0;
                break;
            case StatType.Defense:
                delta = ChangeCalculator.Calculate(delta, percentage, stats.defense.baseValue);
                stats.defense.value += delta;
                if (stats.defense.value <= 0)
                    stats.defense.value = 0;
                break;
            case StatType.Speed:
                delta = ChangeCalculator.Calculate(delta, percentage, stats.speed.baseValue);
                stats.speed.value += delta;
                if (stats.speed.value <= 0)
                    stats.speed.value = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(affectedStat), affectedStat, null);
        }
    }

    private void OnDestroy()
    {
        _combatantEvents.OnStatChange -= ModifyStat;
    }
}
