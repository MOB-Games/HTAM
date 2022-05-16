using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    public PathList pathList;
    public GameProgress gameProgress;

    public void InitializeData()
    {
        pathList.Init();
        gameProgress.Init();
    }
}
