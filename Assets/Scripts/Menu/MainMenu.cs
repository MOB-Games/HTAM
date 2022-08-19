using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject playerSelector;
    public GameObject loadSlotSelector;
    
    public void OpenPlayerSelector()
    {
        playerSelector.SetActive(true);
    }
    
    public void ClosePlayerSelector()
    {
        playerSelector.SetActive(false);
    }
    
    public void OpenLoadSlotSelector()
    {
        loadSlotSelector.SetActive(true);
    }
    
    public void CloseLoadSlotSelector()
    {
        loadSlotSelector.SetActive(false);
    }
}
