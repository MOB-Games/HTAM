using System.Collections.Generic;
using Core.DataTypes;
using UnityEngine;

public class StateResetter : MonoBehaviour
{
    public List<PlayableState> playableStates;

    private void Start()
    {
        foreach (var playableState in playableStates)
        {
            playableState.stats.Reset();
            playableState.conditions.Clear();
        }
    }
}
