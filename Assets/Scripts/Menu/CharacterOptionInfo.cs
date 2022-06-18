using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterOptionInfo : MonoBehaviour
{
    public GameObject characterPrefab;
    [Multiline]
    public string description;

    public TextMeshProUGUI textBox;
    public GameObject characterImageGo;

    private Sprite _characterSprite;
    private Image _characterImage;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowDescription);
        _characterSprite = characterPrefab.GetComponent<SpriteRenderer>().sprite;
        _characterImage = characterImageGo.GetComponent<Image>();
    }

    private void ShowDescription()
    {
        _characterImage.sprite = _characterSprite;
        var characterSize = _characterSprite.bounds.size;
        characterImageGo.transform.localScale = new Vector3(characterSize.x / characterSize.y, 1,1);
        _characterImage.color = Color.white;
        textBox.text = description;
        MenuEvents.CharacterOptionSelected(characterPrefab);
    }
}
