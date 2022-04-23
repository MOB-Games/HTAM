using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
	public class GameEventTemplate<T> : ScriptableObject
	{
		private List<GameEventListenerTemplate<T>> listeners =
			new List<GameEventListenerTemplate<T>>();

		public void Raise(T param)
		{
			for (int i = listeners.Count - 1; i >= 0; i--)
				listeners[i].OnEventRaised(param);
		}

		public void RegisterListener(GameEventListenerTemplate<T> listener)
		{
			listeners.Add(listener);
		}

		public void UnregisterListener(GameEventListenerTemplate<T> listener)
		{
			listeners.Remove(listener);
		}
	}
}

