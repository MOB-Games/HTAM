using System;
using Core.Enums;


namespace Core.Stats
{
    [Serializable]
    public class StatBlock
    {
        public Stat hp;
        public Stat energy;
        public Stat energyEfficiency;
        public Stat damage;
        public Stat defence;
        public Stat speed;
        
        public int GetStatValue(StatType stat)
        {
            return stat switch
            {
                StatType.Hp => hp.value,
                StatType.Energy => energy.value,
                StatType.EnergyEfficiency => energyEfficiency.value,
                StatType.Damage => damage.value,
                StatType.Defence => defence.value,
                StatType.Speed => speed.value,
                StatType.None => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
            };
        }
    }
}
