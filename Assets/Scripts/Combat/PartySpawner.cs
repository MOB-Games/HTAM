using System;
using System.Collections.Generic;
using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    [CanBeNull] public GameObject partyMemberTopPrefab;
    [CanBeNull] public GameObject partyMemberBottomPrefab;

    private readonly Dictionary<CombatantId, GameObject> _partyMemberPrefabs = new Dictionary<CombatantId, GameObject>(3);

    private void Start()
    {
        _partyMemberPrefabs.Add(CombatantId.Player, playerPrefab);
        if (partyMemberTopPrefab != null)
            _partyMemberPrefabs.Add(CombatantId.PartyMemberTop, partyMemberTopPrefab);
        if (partyMemberBottomPrefab != null)
            _partyMemberPrefabs.Add(CombatantId.PartyMemberBottom, partyMemberBottomPrefab);
        CombatEvents.OnSpawnParty += Spawn;
    }

    private void OnValidate()
    {
        if (playerPrefab is null)
            throw new NullReferenceException("Player prefab is null");
    }

    private void Spawn()
    {
        var dir = CombatantInfo.Mirror ? Vector3.right : Vector3.left;
        foreach (var partyMemberPrefab in _partyMemberPrefabs)
        {
            var inst = Instantiate(partyMemberPrefab.Value, CombatantInfo.GetLocation(partyMemberPrefab.Key) + 5 * dir,
                Quaternion.identity);
            inst.GetComponent<ID>().id = partyMemberPrefab.Key;
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
