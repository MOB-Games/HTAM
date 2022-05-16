using System;
using UnityEngine;

[Serializable]
public class TownInfo
{
    [Multiline]
    public string signpost;
    public Sprite townBackground;
    public Sprite innSprite;
    public Sprite blacksmithSprite;
    public Sprite shopSprite;
}

public class PathInfo : MonoBehaviour
{
    [HideInInspector]
    public int length;
    public Sprite combatBackground;
    public TownInfo townInfo;
    

    private void Start()
    {
        length = GetComponent<EnemySpawner>().enemiesForStages.NumberOfStages();
    }
}
