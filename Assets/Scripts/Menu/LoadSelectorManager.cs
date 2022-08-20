using UnityEngine;

public class LoadSelectorManager : MonoBehaviour
{
    public GameObject loadSlotSelector;

    public void OpenLoadSlotSelector()
    {
        loadSlotSelector.SetActive(true);
    }
    
    public void CloseLoadSlotSelector()
    {
        loadSlotSelector.SetActive(false);
    }
}
