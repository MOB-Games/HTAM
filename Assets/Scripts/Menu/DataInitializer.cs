using System.Collections.Generic;
using Core.DataTypes;
using Core.Stats;
using UnityEngine;



public class DataInitializer : MonoBehaviour
{
    public List<StatBlockSO> characterStatBlocks;
    public PathList pathList;
    public GameProgress gameProgress;

    public void InitializeData()
    {
        gameProgress.Init();
        foreach (var characterStatBlock in characterStatBlocks)
        {
            characterStatBlock.Init();
        }
    }
}
