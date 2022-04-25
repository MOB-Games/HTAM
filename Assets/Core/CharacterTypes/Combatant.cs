using Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.CharacterTypes
{
    public abstract class Combatant : MonoBehaviour, IPointerClickHandler
    {
        public int id = 2;
        public GameEvent endTurnEvent;
        public GameIntEvent attackEvent;
        public GameClickEvent clickEvent;

        protected bool MyTurn = false;
        protected int CurrentDamage;
        protected Animator Animator;
        protected static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");
        protected static readonly int TriggerAttacked = Animator.StringToHash("TriggerAttacked");

        private void OnEnable()
        {
            Animator = GetComponent<Animator>();
        }

        protected abstract int GetDefense();

        protected abstract int GetDamage();

        // this function should reduce the hp by _currentDamage
        protected abstract void ReduceHp();

        // this function should trigger when the battle system raises the next turn event
        public abstract void TurnStarted(int turnId);

        private void EndTurn()
        {
            MyTurn = false;
            endTurnEvent.Raise();
        }

        private void Attack()
        {
            attackEvent.Raise(GetDamage());
        }

        public void Attacked(int damage)
        {
            var relativeDamage = damage - GetDefense();
            CurrentDamage = relativeDamage > 0 ? relativeDamage : 1;
            Animator.SetTrigger(TriggerAttacked);
        }

        private void TakeDamage()
        {
            ReduceHp();
            CurrentDamage = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickEvent.Raise(eventData);
        }

    }
}
