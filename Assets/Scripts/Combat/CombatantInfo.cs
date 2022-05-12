using System.Collections.Generic;
using Core.Enums;
using Core.Stats;
using UnityEngine;

public static class CombatantInfo
{
    private static readonly Dictionary<CombatantId, StatBlock> CombatantsStats = new();
    private static readonly Dictionary<CombatantId, Vector3> CombatantLocations = new()
    {
        { CombatantId.Player, new Vector3(-6, 0, 0) },
        { CombatantId.PartyMemberTop, new Vector3(-8, 3, 0) },
        { CombatantId.PartyMemberBottom, new Vector3(-8, -3, 0) },
        { CombatantId.EnemyCenter, new Vector3(6, 0, 0) },
        { CombatantId.EnemyTop, new Vector3(8, 3, 0) },
        { CombatantId.EnemyBottom, new Vector3(8, -3, 0) }
    };

    public static bool CombatantIsActive(CombatantId id)
    {
        return CombatantsStats.ContainsKey(id);
    } 

    public static void AddCombatant(CombatantId id, StatBlock stats)
    {
        CombatantsStats.Add(id, stats);
    }

    public static StatBlock GetStatBlock(CombatantId id)
    {
        if (CombatantsStats.ContainsKey(id))
            return CombatantsStats[id];
        throw new KeyNotFoundException($"Tried to get stats for {id} but no combatant had this id");
    }

    public static Vector3 GetLocation(CombatantId id)
    {
        if (CombatantLocations.ContainsKey(id))
            return CombatantLocations[id];
        throw new KeyNotFoundException($"Tried to get location for {id} but no combatant had this id");
    }

    public static void Reset()
    {
        CombatantsStats.Clear();
    }
}
