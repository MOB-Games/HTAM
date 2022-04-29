using System;
using System.Threading.Tasks;
using Core.Enums;
using Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.CharacterTypes
{
    public abstract class Combatant : MonoBehaviour, IPointerClickHandler
    {
        public GameEvent endTurnEvent;
        public GameIntEvent attackEvent;
        public GameIntEvent diedEvent;
        public GameClickEvent clickEvent;

        protected bool MyTurn = false;
        protected int Id;
        protected Animator Animator;
        protected static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");
        protected static readonly int TriggerAttacked = Animator.StringToHash("TriggerAttacked");
        protected static readonly int TriggerDie = Animator.StringToHash("TriggerDie");

        private void OnEnable()
        {
            Animator = GetComponent<Animator>();
        }

        // returns the value of the corresponding stat (instead of having a getter for each stat
        protected abstract int GetStat(StatType statType);
        
        // changes the value of the corresponding stat (instead of having a getter for each stat
        protected abstract void ChangeStat(StatType statType, int change);

        // this function should trigger when the battle system raises the next turn event
        public abstract void TurnStarted(int turnId);

        private void EndTurn()
        {
            MyTurn = false;
            endTurnEvent.Raise();
        }

        private void Attack()
        {
            attackEvent.Raise(GetStat(StatType.Damage));
        }

        public void Attacked(int damage)
        {
            Animator.SetTrigger(TriggerAttacked);
            var relativeDamage = damage - GetStat(StatType.Defense);
            var takenDamage = relativeDamage > 0 ? relativeDamage : 1;
            ChangeStat(StatType.Hp, -takenDamage);
            if (GetStat(StatType.Hp) <= 0)
            {
                Debug.Log($"trigger die {Id}");
                Animator.SetTrigger(TriggerDie);
            }
        }

        public void Die()
        {
            diedEvent.Raise(Id);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickEvent.Raise(eventData);
        }

    }
}
