using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core.Events
{
    public class GameEventListenerTemplate<T> : MonoBehaviour
    {
        public GameEventTemplate<T> gameEvent;
        public UnityEvent<T> response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T param)
        {
            response.Invoke(param);
        }
    }
}

