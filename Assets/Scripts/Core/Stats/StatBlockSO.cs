using UnityEngine;

namespace Core.Stats
{
    [CreateAssetMenu]
    public class StatBlockSO : ScriptableObject
    {
        public StatSO hp;
        public StatSO energy;

        public StatSO damage;
        public StatSO defense;
        public StatSO speed;

        public void Reset()
        {
            hp.value = hp.baseValue;
            energy.value = energy.baseValue;
            damage.value = damage.baseValue;
            defense.value = defense.baseValue;
            speed.value = speed.baseValue;
        }

        public void SaveStats(StatBlock stats)
        {
            hp.baseValue = stats.hp.baseValue;
            energy.baseValue = stats.energy.baseValue;
            damage.baseValue = stats.damage.baseValue;
            defense.baseValue = stats.defense.baseValue;
            speed.baseValue = stats.speed.baseValue;
            
            hp.value = stats.hp.value;
            energy.value = stats.energy.value;
            damage.value = stats.damage.value;
            defense.value = stats.defense.value;
            speed.value = stats.speed.value;
        }

        public void LoadStats(StatBlock stats)
        {
            stats.hp.baseValue = hp.baseValue;
            stats.energy.baseValue = energy.baseValue;
            stats.damage.baseValue = damage.baseValue;
            stats.defense.baseValue = defense.baseValue;
            stats.speed.baseValue = speed.baseValue;
            
            stats.hp.value = hp.value;
            stats.energy.value = energy.value;
            stats.damage.value = damage.value;
            stats.defense.value = defense.value;
            stats.speed.value = speed.value;
        }
    }

}
