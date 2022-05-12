using System;
using Core.Enums;
using JetBrains.Annotations;
using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    [CanBeNull] public GameObject partyMemberTopPrefab;
    [CanBeNull] public GameObject partyMemberBottomPrefab;

    private void Start()
    {
        CombatEvents.OnLoadScene += Spawn;
    }

    private void OnValidate()
    {
        if (playerPrefab is null)
            throw new NullReferenceException("Player prefab is null");
    }

    private void Spawn()
    {
        var inst = Instantiate(playerPrefab, CombatantInfo.GetLocation(CombatantId.Player) + 6 * Vector3.left,
            Quaternion.identity);
        inst.GetComponent<ID>().id = CombatantId.Player;
        CombatantInfo.AddCombatant(CombatantId.Player, inst.GetComponent<StatModifier>().stats);
        
        if (partyMemberTopPrefab != null)
        {
            inst = Instantiate(partyMemberTopPrefab, CombatantInfo.GetLocation(CombatantId.PartyMemberTop) + 6 * Vector3.left,
                Quaternion.identity);
            inst.GetComponent<ID>().id = CombatantId.PartyMemberTop;
            CombatantInfo.AddCombatant(CombatantId.PartyMemberTop, inst.GetComponent<StatModifier>().stats);
        }
        if (partyMemberBottomPrefab != null)
        {
            inst = Instantiate(partyMemberBottomPrefab, CombatantInfo.GetLocation(CombatantId.PartyMemberBottom) + 6 * Vector3.left,
                Quaternion.identity);
            inst.GetComponent<ID>().id = CombatantId.PartyMemberBottom;
            CombatantInfo.AddCombatant(CombatantId.PartyMemberBottom, inst.GetComponent<StatModifier>().stats);
        }
    }

    private void OnDestroy()
    {
        CombatEvents.OnLoadScene -= Spawn;
    }
}
