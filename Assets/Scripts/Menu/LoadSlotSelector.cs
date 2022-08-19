using Core.DataTypes;
using UnityEngine;

public class LoadSlotSelector : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    
    public SaveSlots saveSlots;
    
    private void OnEnable()
    {
        slot1.SetActive(saveSlots.saveSlots[0].full);    
        slot2.SetActive(saveSlots.saveSlots[1].full);    
        slot3.SetActive(saveSlots.saveSlots[2].full);   
    }
}
