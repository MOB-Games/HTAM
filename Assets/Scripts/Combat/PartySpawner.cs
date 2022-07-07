using System;
using System.Collections.Generic;
using Core.DataTypes;
using Core.Enums;
using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    public CharacterDB characters;

    private readonly Dictionary<CombatantId, GameObject> _partyMemberPrefabs = new(3);

    private void Start()
    {
        _partyMemberPrefabs.Add(CombatantId.Player, characters.playerPrefab);
        if (characters.partyMemberTopPrefab != null)
            _partyMemberPrefabs.Add(CombatantId.PartyMemberTop, characters.partyMemberTopPrefab);
        if (characters.partyMemberBottomPrefab != null)
            _partyMemberPrefabs.Add(CombatantId.PartyMemberBottom, characters.partyMemberBottomPrefab);
        CombatEvents.OnSpawnParty += Spawn;
    }

    private void OnValidate()
    {
        if (characters.playerPrefab is null)
            throw new NullReferenceException("Player prefab is null");
    }

    private void Spawn()
    {
        foreach (var partyMemberPrefab in _partyMemberPrefabs)
        {
            var startingY = CombatantInfo.GetLocation(partyMemberPrefab.Key).y + 1.5f;
            var startingX = CombatantInfo.Mirror ? 10 : -10;
            var inst = Instantiate(partyMemberPrefab.Value,new Vector3(startingX, startingY, 0),
                Quaternion.identity);
            inst.GetComponent<CombatId>().id = partyMemberPrefab.Key;
            if (CombatantInfo.Mirror)
                inst.transform.localScale = Vector3.Scale(inst.transform.localScale, new Vector3(-1, 1, 1));
            CombatEvents.CombatantAdded(inst);
        }
    }

    private void OnDestroy()
    {
        CombatEvents.OnSpawnParty -= Spawn;
    }
}
