using UnityEngine;

namespace Core.Stats
{
    [CreateAssetMenu]
    public class StatBlock : ScriptableObject
    {
        public Stat hp;
        public Stat energy;

        public Stat damage;
        public Stat defense;
        public Stat speed;
    }

}
