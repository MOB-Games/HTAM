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
        public StatBlock stats;
        public GameEvent endTurnEvent;
        public GameIntEvent attackEvent;
        public GameIntEvent diedEvent;
        public GameClickEvent clickEvent;

        protected bool MyTurn = false;
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
            attackEvent.Raise(stats.damage.value);
        }

        public void Attacked(int damage)
        {
            Animator.SetTrigger(TriggerAttacked);
            var relativeDamage = damage - stats.defense.value;
            var takenDamage = relativeDamage > 0 ? relativeDamage : 1;
            stats.hp.value -= takenDamage;
            if (stats.hp.value <= 0)
            {
                Animator.SetTrigger(TriggerDie);
            }
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
