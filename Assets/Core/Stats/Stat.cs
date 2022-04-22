using UnityEngine;

namespace Core.Stats
{
    [CreateAssetMenu]
    public class Stat : ScriptableObject
    {
        public int baseValue;
        public int value;
    }
}