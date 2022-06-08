using System.Collections.Generic;
using Core.DataTypes;
using UnityEngine;

public class StateResetter : MonoBehaviour
{
    public List<CharacterState> characterStates;

    private void Start()
    {
        foreach (var characterState in characterStates)
        {
            characterState.stats.Reset();
            characterState.conditions.Clear();
        }
    }
}
