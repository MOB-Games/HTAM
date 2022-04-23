using System;
using Core.Events;
using Core.Stats;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StatBlock stats;

    public GameEvent endTurnEvent;

    public GameIntEvent enemyAttackedEvent;

    private Animator _animator;

    private int _currentDamage;
    private static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void PlayTurn(int id)
    {
        if (id != 1) return;
        _animator.SetTrigger(TriggerAttack);
    }

    private void DoDamage()
    {
        enemyAttackedEvent.Raise(stats.damage.value);
    }

    private void EndTurn()
    {
        endTurnEvent.Raise();
    }

    public void Attacked(int damage)
    {
        var relativeDamage = damage - stats.defense.value;
        _currentDamage = relativeDamage > 0 ? relativeDamage : 1;
        _animator.Play("Attacked");
    }

    private void TakeDamage()
    {
        stats.hp.value -= _currentDamage;
        _currentDamage = 0;
    }
}
