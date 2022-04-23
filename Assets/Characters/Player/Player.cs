using System;
using Core.Events;
using Core.Stats;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPointerClickHandler
{
    public StatBlock stats;

    public GameEvent endTurnEvent;
    public GameIntEvent enemyAttackedEvent;
    public GameClickEvent clickEvent;

    private bool _myTurn = false;
    private int _currentDamage;
    private Animator _animator;
    private static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");
    private static readonly int TriggerAttacked = Animator.StringToHash("TriggerAttacked");

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }
    
    
    
    public void PlayTurn(int id)
    {
        if (id != 1) return;
        _myTurn = true;
        //_animator.SetTrigger(TriggerAttack);
    }

    private void Attack()
    {
        enemyAttackedEvent.Raise(stats.damage.value);
    }

    private void EndTurn()
    {
        _myTurn = false;
        endTurnEvent.Raise();
    }

    public void Attacked(int damage)
    {
        var relativeDamage = damage - stats.defense.value;
        _currentDamage = relativeDamage > 0 ? relativeDamage : 1;
        _animator.SetTrigger(TriggerAttacked);
    }

    private void TakeDamage()
    {
        stats.hp.value -= _currentDamage;
        _currentDamage = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent.Raise(eventData);
    }

    public void HandleClick(PointerEventData eventData)
    {
        // detect who was clicked, open menu, menu needs to deal with whatever option is chosen on the clicked target
        if (!_myTurn) return;
        if (eventData.pointerPress.TryGetComponent(typeof(EnemyScript), out Component enemyScript))
        {
            _animator.SetTrigger(TriggerAttack);
            //return;
        }
    }
}
