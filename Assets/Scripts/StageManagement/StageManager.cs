using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Path
{
    public readonly PathInfo Info;
    public readonly EnemySpawner EnemySpawner;
    // public readonly TownSpawner TownSpawner;

    public Path(PathInfo info, EnemySpawner enemySpawner)
    {
        Info = info;
        EnemySpawner = enemySpawner;
        // TownSpawner = townSpawner;
    }
}

public class StageManager : MonoBehaviour
{
    public List<GameObject> pathPrefabs;
    public GameProgress gameProgress;
    private readonly List<Path> _paths = new List<Path>();

    private void Start()
    {
        foreach (var pathPrefab in pathPrefabs)
        {
            _paths.Add(new Path(pathPrefab.GetComponent<PathInfo>(), pathPrefab.GetComponent<EnemySpawner>()));
        }

        CombatEvents.OnLoadScene += LoadCombatStage;
    }

    private void OnValidate()
    {
        if (pathPrefabs.Count < gameProgress.currentPath)
            throw new IndexOutOfRangeException(
                $"Current path is {gameProgress.currentPath} but stage manager only has {pathPrefabs.Count} paths");
    }

    public void NextStage()
    {
        gameProgress.currentStage++;
        if (gameProgress.currentStage >= _paths[gameProgress.currentPath].Info.length)
        {
            gameProgress.currentPath++;
            gameProgress.currentStage = -1; // -1 will mean a town
            if (gameProgress.currentPath > gameProgress.maxPath)
            {
                gameProgress.maxPath = gameProgress.currentPath;
                gameProgress.maxStage = -1;
            }
        }

        if (gameProgress.currentPath == gameProgress.maxPath && gameProgress.currentStage > gameProgress.maxStage)
            gameProgress.maxStage = gameProgress.currentStage;
        LoadStage();
    }

    public void PreviousStage()
    {
        gameProgress.currentStage--;
        if (gameProgress.currentStage < -1)
        {
            gameProgress.currentPath--;
            gameProgress.currentStage = _paths[gameProgress.currentPath].Info.length - 1;
        }
        LoadStage();
    }

    private bool StageCleared()
    {
        return gameProgress.maxPath > gameProgress.currentPath ||
                (gameProgress.maxPath == gameProgress.currentPath && gameProgress.maxStage > gameProgress.currentStage);
    }

    private void LoadCombatStage()
    {
        var path = _paths[gameProgress.currentPath];
        Camera.main!.GetComponentInChildren<Image>().sprite = path.Info.background;
        path.EnemySpawner.Spawn(StageCleared() ? -1 : gameProgress.currentStage);
    }

    private void LoadStage()
    {
        if (gameProgress.currentStage != -1)
            SceneManager.LoadScene("BattleScene");
        else
        {
            // load town scene
        }
    }

    private void OnDestroy()
    {
        CombatEvents.OnLoadScene -= LoadCombatStage;
    }
}
