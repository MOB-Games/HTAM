using System.Linq;
using Core.DataTypes;
using UnityEngine;

public class TownCharacterLoader : MonoBehaviour
{
    public CharacterDB characters;
    
    private void Start()
    {
         Invoke(nameof(LoadCharacters), 0.5f);
    }

    private void LoadCharacters()
    {
        TownEvents.LoadedCharacters( characters.characters
            .Where(c => c.available)
            .Select(c => new CharacterTownInfo(c.prefab))
            .ToList());
    }
}
