using UnityEngine;

[System.Serializable]
public class TownInfo
{
    [Multiline]
    public string signpost;
    [Multiline]
    public string signpostCleared;
    public Sprite townBackground;
    public Sprite innSprite;
    public Sprite blacksmithSprite;
    public Sprite shopSprite;

    public BlacksmithInfo blacksmithInfo;
}
