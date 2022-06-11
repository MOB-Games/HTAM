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
        CombatEvents.OnWin += StageCleared;
        var currentPathGo = pathList.pathPrefabs[gameProgress.currentPath];
        _currentPath = new Path(currentPathGo.GetComponent<PathInfo>(), currentPathGo.GetComponent<EnemySpawner>());
        Debug.Log(_currentPath.Info.Length());
    }

    private void RecordPreviousGameProgress()
    {
        gameProgress.previousPath = gameProgress.currentPath;
        gameProgress.previousStage = gameProgress.currentStage;
    }

    public void NextStage()
    {
        RecordPreviousGameProgress();
        gameProgress.currentStage++;
        if (gameProgress.currentStage >= _currentPath.Info.Length())
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

    public void PreviousStage()
    {
        RecordPreviousGameProgress();
        gameProgress.currentStage--;
        if (gameProgress.currentStage < -1)
        {
            gameProgress.currentPath--;
            gameProgress.currentStage = _currentPath.Info.Length() - 1;
        }
        LoadScene();
    }

    private void StageCleared()
    {
        if (gameProgress.currentPath == gameProgress.maxPath && gameProgress.currentStage > gameProgress.maxStage)
            gameProgress.maxStage = gameProgress.currentStage;
    }

    private bool IsStageCleared()
    {
        return gameProgress.maxPath > gameProgress.currentPath ||
                (gameProgress.maxPath == gameProgress.currentPath && gameProgress.maxStage >= gameProgress.currentStage);
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
        _currentPath.EnemySpawner.Spawn(IsStageCleared() ? -1 : gameProgress.currentStage);
        Invoke(nameof(ActivateStatusHubs), 2);
        Invoke(nameof(StartCombat), 3);
    }

    private bool AtFirstPath()
    {
        return gameProgress.currentPath == 0;
    }

    private void LoadTownStage()
    {
        var prevTownSignpost = AtFirstPath() ? 
            "" : 
            pathList.pathPrefabs[gameProgress.currentPath - 1].GetComponent<PathInfo>().townInfo.signpost;
        TownEvents.PublishTownInfo(_currentPath.Info.townInfo, prevTownSignpost);
    }

    private void LoadStage()
    {
        CombatantInfo.Mirror = gameProgress.previousPath > gameProgress.currentPath ||
                               gameProgress.previousPath == gameProgress.currentPath &&
                               gameProgress.previousStage > gameProgress.currentStage;
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
        CombatEvents.OnWin -= StageCleared;
    }
}
