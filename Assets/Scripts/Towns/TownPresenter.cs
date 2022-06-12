using UnityEngine;
using UnityEngine.UI;

public class TownPresenter : MonoBehaviour
{
    public GameObject inn;
    public GameObject shop;
    public GameObject blacksmith;
    public GameObject previousStageButton;

    private void Start()
    {
        TownEvents.OnPublishTownInfo += DisplayTown;
    }

    private void DisplayTown(TownInfo info, bool _, string signpostLeft)
    {
        Camera.main!.GetComponentInChildren<Image>().sprite = info.townBackground;
        inn.GetComponent<Image>().sprite = info.innSprite;
        shop.GetComponent<Image>().sprite = info.shopSprite;
        blacksmith.GetComponent<Image>().sprite = info.blacksmithSprite;
        if (string.IsNullOrEmpty(signpostLeft))
            previousStageButton.SetActive(false);
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= DisplayTown;
    }
}
