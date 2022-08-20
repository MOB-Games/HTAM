using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject playerSelector;
    
    public void OpenPlayerSelector()
    {
        playerSelector.SetActive(true);
    }
    
    public void ClosePlayerSelector()
    {
        playerSelector.SetActive(false);
    }
}
