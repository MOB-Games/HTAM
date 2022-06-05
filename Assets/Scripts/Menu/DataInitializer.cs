using Core.DataTypes;
using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    public PathList pathList;
    public GameProgress gameProgress;

    public void InitializeData()
    {
        gameProgress.Init();
    }
}
