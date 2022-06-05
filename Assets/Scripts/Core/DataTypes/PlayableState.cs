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
    public class PlayableState : ScriptableObject
    {
        public int level;
        public int exp;
        public int gold;
        // inventory
        public StatBlockSO stats;

        public List<ConditionWithLevel> conditions;
        // for now we edit the skills from the inspector, in the future once there is a skill tree, changes made in the
        // character editing window will change the skills and their levels
        public List<SkillWithLevel> activeOffensiveSkills;
        public List<SkillWithLevel> activeDefensiveSkills;
        
        // skill tree - should have all skills and their level, level -1 will signify locked skills. 
    }
}
