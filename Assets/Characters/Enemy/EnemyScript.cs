using Core;

public class EnemyScript : Enemy
{
    protected override void PlayTurn()
    {
        Animator.SetTrigger(TriggerAttack);
    }
}
