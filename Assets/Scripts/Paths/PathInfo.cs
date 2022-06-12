using UnityEngine;

public class PathInfo : MonoBehaviour
{
    [HideInInspector]
    public int length;
    public Sprite combatBackground;
    public TownInfo townInfo;

    private void OnValidate()
    {
        length = GetComponent<EnemySpawner>().enemiesForStages.NumberOfStages();
    }
}
