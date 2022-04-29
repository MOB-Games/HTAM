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
        public override void TurnStarted(int turnId)
        {
            if (id != turnId) return;
            MyTurn = true;
        }

        public void HandleClick(PointerEventData eventData)
        {
            // detect who was clicked, open menu, menu needs to deal with whatever option is chosen on the clicked target
            if (!MyTurn) return;
            if (eventData.pointerPress.TryGetComponent(out Enemy enemyComponent))
            {
                TargetId = enemyComponent.id;
                Animator.SetTrigger(TriggerAttack);
                //return;
            }
        }
    }
}
