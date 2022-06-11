using UnityEngine;

public class PathInfo : MonoBehaviour
{
    public Sprite combatBackground;
    public TownInfo townInfo;
    

    public int Length()
    {
        return GetComponent<EnemySpawner>().enemiesForStages.NumberOfStages();
    }
}
