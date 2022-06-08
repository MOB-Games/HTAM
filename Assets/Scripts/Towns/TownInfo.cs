using UnityEngine;

[System.Serializable]
public class TownInfo
{
    [Multiline]
    public string signpost;
    public Sprite townBackground;
    public Sprite innSprite;
    public Sprite blacksmithSprite;
    public Sprite shopSprite;

    public BlacksmithInfo blacksmithInfo;
}
