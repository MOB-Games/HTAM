using Core.CharacterTypes;
using Core.Enums;
using UnityEngine;

public class SquareEnemy : Enemy
{
    private void Awake()
    {
        id = CharacterId.Enemy1;
        minExp = 2;
        maxExp = 10;
    }
    protected override void PlayTurn()
    {
        TargetId = CharacterId.Player;
        Animator.SetTrigger(TriggerAttack);
    }
}
