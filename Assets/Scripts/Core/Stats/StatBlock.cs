using System;
using Core.Enums;


namespace Core.Stats
{
    [Serializable]
    public class StatBlock
    {
        public Stat hp;
        public Stat energy;
        public Stat damage;
        public Stat defence;
        public Stat speed;

        public StatBlock()
        {
            hp = new Stat();
            energy = new Stat();
            damage = new Stat();
            defence = new Stat();
            speed = new Stat();
        }
        
        public int GetStatValue(StatType stat)
        {
            return stat switch
            {
                StatType.Hp => hp.value,
                StatType.Energy => energy.value,
                StatType.Damage => damage.value,
                StatType.Defence => defence.value,
                StatType.Speed => speed.value,
                StatType.None => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
            };
        }
        
        public int GetStatBaseValue(StatType stat)
        {
            return stat switch
            {
                StatType.Hp => hp.baseValue,
                StatType.Energy => energy.baseValue,
                StatType.Damage => damage.baseValue,
                StatType.Defence => defence.baseValue,
                StatType.Speed => speed.baseValue,
                StatType.None => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
            };
        }
    }
}
