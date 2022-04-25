using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Stats;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.CharacterTypes
{
    public class Playable : Combatant
    {
        public StatBlock stats;

        protected override int GetDefense()
        { return stats.defense.value; }

        protected override int GetDamage()
        { return stats.damage.value; }

        protected override void ReduceHp()
        { stats.hp.value -= CurrentDamage; }

        public override void TurnStarted(int turnId)
        {
            if (id != turnId) return;
            MyTurn = true;
        }

        public void HandleClick(PointerEventData eventData)
        {
            // detect who was clicked, open menu, menu needs to deal with whatever option is chosen on the clicked target
            if (!MyTurn) return;
            if (eventData.pointerPress.TryGetComponent(typeof(EnemyScript), out Component enemyScript))
            {
                Animator.SetTrigger(TriggerAttack);
                //return;
            }
        }
    }
}
