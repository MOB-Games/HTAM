using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlacksmithManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject blacksmithScreen;
    public GameObject toolTip;
    public GameObject characterButton;
    public TextMeshProUGUI priceText;

    private bool _open = false;
    
    private void Start()
    {
        TownEvents.OnLoadedCharacters += SetUpSelectButtons;
        TownEvents.OnPublishTownInfo += DisplayPrice;
    }

    private void DisplayPrice(TownInfo townInfo, bool _, string __)
    {
        priceText.text = $"Price: {townInfo.blacksmithInfo.price.ToString()}";
    }

    private void SetUpSelectButtons(List<CharacterTownInfo> characterTownInfos)
    {
        var height = 172;
        foreach (var characterTownInfo in characterTownInfos)
        {
            var inst = Instantiate(characterButton, blacksmithScreen.transform);
            inst.transform.localPosition = new Vector3(-380, height, 0);
            height -= 75;
            inst.GetComponent<Button>().onClick.AddListener((() => SelectCharacter(characterTownInfo)));
            inst.GetComponentInChildren<TextMeshProUGUI>().text = characterTownInfo.Name;
        }
    }

    public void Open()
    {
        _open = true;
        TownEvents.OpenBlacksmith();
        blacksmithScreen.SetActive(true);
    }

    private void SelectCharacter(CharacterTownInfo character)
    {
        TownEvents.CharacterSelected(character);
    }

    public void Close()
    {
        _open = false;
        TownEvents.CloseBlacksmith();
        blacksmithScreen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!_open)
            toolTip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }

    private void OnDestroy()
    {
        TownEvents.OnLoadedCharacters -= SetUpSelectButtons;
        TownEvents.OnPublishTownInfo -= DisplayPrice;
    }
}
