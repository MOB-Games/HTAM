using System;
using Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyScript : MonoBehaviour, IPointerClickHandler
{
    public int hp;
    public int energy;
    
    public int damage;
    public int defense;
    public int speed;

    public GameIntEvent playerAttackedEvent;
    public GameEvent endTurnEvent;
    public GameClickEvent clickEvent;
    
    private Animator _animator;

    private int _currentDamage;
    private static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");
    private static readonly int TriggerAttacked = Animator.StringToHash("TriggerAttacked");

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayTurn(int id)
    {
        if (id != 2) return;
        _animator.SetTrigger(TriggerAttack);
    }

    private void Attack()
    {
        playerAttackedEvent.Raise(damage);
    }

    private void EndTurn()
    {
        endTurnEvent.Raise();
    }
    
    public void Attacked(int damage)
    {
        var relativeDamage = damage - defense;
        _currentDamage = relativeDamage > 0 ? relativeDamage : 1;
        _animator.SetTrigger(TriggerAttacked);
    }

    private void TakeDamage()
    {
        hp -= _currentDamage;
        _currentDamage = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent.Raise(eventData);
    }
}
