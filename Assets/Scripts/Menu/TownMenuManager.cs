using UnityEngine;
using UnityEngine.SceneManagement;

public class TownMenuManager : MonoBehaviour
{
    public GameObject menuGo;
    public GameObject loadSlotSelector;
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
    
    public void OpenLoadSlotSelector()
    {
        loadSlotSelector.SetActive(true);
    }
    
    public void CloseLoadSlotSelector()
    {
        loadSlotSelector.SetActive(false);
    }
    
    public void OpenSaveSlotSelector()
    {
        saveSlotSelector.SetActive(true);
    }
    
    public void CloseSaveSlotSelector()
    {
        saveSlotSelector.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
