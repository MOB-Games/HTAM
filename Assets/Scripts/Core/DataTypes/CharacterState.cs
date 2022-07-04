using System;
using System.Collections.Generic;
using Core.SkillsAndConditions;
using Core.SkillsAndConditions.PassiveSkills;
using Core.Stats;
using UnityEngine;

namespace Core.DataTypes
{
    [Serializable]
    public class SkillWithLevel
    {
        public GameObject skillGo;
        public int level;

        public SkillWithLevel(GameObject gameObject = null)
        {
            skillGo = gameObject;
            level = 0;
        }
    }

    [CreateAssetMenu]
    public class CharacterState : ScriptableObject
    {
        public int level;
        public int exp;
        // inventory
        public StatBlockSO stats;
        public List<ConditionWithLevel> conditions;
        
        public List<SkillWithLevel> activeOffensiveSkills;
        public List<SkillWithLevel> activeDefensiveSkills;
        public PassiveSkills passiveSkills;
        public GameObject skillTree;

        public CharacterInitialState initialState;

        private void InitSkillTree()
        {
            foreach (var skillTreeNode in skillTree.GetComponentsInChildren<SkillTreeNode>())
                skillTreeNode.level.value = -1;
        }

        private void InitSkills()
        {
            InitSkillTree();
            foreach (var skillLevel in initialState.initialSkills)
                skillLevel.value = 0;
            
            activeOffensiveSkills.Clear();
            activeDefensiveSkills.Clear();
            foreach (var skillTreeNode in skillTree.GetComponentsInChildren<SkillTreeNode>())
            {
                if (skillTreeNode.level.value == 0)
                {
                    if (((Skill)(skillTreeNode.content)).offensive)
                        activeOffensiveSkills.Add(skillTreeNode.skillWithLevel);
                    else
                        activeDefensiveSkills.Add(skillTreeNode.skillWithLevel);
                }
            }
        }

        public void Init()
        {
            stats.Init(initialState.initialHp, initialState.initialEnergy, initialState.initialDamage,
                 initialState.initialDefense, initialState.initialSpeed);
            conditions.Clear();
            level = 0;
            exp = 0;
            InitSkills();
            
            passiveSkills.Init();
        }
    }
}
