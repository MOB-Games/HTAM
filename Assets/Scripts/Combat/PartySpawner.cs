using System;
using System.Collections.Generic;
using Core.DataTypes;
using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    public CharacterDB characters;

    private readonly Dictionary<CombatantId, GameObject> _partyMemberPrefabs = new Dictionary<CombatantId, GameObject>(3);

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
        var dir = CombatantInfo.Mirror ? Vector3.right : Vector3.left;
        foreach (var partyMemberPrefab in _partyMemberPrefabs)
        {
            var inst = Instantiate(partyMemberPrefab.Value, CombatantInfo.GetLocation(partyMemberPrefab.Key) + 5 * dir,
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
