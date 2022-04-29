using Core.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    public class GameBattleActionEventListener : MonoBehaviour
    {
        public GameBattleActionEvent gameEvent;
        public UnityEvent<int, int, StatType> response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(int id, int change, StatType affectedStat)
        {
            response.Invoke(id, change, affectedStat);
        }
    }
}