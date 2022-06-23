using Core.DataTypes;
using UnityEngine;



public class DataInitializer : MonoBehaviour
{
    public CharacterDB characterDB;
    public PathList pathList;
    public GameProgress gameProgress;
    public GameObject playerSelector;

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

    public void InitializeData()
    {
        GameManager.Instance.gold.value = 0;
        characterDB.Init(_player);
    }

    public void OpenPlayerSelector()
    {
        playerSelector.SetActive(true);
    }
    
    public void ClosePlayerSelector()
    {
        playerSelector.SetActive(false);
    }

    private void OnDestroy()
    {
        MenuEvents.OnCharacterOptionSelected -= SavePlayer;
    }
}
