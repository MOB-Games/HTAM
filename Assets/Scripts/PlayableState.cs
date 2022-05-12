using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Stats;
using UnityEngine;

[Serializable]
public class SkillLevel
{
    public SkillId id;
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
    // for now we edit the skills from the inspector, in the future once there is a skill tree, changes made in the
    // character editing window will change the skill and their levels
    public List<SkillLevel> skillLevels;
    public List<GameObject> activeOffensiveSkills;
    public List<GameObject> activeDefensiveSkills;
    public List<GameObject> availableOffensiveSkills;
    public List<GameObject> availableDefensiveSkills;
    // skill tree
}
