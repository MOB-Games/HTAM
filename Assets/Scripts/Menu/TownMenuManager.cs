using UnityEngine;

public class TownMenuManager : MonoBehaviour
{
    public GameObject menuGo;
    public GameObject saveSlotSelector;
    
    public void Open()
    {
        menuGo.SetActive(true);
        TownEvents.OpenMenu();
    }
    
    public void Close()
    {
        menuGo.SetActive(false);
        TownEvents.CloseMenu();
    }

    public void OpenSaveSlotSelector()
    {
        saveSlotSelector.SetActive(true);
    }
    
    public void CloseSaveSlotSelector()
    {
        saveSlotSelector.SetActive(false);
    }
}
