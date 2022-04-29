using Core.CharacterTypes;
using Core.Enums;
using UnityEngine;

public class SquareEnemy : Enemy
{
    private void Awake()
    {
        Id = CharacterId.Enemy1;
        MinExp = 2;
        MaxExp = 10;
    }
    protected override void PlayTurn()
    {
        Debug.Log($"trigger attack {Id}");
        Animator.SetTrigger(TriggerAttack);
    }
}
