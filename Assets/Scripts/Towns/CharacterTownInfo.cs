using Core.DataTypes;
using UnityEngine;

public class CharacterTownInfo
{
    public readonly string Name;
    public readonly CharacterState State;
    public readonly Sprite Sprite;

    public CharacterTownInfo(GameObject characterPrefab)
    {
        Name = characterPrefab.name;
        State = characterPrefab.GetComponent<MemoryManager>().state;
        Sprite = characterPrefab.GetComponent<SpriteRenderer>().sprite;
    }
}
