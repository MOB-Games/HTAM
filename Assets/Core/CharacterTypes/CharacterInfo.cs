using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Stats;
using UnityEngine;

[CreateAssetMenu]
public class CharacterInfo : ScriptableObject
{
    public StatBlock playerStats;
    public StatBlock partyMember1Stats;
    public StatBlock partyMember2Stats;
    public StatBlock enemy1Stats;
    public StatBlock enemy2Stats;
    public StatBlock enemy3Stats;
    
    private readonly Dictionary<int, Vector3> _characterLocations = new()
    {
        { CharacterId.Player, new Vector3(-6, 0, 0) },
        { CharacterId.PartyMember1, new Vector3(-8, 3, 0) },
        { CharacterId.PartyMember2, new Vector3(-8, -3, 0) },
        { CharacterId.Enemy1, new Vector3(6, 0, 0) },
        { CharacterId.Enemy2, new Vector3(8, 3, 0) },
        { CharacterId.Enemy3, new Vector3(8, -3, 0) }
    };

    public StatBlock GetStatBlock(int id)
    {
        return id switch
        {
            CharacterId.Player => playerStats,
            CharacterId.PartyMember1 => partyMember1Stats,
            CharacterId.PartyMember2 => partyMember2Stats,
            CharacterId.Enemy1 => enemy1Stats,
            CharacterId.Enemy2 => enemy2Stats,
            CharacterId.Enemy3 => enemy3Stats,
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, $"{id} is not a character id")
        };
    }

    public Vector3 GetLocation(int id)
    {
        return _characterLocations[id];
    }
}
