using System;
using Core.Enums;
using UnityEngine;

public class CombatantAudioManager : MonoBehaviour
{
    public Sound moveSound;
    public Sound attackSound;
    public Sound powerAttackSound;
    public Sound spellSound;
    public Sound hurtSound;
    public Sound defendSound;
    public Sound dieSound;
    
    private CombatantEvents _combatantEvents;
    
    private void Awake()
    {
        InitSound(moveSound);
        InitSound(attackSound);
        InitSound(powerAttackSound);
        InitSound(spellSound);
        InitSound(hurtSound);
        InitSound(defendSound);
        InitSound(dieSound);
    }

    private void Start()
    {
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnMoveToTarget += StartMovementAudio;
        _combatantEvents.OnReturn += StartMovementAudio;
        _combatantEvents.OnFinishedMoving += StopMovementAudio;
        _combatantEvents.OnAnimateSkill += TriggerSkillAudio;
        _combatantEvents.OnHurt += TriggerHurtAudio;
        _combatantEvents.OnDamageReduced += TriggerDefendAudio;
        _combatantEvents.OnDied += TriggerDieAudio;
    }

    private void InitSound(Sound sound)
    {
        if (sound.audioClip == null) return;
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.audioClip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }

    private void StartMovementAudio()
    {
        moveSound.source.Play();
    }

    private void StartMovementAudio(CombatantId _)
    {
        moveSound.source.Play();
    }

    private void StopMovementAudio()
    {
        moveSound.source.Stop();
    }

    private void TriggerSkillAudio(SkillAnimation skillAnimation)
    {
        switch (skillAnimation)
        {
            case SkillAnimation.Attack:
                attackSound.source.Play();
                break;
            case SkillAnimation.PowerAttack:
                powerAttackSound.source.Play();
                break;
            case SkillAnimation.Spell:
                spellSound.source.Play();
                break;
            case SkillAnimation.None:
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(skillAnimation), skillAnimation, null);
        }
    }

    private void TriggerHurtAudio()
    {
        hurtSound.source.Play();
    }

    private void TriggerDefendAudio()
    {
        defendSound.source.Play();
    }

    private void TriggerDieAudio()
    {
        dieSound.source.Play();
    }

    private void OnDestroy()
    {
        _combatantEvents.OnMoveToTarget -= StartMovementAudio;
        _combatantEvents.OnReturn -= StartMovementAudio;
        _combatantEvents.OnFinishedMoving -= StopMovementAudio;
        _combatantEvents.OnAnimateSkill -= TriggerSkillAudio;
        _combatantEvents.OnHurt -= TriggerHurtAudio;
        _combatantEvents.OnDamageReduced -= TriggerDefendAudio;
        _combatantEvents.OnDied -= TriggerDieAudio;
    }
}
