using System.Linq;
using Core.Enums;
using Core.Stats;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public PlayableState state;
    private StatBlock _stats;

    private void Start()
    {
        _stats = GetComponent<StatModifier>().stats;
        state.stats.LoadStats(_stats);
        var activeSkills = GetComponent<ActiveSkills>();
        activeSkills.SkillLevels =
            state.skillLevels.ToDictionary(keySelector: s => s.id, elementSelector: s => s.level);
        activeSkills.offensiveSkills = state.activeOffensiveSkills;
        activeSkills.defensiveSkills = state.activeDefensiveSkills;
        CombatEvents.OnFinalDrop += LootDrop;
    }

    private void LootDrop(Drop drop)
    {
        state.exp += drop.Exp;
        state.gold += drop.Gold;
    }

    private void OnDestroy()
    {
        state.stats.SaveStats(_stats);
    }
}
