using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUIWindowManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject parentScreen;
    public GameObject characterButton;
    public GameObject toolTip;
    
    private bool _open;
    
    private void Start()
    {
        TownEvents.OnLoadedCharacters += SetUpSelectButtons;
    }
    
    private void SetUpSelectButtons(List<CharacterTownInfo> characterTownInfos)
    {
        var height = 180;
        foreach (var characterTownInfo in characterTownInfos)
        {
            var inst = Instantiate(characterButton, parentScreen.transform);
            inst.transform.localPosition = new Vector3(-380, height, 0);
            height -= 80;
            inst.GetComponent<Button>().onClick.AddListener((() => SelectCharacter(characterTownInfo)));
            inst.GetComponentInChildren<TextMeshProUGUI>().text = characterTownInfo.Name;
        }
    }
    
    private void SelectCharacter(CharacterTownInfo character)
    {
        TownEvents.CharacterSelected(character);
    }
    
    public void Open()
    {
        _open = true;
        parentScreen.SetActive(true);
    }
    
    public void Close()
    {
        _open = false;
        parentScreen.SetActive(false);
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
