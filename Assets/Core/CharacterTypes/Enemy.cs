
namespace Core
{
    public abstract class Enemy : Combatant
    {
        public int hp;
        public int energy;

        public int damage;
        public int defense;
        public int speed;

        protected override int GetDefense()
        {
            return defense;
        }

        protected override int GetDamage()
        {
            return damage;
        }

        protected override void ReduceHp()
        {
            hp -= CurrentDamage;
        }

        protected abstract void PlayTurn();

        public override void TurnStarted(int turnId)
        {
            if (id != turnId) return;
            MyTurn = true;
            PlayTurn();
        }
    }
}
