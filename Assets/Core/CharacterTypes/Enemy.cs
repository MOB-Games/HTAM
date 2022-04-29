using Core.Enums;
using Core.Stats;
using UnityEngine;

namespace Core.CharacterTypes
{
    public abstract class Enemy : Combatant
    {
        // stats that the enemy wil start with
        public int hp;
        public int energy;
        public int damage;
        public int defense;
        public int speed;
        
        // range of exp this enemy can give
        public int minExp;
        public int maxExp;

        public void InitStats()
        {
            stats.hp.value = stats.hp.baseValue = hp;
            stats.energy.value = stats.energy.baseValue = energy;
            stats.damage.value = stats.damage.baseValue = damage;
            stats.defense.value = stats.defense.baseValue = defense;
            stats.speed.value = stats.speed.baseValue = speed;
        }

        protected abstract void PlayTurn();

        public override void TurnStarted(int turnId)
        {
            if (id != turnId) return;
            MyTurn = true;
            PlayTurn();
        }

        public int ExpDrop()
        {
            return Random.Range(minExp, maxExp);
        }
    }
}
