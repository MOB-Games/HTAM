using System.Collections.Generic;
using Core.Enums;
using UnityEngine;

public class BarsManager : MonoBehaviour
{
    private readonly List<GameObject> _activeCombatants = new List<GameObject>();
    private readonly Dictionary<CombatantId, GameObject> _barsConnectors = new Dictionary<CombatantId, GameObject>(6);
    
    private void Start()
    {
        foreach (Transform child in transform)
        {
            var childGo = child.gameObject;
            _barsConnectors.Add(childGo.GetComponent<BarsConnector>().id, childGo);
        }

        CombatEvents.OnCombatantAdded += CombatantActivated;
        CombatEvents.OnStartCombat += ActivateBars;
    }

    private void CombatantActivated(GameObject combatant)
    {
        _activeCombatants.Add(combatant);
    }

    private void ConnectBars(GameObject combatant)
    {
        var id = combatant.GetComponent<ID>().id;
        var barConnectorGo = _barsConnectors[id];
        barConnectorGo.GetComponent<BarsConnector>().Connect(combatant);
        barConnectorGo.SetActive(true);
    }

    private void ActivateBars()
    {
        foreach (var combatant in _activeCombatants)
        {   
            ConnectBars(combatant);
        }   
    }

    private void OnDestroy()
    {
        CombatEvents.OnCombatantAdded -= ConnectBars;
        CombatEvents.OnStartCombat -= ActivateBars;
    }
}
