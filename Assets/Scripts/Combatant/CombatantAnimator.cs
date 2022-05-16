using UnityEngine;

public class CombatantAnimator : MonoBehaviour
{
    private Animator _animator;
    private CombatantEvents _combatantEvents;
    
    private static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");
    private static readonly int TriggerAttacked = Animator.StringToHash("TriggerAttacked");
    private static readonly int TriggerDie = Animator.StringToHash("TriggerDie");
    private static readonly int Moving = Animator.StringToHash("Moving");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnMoveToTarget += StartMovementAnimation;
        _combatantEvents.OnReturn += StartMovementAnimation;
        _combatantEvents.OnFinishedMoving += StopMovementAnimation;
        _combatantEvents.OnAttack += TriggerAttackAnimation;
        _combatantEvents.OnHurt += TriggerHurtAnimation;
        _combatantEvents.OnDied += TriggerDieAnimation;
    }
    
    private void StartMovementAnimation()
    {
        _animator.SetBool(Moving, true);
    }

    private void StartMovementAnimation(Vector3 v)
    {
        _animator.SetBool(Moving, true);
    }

    private void StopMovementAnimation()
    {
        _animator.SetBool(Moving, false);
    }
    
    private void TriggerAttackAnimation()
    {
        _animator.SetTrigger(TriggerAttack);
    }

    private void TriggerHurtAnimation()
    {
        _animator.SetTrigger(TriggerAttacked);
    }

    private void TriggerDieAnimation()
    {
        _animator.SetTrigger(TriggerDie);
    }

    private void OnDestroy()
    {
        _combatantEvents.OnMoveToTarget -= StartMovementAnimation;
        _combatantEvents.OnReturn -= StartMovementAnimation;
        _combatantEvents.OnFinishedMoving -= StopMovementAnimation;
        _combatantEvents.OnAttack -= TriggerAttackAnimation;
        _combatantEvents.OnHurt -= TriggerHurtAnimation;
        _combatantEvents.OnDied -= TriggerDieAnimation;
    }
}
