using System;
using Core.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownUIManager : MonoBehaviour
{
    private static GameObject _inn;
    private static GameObject _shop;
    private static GameObject _blacksmith;
    private static GameObject _signpostRight;
    private static GameObject _signpostLeft;
    private static GameObject _signpostBackground;
    private static GameObject _previousStageArrow;
    private static string _signpostRightText;
    private static string _signpostLeftText;

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            var go = child.gameObject;
            switch (go.name)
            {
                case "Inn":
                    _inn = go;
                    break;
                case "Shop":
                    _shop = go;
                    break;
                case "Blacksmith":
                    _blacksmith = go;
                    break;
                case "SignpostRight":
                    _signpostRight = go;
                    break;
                case "SignpostLeft":
                    _signpostLeft = go;
                    break;
                case "SignpostBackground":
                    _signpostBackground = go;
                    break;
                case "PreviousStageButton":
                    _previousStageArrow = go;
                    break;
                default:
                    continue;
            }
        }
    }

    private static void DisableButtons()
    {
        _inn.GetComponent<Button>().interactable = false;
        _shop.GetComponent<Button>().interactable = false;
        _blacksmith.GetComponent<Button>().interactable = false;
        _signpostRight.GetComponent<Button>().interactable = false;
        _signpostLeft.GetComponent<Button>().interactable = false;
    }
    
    private static void EnableButtons()
    {
        _inn.GetComponent<Button>().interactable = true;
        _shop.GetComponent<Button>().interactable = true;
        _blacksmith.GetComponent<Button>().interactable = true;
        _signpostRight.GetComponent<Button>().interactable = true;
        _signpostLeft.GetComponent<Button>().interactable = true;
    }

    private static void DisplaySignpost(string text)
    {
        DisableButtons();
        _signpostBackground.GetComponentInChildren<TextMeshProUGUI>().text = text;
        _signpostBackground.SetActive(true);
    }

    public static void CloseSignpost()
    {
        EnableButtons();
        _signpostBackground.SetActive(false);
    }

    public static void DisplayTown(TownInfo info, string signpostLeft)
    {
        Camera.main!.GetComponentInChildren<Image>().sprite = info.townBackground;
        _inn.GetComponent<Image>().sprite = info.innSprite;
        _shop.GetComponent<Image>().sprite = info.shopSprite;
        _blacksmith.GetComponent<Image>().sprite = info.blacksmithSprite;
        _signpostRightText = info.signpost;
        _signpostRight.GetComponent<Button>().onClick.AddListener((() => DisplaySignpost(_signpostRightText)));
        if (string.IsNullOrEmpty(signpostLeft))
        {
            _signpostLeft.SetActive(false);
            _previousStageArrow.SetActive(false);
        }
        else
        {
            _signpostLeftText = signpostLeft;
            _signpostLeft.GetComponent<Button>().onClick.AddListener((() => DisplaySignpost(_signpostLeftText)));
        }
    }
}
