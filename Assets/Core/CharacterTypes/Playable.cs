using UnityEngine.EventSystems;

namespace Core.CharacterTypes
{
    public class Playable : Combatant
    {
        private void Start()
        {
            Stats = characterInfo.GetStatBlock(id);
        }

        public override void TurnStarted(int turnId)
        {
            if (id != turnId) return;
            MyTurn = true;
        }

        public void HandleClick(PointerEventData eventData)
        {
            // open menu, menu needs to deal with whatever option is chosen on the clicked target
            if (!MyTurn) return;
            if (eventData.pointerPress.TryGetComponent(out Enemy enemyComponent))
            {
                TargetId = enemyComponent.id;
                StartCoroutine(AnimateAttack());
                //return;
            }
        }
    }
}
