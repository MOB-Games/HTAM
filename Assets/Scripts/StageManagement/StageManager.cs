using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameProgress gameProgress;
    public PathList pathList;
    
    private void Start()
    {
        GameEvents.OnLoadStage += LoadStage;
        CombatEvents.OnWin += StageCleared;
        pathList.Init();
    }

    private bool AtFirstPath()
    {
        return gameProgress.currentPath == 0;
    }

    public void NextStage()
    {
        gameProgress.currentStage++;
        if (gameProgress.currentStage >= pathList.paths[gameProgress.currentPath].Info.length)
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
            gameProgress.currentStage = pathList.paths[gameProgress.currentPath].Info.length - 1;
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
        CombatEvents.SpawnParty();
        var path = pathList.paths[gameProgress.currentPath];
        Camera.main!.GetComponentInChildren<Image>().sprite = path.Info.combatBackground;
        path.EnemySpawner.Spawn(IsStageCleared() ? -1 : gameProgress.currentStage);
        Invoke(nameof(StartCombat), 1);
    }

    public void StartCombat()
    {
        CombatEvents.StartCombat();
    }

    private void LoadTownStage()
    {
        var townInfo = pathList.paths[gameProgress.currentPath].Info.townInfo;
        var prevTownSignpost = AtFirstPath() ? "" : pathList.paths[gameProgress.currentPath - 1].Info.townInfo.signpost;
        TownEvents.PublishTownInfo(townInfo, prevTownSignpost);
    }

    private void LoadStage()
    {
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
