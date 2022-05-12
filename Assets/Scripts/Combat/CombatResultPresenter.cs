using TMPro;
using UnityEngine;

public class CombatResultPresenter : MonoBehaviour
{
    public GameObject winWindow;
    public GameObject loseWindow;
    public GameObject navigationArrows;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI goldText;

    private void Start()
    {
        CombatEvents.OnFinalDrop += RegisterLoot;
        CombatEvents.OnLose += Lose;
        CombatEvents.OnWin += Win;
    }

    private void RegisterLoot(Drop drop)
    {
        expText.text = drop.Exp.ToString();
        goldText.text = drop.Gold.ToString();
    }

    private void Win()
    {
        Invoke(nameof(DisplayWin), 2);
    }

    private void DisplayWin()
    {
        winWindow.SetActive(true);
        navigationArrows.SetActive(true);
    }
    
    private void Lose()
    {
        Invoke(nameof(DisplayLose), 2);
    }

    private void DisplayLose()
    {
        loseWindow.SetActive(true);
    }


    private void OnDestroy() 
    {
        CombatEvents.OnFinalDrop -= RegisterLoot;
        CombatEvents.OnLose -= Lose;
        CombatEvents.OnWin -= Win;
    }
}
