using System.Linq;
using Core.DataTypes;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject playerMark;
    public GameObject topMark;
    public GameObject bottomMark;

    public GameObject partyEditor;
    public GameObject skillTree;

    public GameObject removeFromPartyButton;
    public GameObject makeTopButton;
    public GameObject makeBottomButton;
    
    
    public CharacterDB characterDB;
    
    private CharacterTownInfo _selectedCharacterTownInfo;
    
    private void Start()
    {
        TownEvents.OnOpenBlacksmith += MarkParty;
        TownEvents.OnOpenInn += MarkParty;
        TownEvents.OnCloseBlacksmith += UnmarkParty;
        TownEvents.OnCloseInn += UnmarkParty;
        
        TownEvents.OnOpenInn += RegisterForSelectedCharacter;
        TownEvents.OnCloseInn += UnregisterForSelectedCharacter;
    }
    
    private void RegisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected += CharacterSelected;
    }

    private void CharacterSelected(CharacterTownInfo characterTownInfo)
    {
        _selectedCharacterTownInfo = characterTownInfo;
        UpdateEditorOptions();
    }
    
    private void UnregisterForSelectedCharacter()
    {
        TownEvents.OnCharacterSelected -= CharacterSelected;
    }

    private void UnmarkParty()
    {
        playerMark.SetActive(false);
        topMark.SetActive(false);
        bottomMark.SetActive(false);
    }

    private void MarkParty()
    {
        var availableCharacters = characterDB.characterGameInfos
            .Where(c => c.available)
            .Select(c => c.prefab)
            .ToList();
        var index = availableCharacters.FindIndex(c => c == characterDB.playerPrefab);
        playerMark.transform.localPosition = new Vector3(-380, 220 - 80 * index, 0);
        playerMark.SetActive(true);

        if (characterDB.partyMemberTopPrefab != null)
        {
            index = availableCharacters.FindIndex(c => c == characterDB.partyMemberTopPrefab);
            topMark.transform.localPosition = new Vector3(-380, 220 - 80 * index, 0);
            topMark.SetActive(true);
        }
        else
            topMark.SetActive(false);

        if (characterDB.partyMemberBottomPrefab != null)
        {
            index = availableCharacters.FindIndex(c => c == characterDB.partyMemberBottomPrefab);
            bottomMark.transform.localPosition = new Vector3(-380, 220 - 80 * index, 0);
            bottomMark.SetActive(true);
        }
        else
            bottomMark.SetActive(false);
    }

    private void UpdateEditorOptions()
    {
        if (_selectedCharacterTownInfo == null || characterDB.playerPrefab == _selectedCharacterTownInfo.Prefab)
        {
            removeFromPartyButton.SetActive(false);
            makeTopButton.SetActive(false);
            makeBottomButton.SetActive(false);
            return;
        }
        removeFromPartyButton.SetActive(true);
        makeTopButton.SetActive(true);
        makeBottomButton.SetActive(true);
        if (characterDB.partyMemberTopPrefab == _selectedCharacterTownInfo.Prefab)
            makeTopButton.SetActive(false);
        else if (characterDB.partyMemberBottomPrefab == _selectedCharacterTownInfo.Prefab)
            makeBottomButton.SetActive(false);
        else 
            removeFromPartyButton.SetActive(false);
    }

    public void OpenEditor()
    {
        skillTree.SetActive(false);
        UpdateEditorOptions();
        partyEditor.SetActive(true);
    }
    
    public void CloseEditor()
    {
        skillTree.SetActive(true);
        partyEditor.SetActive(false);
    }

    public void RemoveFromParty()
    {
        if (characterDB.partyMemberTopPrefab == _selectedCharacterTownInfo.Prefab)
            characterDB.partyMemberTopPrefab = null;
        if (characterDB.partyMemberBottomPrefab == _selectedCharacterTownInfo.Prefab)
            characterDB.partyMemberBottomPrefab = null;
        UpdateEditorOptions();
        MarkParty();
    }

    public void MakeTop()
    {
        characterDB.partyMemberTopPrefab = _selectedCharacterTownInfo.Prefab;
        if (characterDB.partyMemberBottomPrefab == _selectedCharacterTownInfo.Prefab)
            characterDB.partyMemberBottomPrefab = null;
        UpdateEditorOptions();
        MarkParty();
    }
    
    public void MakeBottom()
    {
        characterDB.partyMemberBottomPrefab = _selectedCharacterTownInfo.Prefab;
        if (characterDB.partyMemberTopPrefab == _selectedCharacterTownInfo.Prefab)
            characterDB.partyMemberTopPrefab = null;
        UpdateEditorOptions();
        MarkParty();
    }

    private void OnDestroy()
    {
        TownEvents.OnOpenBlacksmith -= MarkParty;
        TownEvents.OnOpenInn -= MarkParty;
        TownEvents.OnCloseBlacksmith -= UnmarkParty;
        TownEvents.OnCloseInn -= UnmarkParty;
        
        TownEvents.OnOpenInn -= RegisterForSelectedCharacter;
        TownEvents.OnCloseInn -= UnregisterForSelectedCharacter;
    }
}
