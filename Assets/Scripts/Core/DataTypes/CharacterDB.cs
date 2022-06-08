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
        public List<CharacterGameInfo> characters;

        public GameObject playerPrefab;
        [CanBeNull] public GameObject partyMemberTopPrefab;
        [CanBeNull] public GameObject partyMemberBottomPrefab;
    }
}

