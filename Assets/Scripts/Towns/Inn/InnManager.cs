using UnityEngine;

public class InnManager : MonoBehaviour
{
    private void Start()
    {
        
    }
    
    public void Open()
    {
        TownEvents.OpenInn();
    }
    
    public void Close()
    {
        TownEvents.CloseInn();
    }

    private void OnDestroy()
    {
        
    }
}
