using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.DataTypes
{
    [Serializable]
    public class CharacterGameInfo
    {
        public GameObject prefab;
        public bool available;
    }
    
    [CreateAssetMenu]
    public class CharacterDB : ScriptableObject
    {
        public List<CharacterGameInfo> characterGameInfos;
        public List<CharacterState> characterStates;
        
        [HideInInspector]
        public GameObject playerPrefab;
        [HideInInspector]
        [CanBeNull] public GameObject partyMemberTopPrefab;
        [HideInInspector]
        [CanBeNull] public GameObject partyMemberBottomPrefab;

        public void Init(GameObject player)
        {
            foreach (var characterState in characterStates)
                characterState.Init();
            

            foreach (var characterGameInfo in characterGameInfos) 
                characterGameInfo.available = false;

            var playerGameInfo = characterGameInfos.Find(c => c.prefab == player);
            characterGameInfos.Remove(playerGameInfo);
            characterGameInfos = characterGameInfos.OrderBy(c => Random.Range(0, 100)).ToList();
            characterGameInfos.Insert(0, playerGameInfo);
            playerGameInfo.available = true;
            playerPrefab = player;
            partyMemberTopPrefab = null;
            partyMemberBottomPrefab = null;
        }
    }
}

