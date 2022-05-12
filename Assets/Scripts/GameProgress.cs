using UnityEngine;

[CreateAssetMenu]
public class GameProgress : ScriptableObject
{
    public int currentPath;
    public int currentStage;

    public int maxPath;
    public int maxStage;
}
