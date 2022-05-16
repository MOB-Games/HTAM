using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PathList : ScriptableObject
{
    public List<GameObject> pathPrefabs;
    public List<Path> paths;

    public void Init()
    {
        paths = new List<Path>();
        foreach (var pathPrefab in pathPrefabs)
        {
            paths.Add(new Path(pathPrefab.GetComponent<PathInfo>(), 
                pathPrefab.GetComponent<EnemySpawner>()));
        }
    }
}
