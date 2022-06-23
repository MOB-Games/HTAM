using Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPresenter : MonoBehaviour
{
    public GameObject characterImageGo;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;

    private Image _characterImage;


    private void Start()
    {
        _characterImage = characterImageGo.GetComponent<Image>();
        TownEvents.OnCharacterSelected += PresentCharacter;
        TownEvents.OnStatChange += ChangeStat;
    }

    private void PresentCharacter(CharacterTownInfo character)
    {
        _characterImage.sprite = character.Sprite;
        _characterImage.color = Color.white;
        var characterSize = character.Sprite.bounds.size;
        characterImageGo.transform.localScale = new Vector3(characterSize.x / characterSize.y, 1,1);
        var stats = character.State.stats;
        damageText.text = stats.damage.value.ToString();
        damageText.color = Color.black;
        defenseText.text = stats.defense.value.ToString();
        defenseText.color = Color.black;
        speedText.text = stats.speed.value.ToString();
        speedText.color = Color.black;
    }
    
    private void ChangeStat(StatType stat, int change)
    {
        var text = stat switch
        {
            StatType.Damage => damageText,
            StatType.Defense => defenseText,
            StatType.Speed => speedText,
            _ => null
        };
        if (text != null)
        {
            text.text = (int.Parse(text.text) + change).ToString();
            text.color = Color.green;
        }
    }


    private void OnDestroy()
    {
        TownEvents.OnCharacterSelected -= PresentCharacter;
        TownEvents.OnStatChange -= ChangeStat;
    }
}
