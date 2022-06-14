using System.Collections.Generic;
using Core.Enums;
using Core.Stats;
using UnityEngine;

public class Dimensions
{
    public float Width;
    public float Height;

    public Dimensions(Vector3 dimensions)
    {
        Width = dimensions.x;
        Height = dimensions.y;
    }
}

public static class CombatantInfo
{
    public static bool Mirror;
    private static readonly Dictionary<CombatantId, StatBlock> CombatantsStats = new();
    private static readonly Dictionary<CombatantId, Dimensions> CombatantsDimensions = new();
    private static readonly Dictionary<CombatantId, Vector3> CombatantLocations = new()
    {
        { CombatantId.Player, new Vector3(-5.5f, -1.5f, 0) },
        { CombatantId.PartyMemberTop, new Vector3(-7.5f, 1f, 0) },
        { CombatantId.PartyMemberBottom, new Vector3(-7.5f, -4f, 0) },
        { CombatantId.EnemyCenter, new Vector3(5.5f, -1.5f, 0) },
        { CombatantId.EnemyTop, new Vector3(7.5f, 1f, 0) },
        { CombatantId.EnemyBottom, new Vector3(7.5f, -4f, 0) }
    };

    public static bool CombatantIsActive(CombatantId id)
    {
        return CombatantsStats.ContainsKey(id);
    } 

    public static void AddCombatant(GameObject combatant)
    {
        var id = combatant.GetComponent<CombatId>().id;
        var stats = combatant.GetComponent<StatModifier>().stats;
        var size = combatant.GetComponent<BoxCollider2D>().bounds.size;
        CombatantsStats.Add(id, stats);
        CombatantsDimensions.Add(id, new Dimensions(size));
    }

    public static StatBlock GetStatBlock(CombatantId id)
    {
        if (CombatantsStats.ContainsKey(id))
            return CombatantsStats[id];
        throw new KeyNotFoundException($"Tried to get stats for {id} but no combatant had this id");
    }
    
    public static Dimensions GetDimensions(CombatantId id)
    {
        if (CombatantsDimensions.ContainsKey(id))
            return CombatantsDimensions[id];
        throw new KeyNotFoundException($"Tried to get width for {id} but no combatant had this id");
    }

    public static Vector3 GetLocation(CombatantId id)
    {
        if (CombatantLocations.ContainsKey(id))
            return Mirror ? Vector3.Scale(CombatantLocations[id], new Vector3(-1,1,1)) : CombatantLocations[id];
        throw new KeyNotFoundException($"Tried to get location for {id} but no combatant had this id");
    }

    public static void Reset()
    {
        CombatantsStats.Clear();
        CombatantsDimensions.Clear();
    }
}
