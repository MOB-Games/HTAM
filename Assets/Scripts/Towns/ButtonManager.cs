using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private readonly List<Button> _buttons = new();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            var button = child.gameObject.GetComponent<Button>();
            if (button != null)
                _buttons.Add(button);
        }

        TownEvents.OnOpenSignpost += DisableButtons;
        TownEvents.OnOpenInn += DisableButtons;
        TownEvents.OnOpenShop += DisableButtons;
        TownEvents.OnOpenBlacksmith += DisableButtons;
        
        TownEvents.OnCloseSignpost += EnableButtons;
        TownEvents.OnCloseInn += EnableButtons;
        TownEvents.OnCloseShop += EnableButtons;
        TownEvents.OnCloseBlacksmith += EnableButtons;
    }

    private void DisableButtons()
    {
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
    }
    
    private void EnableButtons()
    {
        foreach (var button in _buttons)
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
        
        TownEvents.OnCloseSignpost -= EnableButtons;
        TownEvents.OnCloseInn -= EnableButtons;
        TownEvents.OnCloseShop -= EnableButtons;
        TownEvents.OnCloseBlacksmith -= EnableButtons;
    }
}
