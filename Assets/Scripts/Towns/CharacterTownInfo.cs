using Core.DataTypes;
using Core.Stats;
using UnityEngine;

public class CharacterTownInfo
{
    public readonly string Name;
    public readonly StatBlockSO Stats;
    public readonly Sprite Sprite;
    public readonly IntegerVariable Level;
    public readonly IntegerVariable Exp;
    public readonly IntegerVariable Gold;

    public CharacterTownInfo(GameObject characterPrefab)
    {
        Name = characterPrefab.name;
        var state = characterPrefab.GetComponent<MemoryManager>().state;
        Stats = state.stats;
        Level = state.level;
        Exp = state.exp;
        Gold = state.gold;
        Sprite = characterPrefab.GetComponent<SpriteRenderer>().sprite;
    }
}
