using UnityEngine;

[CreateAssetMenu]
public class GameProgress : ScriptableObject
{
    public int currentPath;
    public int currentStage;

    public int maxPath;
    public int maxStage;

    public void Init()
    {
        maxPath = currentPath = 0;
        maxStage = currentStage = -1;
    }
}
