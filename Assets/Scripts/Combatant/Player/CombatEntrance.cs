using UnityEngine;

public class CombatEntrance : MonoBehaviour
{
    private CombatantEvents _combatantEvents;
    
    private void Start()
    {
        _combatantEvents = GetComponent<CombatantEvents>();
        CombatEvents.OnPartyEnter += Enter;
    }

    private void Enter()
    {
        _combatantEvents.Return();
    }

    private void OnDestroy()
    {
        CombatEvents.OnPartyEnter -= Enter;
    }
}
