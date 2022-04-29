using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Enums;
using Core.Stats;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.CharacterTypes
{
    public class Playable : Combatant
    {
        public StatBlock stats;
        
        protected override int GetStat(StatType statType)
        {
            switch (statType)
            {
                case StatType.Hp:
                    return stats.hp.value;
                case StatType.Energy:
                    return stats.energy.value;
                case StatType.Damage:
                    return stats.damage.value;
                case StatType.Defense:
                    return stats.defense.value;
                case StatType.Speed:
                    return stats.speed.value;
                
            }
            // avoiding error
            return 0;
        }

        protected override void ChangeStat(StatType statType, int change)
        {
            switch (statType)
            {
                case StatType.Hp:
                    stats.hp.value += change;
                    break;
                case StatType.Energy:
                    stats.energy.value += change;
                    break;
                case StatType.Damage:
                    stats.damage.value += change;
                    break;
                case StatType.Defense:
                    stats.defense.value += change;
                    break;
                case StatType.Speed:
                    stats.speed.value += change;
                    break;
            }
        }

        public override void TurnStarted(int turnId)
        {
            if (Id != turnId) return;
            MyTurn = true;
        }

        public void HandleClick(PointerEventData eventData)
        {
            // detect who was clicked, open menu, menu needs to deal with whatever option is chosen on the clicked target
            if (!MyTurn) return;
            if (eventData.pointerPress.TryGetComponent(typeof(SquareEnemy), out Component enemyScript))
            {
                Animator.SetTrigger(TriggerAttack);
                //return;
            }
        }
    }
}
