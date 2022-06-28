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
        public int initialDefense;
        public int initialSpeed;

        public List<IntegerVariable> initialSkills;
    }
}