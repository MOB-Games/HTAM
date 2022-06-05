using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CombatEvents.OnCombatantAdded += AddCombatant;
        Invoke(nameof(LoadScene), 0.5f);
        Invoke(nameof(OpenLoadingScreen), 1);
    }
    
    private void LoadScene()
    {
        GameEvents.LoadStage();
    }

    private static void AddCombatant(GameObject combatant)
    {
        CombatantInfo.AddCombatant(combatant);
    }

    private void OpenLoadingScreen()
    {
        GameEvents.OpenLoadingScreen();
    }

    private void EndCombat()
    {
        CombatantInfo.Reset();
    }

    private void OnDestroy()
    {
        CombatEvents.OnCombatantAdded -= AddCombatant;
        EndCombat();
    }
}
