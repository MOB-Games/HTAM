using System;
using Core.Enums;


namespace Core.Stats
{
    [Serializable]
    public class StatBlock
    {
        public Stat hp;
        public Stat energy;
        public Stat energyPenalty;
        public Stat damage;
        public Stat defense;
        public Stat speed;
        
        public int GetStatValue(StatType stat)
        {
            return stat switch
            {
                StatType.Hp => hp.value,
                StatType.Energy => energy.value,
                StatType.EnergyPenalty => energyPenalty.value,
                StatType.Damage => damage.value,
                StatType.Defense => defense.value,
                StatType.Speed => speed.value,
                _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
            };
        }
    }
}
