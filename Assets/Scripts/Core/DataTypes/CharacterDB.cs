using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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

        public GameObject playerPrefab;
        [HideInInspector]
        [CanBeNull] public GameObject partyMemberTopPrefab;
        [HideInInspector]
        [CanBeNull] public GameObject partyMemberBottomPrefab;
    }
}

