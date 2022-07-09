using System;
using UnityEngine;
using UnityEngine.UI;

public class TownPresenter : MonoBehaviour
{
    public GameObject innIcon;
    public GameObject shopIcon;
    public GameObject blacksmithIcon;
    public GameObject previousStageButton;

    private bool _active;
    private float _lastWidth;
    private Image _innImage;
    private Image _shopImage;
    private Image _blacksmithImage;

    private void Start()
    {
        TownEvents.OnPublishTownInfo += DisplayTown;
    }

    private void DisplayTown(TownInfo info, bool _, string signpostLeft)
    {
        Camera.main!.GetComponentInChildren<Image>().sprite = info.townBackground;
        _innImage = innIcon.GetComponent<Image>();
        _innImage.sprite = info.innSprite;
        _shopImage = shopIcon.GetComponent<Image>();
        _shopImage.sprite = info.shopSprite;
        _blacksmithImage = blacksmithIcon.GetComponent<Image>();
        _blacksmithImage.sprite = info.blacksmithSprite;
        if (string.IsNullOrEmpty(signpostLeft))
            previousStageButton.SetActive(false);
        _active = true;
    }

    private void Update()
    {
        if (!_active || Math.Abs(_lastWidth - Screen.width) < 1) return;
        _lastWidth = Screen.width;
        var screen = new Vector2(Screen.width, Screen.height);
        var size = Vector2.Scale(screen, new Vector2(0.29f, 0.53f));
        _innImage.rectTransform.sizeDelta = size;
        _shopImage.rectTransform.sizeDelta = size;
        _blacksmithImage.rectTransform.sizeDelta = size;
        var screen3d = new Vector3(screen.x, screen.y);
        innIcon.transform.localPosition = Vector3.Scale(screen3d, new Vector3(0.225f, 0.205f, 1));
        shopIcon.transform.localPosition = Vector3.Scale(screen3d, new Vector3(-0.025f, -0.205f, 1));
        blacksmithIcon.transform.localPosition = Vector3.Scale(screen3d, new Vector3(-0.25f, 0.205f, 1));
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= DisplayTown;
    }
}
