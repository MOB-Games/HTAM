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

        private static void InitSkillTreeNodeRecursive(GameObject node)
        {
            foreach (Transform childTransform in node.transform)
                InitSkillTreeNodeRecursive(childTransform.gameObject);

            node.GetComponent<SkillTreeNode>().level.value = -1;
        }
        
        private void InitSkillTree()
        {
            foreach (Transform root in skillTree.transform)
                InitSkillTreeNodeRecursive(root.gameObject);
        }

        private void InitSkills()
        {
            InitSkillTree();
            foreach (var skillLevel in initialState.initialSkills)
                skillLevel.value = 0;
            
            activeOffensiveSkills.Clear();
            activeDefensiveSkills.Clear();
            foreach (Transform root in skillTree.transform)
            {
                var node = root.gameObject.GetComponent<SkillTreeNode>();
                if (node.level.value == 0)
                {
                    if (((Skill)(node.content)).offensive)
                        activeOffensiveSkills.Add(node.skillWithLevel);
                    else
                        activeDefensiveSkills.Add(node.skillWithLevel);
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
