using System;
using Core.DataTypes;
using UnityEngine;



public class DataInitializer : MonoBehaviour
{
    public CharacterDB characterDB;
    public GameProgress gameProgress;
    public SaveSlots saveSlots;
    private GameObject _player;

    private void Start()
    {
        gameProgress.Init();
        MenuEvents.OnCharacterOptionSelected += SavePlayer;
    }

    private void SavePlayer(GameObject player)
    {
        _player = player;
    }

    public void InitializeData(int saveSlotIndex)
    {
        switch (saveSlotIndex)
        {
            case -1:
                gameProgress.Init();
                GameManager.Instance.gold.value = 0;
                characterDB.Init(_player);
                break;
            case 0 or 1 or 2:
            {
                var savedSlot = saveSlots.saveSlots[saveSlotIndex];
                if (!savedSlot.full)
                {
                    throw new DataMisalignedException("There is no saves state in this save slot");
                }
                gameProgress.Init(savedSlot);
                GameManager.Instance.gold.value = savedSlot.gold;
                characterDB.Init(savedSlot);
                break;
            }
            default:
                throw new IndexOutOfRangeException(
                    $"{saveSlotIndex} is not a valid save slot index. Only -1, 0, 1, 2 are allowed");
        }
    }

    private void OnDestroy()
    {
        MenuEvents.OnCharacterOptionSelected -= SavePlayer;
    }
}
