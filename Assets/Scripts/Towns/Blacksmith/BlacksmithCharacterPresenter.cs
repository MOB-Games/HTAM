using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithCharacterPresenter : MonoBehaviour
{
    public GameObject characterImageGo;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI energyEfficiencyText;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI speedText;

    private Image _characterImage;


    private void Start()
    {
        _characterImage = characterImageGo.GetComponent<Image>();
        TownEvents.OnCharacterSelected += PresentCharacter;
    }

    private void RegisterToCharacterSelect()
    {
        TownEvents.OnCharacterSelected += PresentCharacter;
    }

    private void UnregisterToCharacterSelect()
    {
        TownEvents.OnCharacterSelected -= PresentCharacter;
    }

    private void PresentCharacter(CharacterTownInfo character)
    {
        _characterImage.sprite = character.Sprite;
        _characterImage.color = Color.white;
        var characterSize = character.Sprite.bounds.size;
        characterImageGo.transform.localScale = new Vector3(characterSize.x / characterSize.y, 1,1);
        damageText.text = character.Stats.damage.value.ToString();
        energyEfficiencyText.text = character.Stats.energyEfficiency.value.ToString();
        defenceText.text = character.Stats.defence.value.ToString();
        speedText.text = character.Stats.speed.value.ToString();
    }


    private void OnDestroy()
    {
        TownEvents.OnCharacterSelected -= PresentCharacter;
    }
}
