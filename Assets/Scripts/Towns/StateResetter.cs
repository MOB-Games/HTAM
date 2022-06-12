using Core.DataTypes;
using UnityEngine;

public class StateResetter : MonoBehaviour
{
    public CharacterDB characterDB;

    private void Start()
    {
        foreach (var characterState in characterDB.characterStates)
        {
            characterState.stats.Reset();
            characterState.conditions.Clear();
        }
    }
}
