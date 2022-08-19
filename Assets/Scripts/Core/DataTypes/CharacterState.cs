using System;
using System.Collections.Generic;
using System.Linq;
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

        public SkillWithLevel(GameObject gameObject = null, int lvl = 0)
        {
            skillGo = gameObject;
            level = lvl;
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
            passiveSkills.Init();
            InitSkills();
        }

        public void Init(CharacterSaveState saveState)
        {
            stats.SaveStats(saveState.stats);
            conditions.Clear();
            level = saveState.level;
            exp = saveState.exp;
            passiveSkills = saveState.passiveSkills.Copy();
            activeOffensiveSkills.Clear();
            activeDefensiveSkills.Clear();

            foreach (var skillTreeNode in skillTree.GetComponentsInChildren<SkillTreeNode>())
            {
                var savedSkill = saveState.skills.Find(s => s.skillGo == skillTreeNode.skillWithLevel.skillGo);
                skillTreeNode.level.value = savedSkill.level;
                skillTreeNode.skillWithLevel.level = savedSkill.level;
                
                if (saveState.activeOffensiveSkills.Any(s => s.skillGo == skillTreeNode.skillWithLevel.skillGo))
                    activeOffensiveSkills.Add(skillTreeNode.skillWithLevel);
                else if (saveState.activeDefensiveSkills.Any(s => s.skillGo == skillTreeNode.skillWithLevel.skillGo))
                    activeDefensiveSkills.Add(skillTreeNode.skillWithLevel);
            }

            activeDefensiveSkills = activeDefensiveSkills.OrderBy(s =>
                saveState.activeDefensiveSkills.IndexOf(
                    saveState.activeDefensiveSkills.Find(s2 => s2.skillGo == s.skillGo))).ToList();
            
            activeOffensiveSkills = activeOffensiveSkills.OrderBy(s =>
                saveState.activeOffensiveSkills.IndexOf(
                    saveState.activeOffensiveSkills.Find(s2 => s2.skillGo == s.skillGo))).ToList();
        }
    }
}
