using System;
using UnityEngine;

public class PathInfo : MonoBehaviour
{
    [HideInInspector]
    public int length;
    public Sprite background;

    private void Start()
    {
        length = GetComponent<EnemySpawner>().enemiesForStages.NumberOfStages();
    }
}
