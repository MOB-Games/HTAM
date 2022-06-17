using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPresenter : MonoBehaviour
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

    private void PresentCharacter(CharacterTownInfo character)
    {
        _characterImage.sprite = character.Sprite;
        _characterImage.color = Color.white;
        var characterSize = character.Sprite.bounds.size;
        characterImageGo.transform.localScale = new Vector3(characterSize.x / characterSize.y, 1,1);
        var stats = character.State.stats;
        damageText.text = stats.damage.value.ToString();
        energyEfficiencyText.text = stats.energyEfficiency.value.ToString();
        defenceText.text = stats.defence.value.ToString();
        speedText.text = stats.speed.value.ToString();
    }


    private void OnDestroy()
    {
        TownEvents.OnCharacterSelected -= PresentCharacter;
    }
}
