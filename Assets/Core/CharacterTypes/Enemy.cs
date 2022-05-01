using System;
using Core.Enums;
using Core.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

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
            Stats = characterInfo.GetStatBlock(id);
            Stats.hp.value = Stats.hp.baseValue = hp;
            Stats.energy.value = Stats.energy.baseValue = energy;
            Stats.damage.value = Stats.damage.baseValue = damage;
            Stats.defense.value = Stats.defense.baseValue = defense;
            Stats.speed.value = Stats.speed.baseValue = speed;
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
