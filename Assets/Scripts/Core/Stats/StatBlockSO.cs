using Core.Enums;
using UnityEngine;

namespace Core.Stats
{
    [CreateAssetMenu]
    public class StatBlockSO : ScriptableObject
    {
        public StatSO hp;
        public StatSO energy;

        public StatSO damage;
        public StatSO defence;
        public StatSO speed;

        public StatType advantage;
        public StatType disadvantage;

        public void Init(int initialHp, int initialEnergy, int initialDamage, int initialDefence, int initialSpeed)
        {
            hp.baseValue = initialHp;
            energy.baseValue = initialEnergy;
            damage.baseValue = initialDamage;
            defence.baseValue = initialDefence;
            speed.baseValue = initialSpeed;
            
            Reset();
        }

        public void Reset()
        {
            hp.value = hp.baseValue;
            energy.value = energy.baseValue;
            damage.value = damage.baseValue;
            defence.value = defence.baseValue;
            speed.value = speed.baseValue;
        }

        public void SaveStats(StatBlock stats)
        {
            hp.baseValue = stats.hp.baseValue;
            energy.baseValue = stats.energy.baseValue;
            damage.baseValue = stats.damage.baseValue;
            defence.baseValue = stats.defence.baseValue;
            speed.baseValue = stats.speed.baseValue;
            
            hp.value = stats.hp.value;
            energy.value = stats.energy.value;
            damage.value = stats.damage.value;
            defence.value = stats.defence.value;
            speed.value = stats.speed.value;
        }

        public void LoadStats(StatBlock stats)
        {
            stats.hp.baseValue = hp.baseValue;
            stats.energy.baseValue = energy.baseValue;
            stats.damage.baseValue = damage.baseValue;
            stats.defence.baseValue = defence.baseValue;
            stats.speed.baseValue = speed.baseValue;
            
            stats.hp.value = hp.value;
            stats.energy.value = energy.value;
            stats.damage.value = damage.value;
            stats.defence.value = defence.value;
            stats.speed.value = speed.value;
        }
    }

}
