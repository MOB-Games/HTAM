using UnityEngine;

public class MoveToLocation : MonoBehaviour
{
    private CombatantEvents _combatantEvents;
    
    private void Start()
    {
        _combatantEvents = GetComponent<CombatantEvents>();
        GameEvents.OnOpenLoadingScreen += Enter;
    }

    private void Enter()
    {
        _combatantEvents.Return();
    }

    private void OnDestroy()
    {
        GameEvents.OnOpenLoadingScreen -= Enter;
    }
}
