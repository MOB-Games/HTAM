using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    private CombatantEvents _combatantEvents;
    private void Start()
    {
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnDied += Disable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CombatEvents.Click(eventData);
    }

    private void Disable()
    {
        enabled = false;
    }

    private void OnDestroy()
    {
        _combatantEvents.OnDied -= Disable;
    }
}
