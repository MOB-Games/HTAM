using Core.DataTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameProgress gameProgress;
    public PathList pathList;


    private Path _currentPath;
    private void Start()
    {
        GameEvents.OnLoadStage += LoadStage;
        var currentPathGo = pathList.pathPrefabs[gameProgress.currentPath];
        _currentPath = new Path(currentPathGo.GetComponent<PathInfo>(), currentPathGo.GetComponent<EnemySpawner>());
    }

    public void NextStage()
    {
        if (gameProgress.currentPath < gameProgress.lastTown) // return to last town
        {
            gameProgress.currentPath++;
            gameProgress.currentStage = -1;
        }
        else
        {
            gameProgress.currentStage++;
            if (gameProgress.currentStage >= _currentPath.Info.Length())
            {
                if (gameProgress.currentPath > gameProgress.maxClearedPath)
                    gameProgress.maxClearedPath++;
                gameProgress.currentPath++;
                gameProgress.currentStage = -1; // -1 will mean a town
            }
        }

        LoadScene();
    }

    public void PreviousStage()
    {
        if (gameProgress.currentPath == gameProgress.lastTown) // return to last town
        {
            gameProgress.currentStage = -1;
            gameProgress.lastTown++; // so that mirror will be true
        }
        else
        {
            gameProgress.currentStage--;
            if (gameProgress.currentStage < -1)
            {
                gameProgress.currentPath--;
                gameProgress.currentStage = _currentPath.Info.Length() - 1;
            }
        }
        LoadScene();
    }

    private bool IsPathCleared()
    {
        return gameProgress.maxClearedPath > gameProgress.currentPath;
    }

    public void ActivateStatusHubs()
    {
        CombatEvents.ActivateStatusHubs();
    }

    public void StartCombat()
    {
        CombatEvents.StartCombat();
    }

    private void LoadCombatStage()
    {
        CombatEvents.SpawnParty();
        Camera.main!.GetComponentInChildren<Image>().sprite = _currentPath.Info.combatBackground;
        _currentPath.EnemySpawner.Spawn(IsPathCleared() ? -1 : gameProgress.currentStage);
        Invoke(nameof(ActivateStatusHubs), 2);
        Invoke(nameof(StartCombat), 3);
    }

    private bool AtFirstPath()
    {
        return gameProgress.currentPath == 0;
    }

    private void LoadTownStage()
    {
        gameProgress.lastTown = gameProgress.currentPath;
        var prevTownSignpost = AtFirstPath() ? 
            "" : 
            pathList.pathPrefabs[gameProgress.currentPath - 1].GetComponent<PathInfo>().townInfo.signpostCleared;
        TownEvents.PublishTownInfo(_currentPath.Info.townInfo, IsPathCleared(), prevTownSignpost);
    }

    private void LoadStage()
    {
        CombatantInfo.Mirror = gameProgress.currentPath < gameProgress.lastTown;
        if (gameProgress.currentStage == -1)
            LoadTownStage();
        else
            LoadCombatStage();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(gameProgress.currentStage != -1 ? "BattleScene" : "TownScene");
    }

    private void OnDestroy()
    {
        GameEvents.OnLoadStage -= LoadStage;
    }
}
