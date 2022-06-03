using UnityEngine;

[CreateAssetMenu]
public class GameProgress : ScriptableObject
{
    public int currentPath;
    public int currentStage;
    public int previousPath;
    public int previousStage;

    public int maxPath;
    public int maxStage;

    public void Init()
    {
        maxPath = currentPath = previousPath = 0;
        maxStage = currentStage = previousStage =-1;
    }
}
