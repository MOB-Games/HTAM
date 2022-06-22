using System.Collections.Generic;
using UnityEngine;

namespace Core.DataTypes
{
    [CreateAssetMenu]
    public class CharacterInitialState : ScriptableObject
    {
        public int initialHp;
        public int initialEnergy;

        public int initialDamage;
        public int initialDefence;
        public int initialSpeed;
        
        public List<GameObject> initialOffensiveSkills;
        public List<GameObject> initialDefensiveSkills;
        
        // skill tree - should have all skills and their level, level -1 will signify locked skills. 
    }
}