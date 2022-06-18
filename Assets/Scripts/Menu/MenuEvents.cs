using System;
using UnityEngine;

public static class MenuEvents
{
    public static event Action<GameObject> OnCharacterOptionSelected;

    public static void CharacterOptionSelected(GameObject prefab)
    {
        OnCharacterOptionSelected?.Invoke(prefab);
    }
}
