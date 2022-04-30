using System;
using Core.Enums;
using Core.Events;
using Core.Stats;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.CharacterTypes
{
    public abstract class Combatant : MonoBehaviour, IPointerClickHandler
    {
        public int id;
        public CharacterInfo characterInfo;
        public GameEvent endTurnEvent;
        public GameBattleActionEvent actionEvent;
        public GameIntEvent diedEvent;
        public GameClickEvent clickEvent;

        protected bool MyTurn = false;
        protected int TargetId;
        protected StatBlock Stats;
        protected Animator Animator;
        protected static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");
        protected static readonly int TriggerAttacked = Animator.StringToHash("TriggerAttacked");
        protected static readonly int TriggerDie = Animator.StringToHash("TriggerDie");

        private void OnEnable()
        {
            Animator = GetComponent<Animator>();
        }

        // this function should trigger when the battle system raises the next turn event
        public abstract void TurnStarted(int turnId);

        private void EndTurn()
        {
            MyTurn = false;
            endTurnEvent.Raise();
        }

        private void Attack()
        {
            actionEvent.Raise(TargetId, -Stats.damage.value, StatType.Hp);
        }

        private void ChangeStat(int change, StatType affectedStat)
        {
            switch (affectedStat)
            {
                case StatType.Hp:
                    Stats.hp.value += change;
                    break;
                case StatType.Energy:
                    Stats.energy.value += change;
                    break;
                case StatType.Damage:
                    Stats.damage.value += change;
                    break;
                case StatType.Defense:
                    Stats.defense.value += change;
                    break;
                case StatType.Speed:
                    Stats.speed.value += change;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(affectedStat), affectedStat, null);
            }
        }

        public void ActionTaken(int targetId, int change, StatType affectedStat)
        {
            if (id != targetId) return;
            if (change < 0) // attacked or debuffed 
            {
                Animator.SetTrigger(TriggerAttacked);
                if (affectedStat == StatType.Hp)
                {
                    change += Stats.defense.value;
                    if (change >= 0) change = -1; // minimal damage
                    Stats.hp.value += change; // change is always negative
                    if (Stats.hp.value <= 0)
                    {
                        Animator.SetTrigger(TriggerDie);
                    }
                    return;
                }
            }
            else
            {
                // trigger a animation for healing or buffing
            }
            ChangeStat(change, affectedStat);
        }

        public void Die()
        {
            diedEvent.Raise(id);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickEvent.Raise(eventData);
        }

    }
}
