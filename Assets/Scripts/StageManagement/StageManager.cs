using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Path
{
    public readonly PathInfo Info;
    public readonly EnemySpawner EnemySpawner;

    public Path(PathInfo info, EnemySpawner enemySpawner)
    {
        Info = info;
        EnemySpawner = enemySpawner;
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
            _paths.Add(new Path(pathPrefab.GetComponent<PathInfo>(), 
                pathPrefab.GetComponent<EnemySpawner>()));
        }

        CombatEvents.OnLoadScene += LoadCombatStage;
        CombatEvents.OnWin += StageCleared;
        if (gameProgress.currentStage == -1)
            LoadTownStage();
    }

    private void OnValidate()
    {
        if (pathPrefabs.Count < gameProgress.currentPath)
            throw new IndexOutOfRangeException(
                $"Current path is {gameProgress.currentPath} but stage manager only has {pathPrefabs.Count} paths");
    }

    private bool AtFirstPath()
    {
        return gameProgress.currentPath == 0;
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

        LoadScene();
    }

    private void StageCleared()
    {
        if (gameProgress.currentPath == gameProgress.maxPath && gameProgress.currentStage > gameProgress.maxStage)
            gameProgress.maxStage = gameProgress.currentStage;
    }

    public void PreviousStage()
    {
        gameProgress.currentStage--;
        if (gameProgress.currentStage < -1)
        {
            gameProgress.currentPath--;
            gameProgress.currentStage = _paths[gameProgress.currentPath].Info.length - 1;
        }
        LoadScene();
    }

    private bool IsStageCleared()
    {
        return gameProgress.maxPath > gameProgress.currentPath ||
                (gameProgress.maxPath == gameProgress.currentPath && gameProgress.maxStage >= gameProgress.currentStage);
    }

    private void LoadCombatStage()
    {
        var path = _paths[gameProgress.currentPath];
        Camera.main!.GetComponentInChildren<Image>().sprite = path.Info.combatBackground;
        path.EnemySpawner.Spawn(IsStageCleared() ? -1 : gameProgress.currentStage);
    }

    private void LoadTownStage()
    {
        var townInfo = _paths[gameProgress.currentPath].Info.townInfo;
        var prevTownSignpost = AtFirstPath() ? "" : _paths[gameProgress.currentPath - 1].Info.townInfo.signpost;
        TownUIManager.DisplayTown(townInfo, prevTownSignpost);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(gameProgress.currentStage != -1 ? "BattleScene" : "TownScene");
    }

    private void OnDestroy()
    {
        CombatEvents.OnLoadScene -= LoadCombatStage;
    }
}
