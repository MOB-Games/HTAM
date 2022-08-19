using System;
using System.Collections.Generic;
using System.Linq;
using Core.SkillsAndConditions.PassiveSkills;
using Core.Stats;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.DataTypes
{
    [Serializable]
    public class CharacterSaveState
    {
        public int level;
        public int exp;
        public StatBlock stats;
        public List<SkillWithLevel> activeDefensiveSkills;
        public List<SkillWithLevel> activeOffensiveSkills;
        public PassiveSkills passiveSkills;
        public List<SkillWithLevel> skills;

        public CharacterSaveState(CharacterState state)
        {
            level = state.level;
            exp = state.exp;
            stats = new StatBlock();
            state.stats.LoadStats(stats);
            activeDefensiveSkills =
                state.activeDefensiveSkills.Select(s => new SkillWithLevel(s.skillGo, s.level)).ToList();
            activeOffensiveSkills =
                state.activeOffensiveSkills.Select(s => new SkillWithLevel(s.skillGo, s.level)).ToList();
            passiveSkills = state.passiveSkills.Copy();

            skills = new List<SkillWithLevel>();
            foreach (var skillTreeNode in state.skillTree.GetComponentsInChildren<SkillTreeNode>())
            {
                skills.Add(new SkillWithLevel(skillTreeNode.skillWithLevel.skillGo, skillTreeNode.skillWithLevel.level));
            }

            skills = state.skillTree.GetComponentsInChildren<SkillTreeNode>().Select(stn =>
                new SkillWithLevel(stn.skillWithLevel)).ToList();
        }
    }

    [Serializable]
    public class SaveSlot
    {
        public bool full;
        public int gold;
        public int currentPath;
        public int maxClearedPath;
        public List<CharacterGameInfo> characterGameInfos;
        public List<CharacterSaveState> characterStates;
    
        [CanBeNull] public GameObject playerPrefab;
        [CanBeNull] public GameObject partyMemberTopPrefab;
        [CanBeNull] public GameObject partyMemberBottomPrefab;

        public SaveSlot()
        {
            full = false;
            gold = currentPath = maxClearedPath = 0;
            characterGameInfos = new List<CharacterGameInfo>();
            characterStates = new List<CharacterSaveState>();
            playerPrefab = partyMemberBottomPrefab = partyMemberTopPrefab = null;
        }
    }

    [CreateAssetMenu]
    public class SaveSlots : ScriptableObject
    {
        public readonly List<SaveSlot> saveSlots = new(3)
        {
            new SaveSlot(), new SaveSlot(), new SaveSlot()
        };
    }
}
