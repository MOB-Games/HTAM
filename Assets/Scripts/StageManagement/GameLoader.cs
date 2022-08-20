using System;
using Core.DataTypes;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public CharacterDB characterDB;
    public GameProgress gameProgress;
    public SaveSlots saveSlots;
    public GameObject ruSure;

    private SaveSlot _selectedSaveSlot;

    public void LoadGame(int saveSlotIndex)
    {
        if (saveSlotIndex is 0 or 1 or 2)
        {
            _selectedSaveSlot = saveSlots.saveSlots[saveSlotIndex];
            if (!_selectedSaveSlot.full)
            {
                throw new DataMisalignedException("There is no saves state in this save slot");
            }
            ruSure.SetActive(true);
        }
        else
            throw new IndexOutOfRangeException(
                $"{saveSlotIndex} is not a valid save slot index. Only -1, 0, 1, 2 are allowed");
    }
    
    public void Sure()
    {
        Load();
        ruSure.SetActive(false);
    }

    public void NotSure()
    {
        ruSure.SetActive(false);
    }

    private void Load()
    {
        gameProgress.Init(_selectedSaveSlot);
        GameManager.Instance.gold.value = _selectedSaveSlot.gold;
        characterDB.Init(_selectedSaveSlot);
    }
}
