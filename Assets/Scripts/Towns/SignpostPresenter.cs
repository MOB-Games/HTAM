using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignpostPresenter : MonoBehaviour
{
    public GameObject signpostRight;
    public GameObject signpostLeft;
    public GameObject signpostBackground;
    private string _signpostRightText;
    private string _signpostLeftText;

    private void Start()
    {
        TownEvents.OnPublishTownInfo += Setup;
    }

    private void Setup(TownInfo townInfo, string previousPathSignpost)
    {
        if (string.IsNullOrEmpty(previousPathSignpost))
            signpostLeft.SetActive(false);
        else
        {
            _signpostLeftText = previousPathSignpost;
            signpostLeft.GetComponent<Button>().onClick.AddListener((() => DisplaySignpost(_signpostLeftText)));
        }
        _signpostRightText = townInfo.signpost;
        signpostRight.GetComponent<Button>().onClick.AddListener((() => DisplaySignpost(_signpostRightText)));
        signpostBackground.GetComponentInChildren<Button>().onClick.AddListener((CloseSignpost));
    }
    
    private void DisplaySignpost(string text)
    {
        signpostBackground.GetComponentInChildren<TextMeshProUGUI>().text = text;
        signpostBackground.SetActive(true);
        TownEvents.OpenSignpost();
    }

    private void CloseSignpost()
    {
        signpostBackground.SetActive(false);
        TownEvents.CloseSignpost();
    }

    private void OnDestroy()
    {
        TownEvents.OnPublishTownInfo -= Setup;
    }
}
