using Core.DataTypes;
using Core.Stats;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public CharacterState state;
    private StatBlock _stats;

    private void Start()
    {
        _stats = GetComponent<StatModifier>().stats;
        state.stats.LoadStats(_stats);
        var activeSkills = GetComponent<ActiveSkills>();
        activeSkills.offensiveSkills = state.activeOffensiveSkills;
        activeSkills.defensiveSkills = state.activeDefensiveSkills;
        GetComponent<ConditionManager>().conditions = state.conditions;
        CombatEvents.OnFinalDrop += LootDrop;
    }

    private void LootDrop(Drop drop)
    {
        state.exp.value += drop.Exp;
        GameManager.Instance.gold.value += drop.Gold;
    }

    private void OnDestroy()
    {
        state.stats.SaveStats(_stats);
    }
}
