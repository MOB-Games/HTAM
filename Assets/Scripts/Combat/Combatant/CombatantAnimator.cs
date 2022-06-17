using System;
using Core.Enums;
using UnityEngine;

public class CombatantAnimator : MonoBehaviour
{
    private Animator _animator;
    private CombatantEvents _combatantEvents;
    
    private static readonly int TriggerAttack = Animator.StringToHash("TriggerAttack");
    private static readonly int TriggerPowerAttack = Animator.StringToHash("TriggerPowerAttack");
    private static readonly int TriggerSpell = Animator.StringToHash("TriggerSpell");
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
        _combatantEvents.OnAnimateSkill += TriggerSkillAnimation;
        _combatantEvents.OnHurt += TriggerHurtAnimation;
        _combatantEvents.OnDied += TriggerDieAnimation;
    }
    
    private void StartMovementAnimation()
    {
        _animator.SetBool(Moving, true);
    }

    private void StartMovementAnimation(CombatantId _)
    {
        _animator.SetBool(Moving, true);
    }

    private void StopMovementAnimation()
    {
        _animator.SetBool(Moving, false);
    }

    private void TriggerSkillAnimation(SkillAnimation skillAnimation)
    {
        switch (skillAnimation)
        {
            case SkillAnimation.Attack:
                _animator.SetTrigger(TriggerAttack);
                break;
            case SkillAnimation.PowerAttack:
                _animator.SetTrigger(TriggerPowerAttack);
                break;
            case SkillAnimation.Spell:
                _animator.SetTrigger(TriggerSpell);
                break;
            case SkillAnimation.None:
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(skillAnimation), skillAnimation, null);
        }
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
        _combatantEvents.OnAnimateSkill -= TriggerSkillAnimation;
        _combatantEvents.OnHurt -= TriggerHurtAnimation;
        _combatantEvents.OnDied -= TriggerDieAnimation;
    }
}
