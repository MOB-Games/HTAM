using Core.Enums;
using UnityEngine;

namespace Core.CharacterTypes
{
    public abstract class Enemy : Combatant
    {
        public int hp;
        public int energy;

        public int damage;
        public int defense;
        public int speed;

        public int minExp;
        public int maxExp;

        protected override int GetStat(StatType statType)
        {
            switch (statType)
            {
                case StatType.Hp:
                    return hp;
                case StatType.Energy:
                    return energy;
                case StatType.Damage:
                    return damage;
                case StatType.Defense:
                    return defense;
                case StatType.Speed:
                    return speed;
                
            }
            // avoiding error
            return 0;
        }

        protected override void ChangeStat(StatType statType, int change)
        {
            switch (statType)
            {
                case StatType.Hp:
                    hp += change;
                    break;
                case StatType.Energy:
                    energy += change;
                    break;
                case StatType.Damage:
                    damage += change;
                    break;
                case StatType.Defense:
                    defense += change;
                    break;
                case StatType.Speed:
                    speed += change;
                    break;
            }
        }

        protected abstract void PlayTurn();

        public override void TurnStarted(int turnId)
        {
            if (Id != turnId) return;
            MyTurn = true;
            PlayTurn();
        }

        public int ExpDrop()
        {
            return Random.Range(minExp, maxExp);
        }
    }
}
