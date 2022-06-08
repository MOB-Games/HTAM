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

    private bool _open = false;
    
    private void Start()
    {
        TownEvents.OnLoadedCharacters += SetUpSelectButtons;
    }

    private void SetUpSelectButtons(List<CharacterTownInfo> characterTownInfos)
    {
        var height = 150;
        foreach (var characterTownInfo in characterTownInfos)
        {
            var inst = Instantiate(characterButton, blacksmithScreen.transform);
            inst.transform.localPosition = new Vector3(430, height, 0);
            height -= 60;
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
    }
}
