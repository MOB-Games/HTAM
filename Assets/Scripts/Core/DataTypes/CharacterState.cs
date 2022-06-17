using System;
using System.Collections.Generic;
using Core.Stats;
using UnityEngine;

namespace Core.DataTypes
{
    [Serializable]
    public class SkillWithLevel
    {
        public GameObject skillGo;
        public int level;
    }

    [CreateAssetMenu]
    public class CharacterState : ScriptableObject
    {
        public int level;
        public int exp;
        // inventory
        public StatBlockSO stats;

        public List<ConditionWithLevel> conditions;
        // for now we edit the skills from the inspector, in the future once there is a skill tree, changes made in the
        // character editing window will change the skills and their levels
        public int initialNumOffensiveSkillsSlots;
        public int numOffensiveSkillsSlots;
        public List<SkillWithLevel> activeOffensiveSkills;
        public int initialNumDefensiveSkillsSlots;
        public int numDefensiveSkillsSlots;
        public List<SkillWithLevel> activeDefensiveSkills;
        
        // skill tree - should have all skills and their level, level -1 will signify locked skills. 

        public void Init()
        {
            stats.Init();
            conditions.Clear();
            level = 0;
            exp = 0;
            numOffensiveSkillsSlots = initialNumOffensiveSkillsSlots;
            numDefensiveSkillsSlots = initialNumDefensiveSkillsSlots;
        }
    }
}
