using System;
using System.Linq;
using Core.DataTypes;
using UnityEngine;

public class GameSaverAndLoader : MonoBehaviour
{
    public CharacterDB characterDB;
    public GameProgress gameProgress;
    public SaveSlots saveSlots;
    public GameObject ruSure;

    private SaveSlot _selectedSaveSlot;

    public void SaveGame(int saveSlotIndex)
    {
        if (saveSlotIndex is 0 or 1 or 2)
        {
            _selectedSaveSlot = saveSlots.saveSlots[saveSlotIndex];
            if (_selectedSaveSlot.full)
                ruSure.SetActive(true);
            else 
                Save();
        }
        else
            throw new IndexOutOfRangeException(
                $"{saveSlotIndex} is not a valid save slot index. Only -1, 0, 1, 2 are allowed");
    }

    public void Sure()
    {
        Save();
        ruSure.SetActive(false);
    }

    public void NotSure()
    {
        ruSure.SetActive(false);
    }

    private void Save()
    {
        _selectedSaveSlot.full = true;
        _selectedSaveSlot.gold = GameManager.Instance.gold.value;
        _selectedSaveSlot.currentPath = gameProgress.currentPath;
        _selectedSaveSlot.maxClearedPath = gameProgress.maxClearedPath;
        _selectedSaveSlot.characterGameInfos = characterDB.characterGameInfos
            .Select(cgi => new CharacterGameInfo(cgi.prefab, cgi.available)).ToList();
        _selectedSaveSlot.characterStates = 
            characterDB.characterStates.Select(cs => new CharacterSaveState(cs)).ToList();
        _selectedSaveSlot.playerPrefab = characterDB.playerPrefab;
        _selectedSaveSlot.partyMemberBottomPrefab = characterDB.partyMemberBottomPrefab;
        _selectedSaveSlot.partyMemberTopPrefab = characterDB.partyMemberTopPrefab;
    }
    
    public void LoadGame(int saveSlotIndex)
    {
        if (saveSlotIndex is 0 or 1 or 2)
        {
            var savedSlot = saveSlots.saveSlots[saveSlotIndex];
            if (!savedSlot.full)
            {
                throw new DataMisalignedException("There is no saves state in this save slot");
            }
            gameProgress.Init(savedSlot);
            GameManager.Instance.gold.value = savedSlot.gold;
            characterDB.Init(savedSlot);
        }
        else
            throw new IndexOutOfRangeException(
                $"{saveSlotIndex} is not a valid save slot index. Only -1, 0, 1, 2 are allowed");
    }
}
