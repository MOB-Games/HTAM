using Core.CharacterTypes;
using Core.Enums;

public class Player : Playable
{
    private void Awake()
    {
        Id = CharacterId.Player;
    }
}
