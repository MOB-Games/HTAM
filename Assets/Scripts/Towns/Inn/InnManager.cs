using UnityEngine;

public class InnManager : MonoBehaviour
{
    public void Open()
    {
        TownEvents.OpenInn();
    }
    
    public void Close()
    {
        TownEvents.CloseInn();
    }
}
