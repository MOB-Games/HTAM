using System.Collections.Generic;
using Core.Enums;
using UnityEngine;

public class StatusHubManager : MonoBehaviour
{
    private readonly List<GameObject> _activeCombatants = new List<GameObject>();
    private readonly Dictionary<CombatantId, GameObject> _statusHubConnectors = new Dictionary<CombatantId, GameObject>(6);
    
    private void Start()
    {
        foreach (Transform child in transform)
        {
            var childGo = child.gameObject;
            _statusHubConnectors.Add(childGo.GetComponent<StatusHub>().id, childGo);
        }

        CombatEvents.OnCombatantAdded += CombatantActivated;
        CombatEvents.OnActivateStatusHubs += ConnectStatusHubs;
    }

    private void CombatantActivated(GameObject combatant)
    {
        _activeCombatants.Add(combatant);
    }

    private void ConnectStatusHubs()
    {
        foreach (var combatant in _activeCombatants)
        {   
            var id = combatant.GetComponent<CombatId>().id;
            var barConnectorGo = _statusHubConnectors[id];
            barConnectorGo.GetComponent<StatusHub>().Connect(combatant);
            barConnectorGo.SetActive(true);
        }   
    }

    private void OnDestroy()
    {
        CombatEvents.OnCombatantAdded -= CombatantActivated;
        CombatEvents.OnActivateStatusHubs -= ConnectStatusHubs;
    }
}
