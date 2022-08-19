using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public List<Button> buttons = new();

    private void Start()
    {
        TownEvents.OnOpenSignpost += DisableButtons;
        TownEvents.OnOpenInn += DisableButtons;
        TownEvents.OnOpenShop += DisableButtons;
        TownEvents.OnOpenBlacksmith += DisableButtons;
        TownEvents.OnOpenMenu += DisableButtons;
        
        TownEvents.OnCloseSignpost += EnableButtons;
        TownEvents.OnCloseInn += EnableButtons;
        TownEvents.OnCloseShop += EnableButtons;
        TownEvents.OnCloseBlacksmith += EnableButtons;
        TownEvents.OnCloseMenu += EnableButtons;
    }

    private void DisableButtons()
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }
    
    private void EnableButtons()
    {
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    private void OnDestroy()
    {
        TownEvents.OnOpenSignpost -= DisableButtons;
        TownEvents.OnOpenInn -= DisableButtons;
        TownEvents.OnOpenShop -= DisableButtons;
        TownEvents.OnOpenBlacksmith -= DisableButtons;
        TownEvents.OnOpenMenu -= DisableButtons;
        
        TownEvents.OnCloseSignpost -= EnableButtons;
        TownEvents.OnCloseInn -= EnableButtons;
        TownEvents.OnCloseShop -= EnableButtons;
        TownEvents.OnCloseBlacksmith -= EnableButtons;
        TownEvents.OnCloseMenu -= EnableButtons;
    }
}
