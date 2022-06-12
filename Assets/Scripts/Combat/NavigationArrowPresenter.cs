using TMPro;
using UnityEngine;

public class NavigationArrowPresenter : MonoBehaviour
{
    public GameObject navigationArrowsGo;
    public TextMeshProUGUI returnToTownText;
    
    private void Start()
    {
        CombatEvents.OnWin += Win;
    }

    private void Win()
    {
        Invoke(nameof(Present), 2);
    }

    private void Present()
    {
        var returnTextX = CombatantInfo.Mirror ? 410 : -410;
        returnToTownText.transform.localPosition = new Vector3(returnTextX, 0, 0);
        navigationArrowsGo.SetActive(true);
    }

    private void OnDestroy()
    {
        CombatEvents.OnWin -= Win;
    }
}
