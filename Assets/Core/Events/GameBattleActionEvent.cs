using System.Collections.Generic;
using Core.Enums;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(menuName = "Game Event (int, int, StatType")]
    public class GameBattleActionEvent : ScriptableObject
    {
        private List<GameBattleActionEventListener> listeners =
            new List<GameBattleActionEventListener>();

        public void Raise(int id, int change, StatType affectedStat)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised(id, change, affectedStat);
        }

        public void RegisterListener(GameBattleActionEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameBattleActionEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
