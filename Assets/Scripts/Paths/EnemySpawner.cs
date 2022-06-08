using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemiesForStage
{
    public List<int> enemyPrefabIndices;
}

[Serializable]
public class EnemiesForStages
{
    public List<EnemiesForStage> stages;

    public int NumberOfStages()
    {
        return stages.Count;
    }

    public int NumberEnemiesForStage(int stage)
    {
        if (stage >= NumberOfStages())
            throw new IndexOutOfRangeException(
                $"Tried to get enemies for stage {stage}, but there were only {NumberOfStages()} stages");
        return stages[stage].enemyPrefabIndices.Count;
    }

    public int EnemyPrefabIndex(int stage, int index)
    {
        if (index >= NumberEnemiesForStage(stage))
            throw new IndexOutOfRangeException(
                $"Tried to get enemy of index {index} for stage {stage}, but stage {stage} only has {NumberEnemiesForStage(stage)} enemies");
        return stages[stage].enemyPrefabIndices[index];
    }
}

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;

    public EnemiesForStages enemiesForStages;

    // probabilities for random encounters
    [Range(0,100)]
    public List<int> enemyProbabilities;

    [Range(0,100)]
    public int probabilityFor1Enemy;
    [Range(0,100)]
    public int probabilityFor2Enemy;

    private readonly List<CombatantId> _enemyIds = new List<CombatantId>()
        { CombatantId.EnemyCenter, CombatantId.EnemyTop, CombatantId.EnemyBottom };

    private void OnValidate()
    {
        if (enemyPrefabs.Count == 0)
            throw new ConstraintException("Enemy spawner has no enemy prefabs");
        if(enemyProbabilities.Count != enemyPrefabs.Count - 1)
            throw new ConstraintException("Number of probabilities for enemies must be number of enemies minus 1");
        if(enemyProbabilities.Sum() > 100 || probabilityFor1Enemy + probabilityFor2Enemy > 100)
            throw new ConstraintException("Sum of probabilities exceeds 100");
        for (var i = 0; i < enemiesForStages.NumberOfStages(); i++)
            if (enemiesForStages.NumberEnemiesForStage(i) > 3)
                throw new ConstraintException($"Stage {i} has more than 3 enemies, this is not allowed");
    }
    
    private int GetNumberOfEnemiesToCreate()
    {
        var random = Random.Range(0, 100);
        if (random < probabilityFor1Enemy)
            return 1;
        return random < probabilityFor1Enemy + probabilityFor2Enemy ? 2 : 3;
    }
    
    private GameObject GetEnemyPrefabToCreate()
    {
        var random = Random.Range(0, 100);
        var chance = 0;
        for (var i = 0; i < enemyPrefabs.Count - 1; i++)
        {
            chance += enemyProbabilities[i];
            if (chance < random)
                return enemyPrefabs[i];
        }
        return enemyPrefabs.Last();
    }

    public void Spawn(int stage)
    {
        var randomEncounter = stage < 0;
        var numEnemies = randomEncounter ? GetNumberOfEnemiesToCreate() : enemiesForStages.NumberEnemiesForStage(stage);
        for (var i = 0; i < numEnemies; i++)
        {
            var id = _enemyIds[i];
            var enemyPrefab = randomEncounter
                ? GetEnemyPrefabToCreate()
                : enemyPrefabs[enemiesForStages.EnemyPrefabIndex(stage, i)];
            var inst = Instantiate(enemyPrefab, CombatantInfo.GetLocation(id), Quaternion.identity);
            inst.GetComponent<CombatId>().id = id;
            if (!CombatantInfo.Mirror)
                inst.transform.localScale = Vector3.Scale(inst.transform.localScale, new Vector3(-1, 1, 1));
            CombatEvents.CombatantAdded(inst);
        }
    }
}
