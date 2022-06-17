using TMPro;
using UnityEngine;

public class BlacksmithManager : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    
    private void Start()
    {
        TownEvents.OnPublishTownInfo += DisplayPrice;
    }

    private void DisplayPrice(TownInfo townInfo, bool _, string __)
    {
        priceText.text = $"Price: {townInfo.blacksmithInfo.price.ToString()}";
    }

    public void Open()
    {
        TownEvents.OpenBlacksmith();
    }
    
    public void Close()
    {
        TownEvents.CloseBlacksmith();
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= DisplayPrice;
    }
}
