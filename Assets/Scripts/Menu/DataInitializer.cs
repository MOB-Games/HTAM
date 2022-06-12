using Core.DataTypes;
using UnityEngine;



public class DataInitializer : MonoBehaviour
{
    public CharacterDB characterDB;
    public PathList pathList;
    public GameProgress gameProgress;

    public void InitializeData()
    {
        gameProgress.Init();
        GameManager.Instance.gold.value = 0;
        foreach (var characterState in characterDB.characterStates)
        {
            characterState.Init();
        }
    }
}
