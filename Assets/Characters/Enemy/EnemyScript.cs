using System;
using System.Collections;
using System.Collections.Generic;
using Core.Events;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int hp;
    public int energy;
    
    public int damage;
    public int defense;
    public int speed;

    public GameIntEvent playerAttackedEvent;
    public GameEvent endTurnEvent;
    
    private Animator _animator;

    private int _currentDamage;
    private static readonly int TriggerAttack = Animator.StringToHash("TriggerEnemyAttack");

    private void OnEnable()
    {
        Debug.Log("enable");
        _animator = GetComponent<Animator>();
    }

    public void PlayTurn(int id)
    {
        Debug.Log("hi");
        if (id != 2) return;
        Debug.Log("there");
        _animator.SetTrigger(TriggerAttack);
    }

    private void DoDamage()
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
        _animator.Play("Attacked");
    }

    private void TakeDamage()
    {
        hp -= _currentDamage;
        _currentDamage = 0;
    }
}