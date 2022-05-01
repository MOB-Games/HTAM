using Core.CharacterTypes;
using Core.Enums;

public class DiamondEnemy : Enemy
{
    private void Awake()
    {
        minExp = 15;
        maxExp = 30;
    }
    protected override void PlayTurn()
    {
        TargetId = CharacterId.Player;
        Animator.SetTrigger(TriggerAttack);
    }
}
