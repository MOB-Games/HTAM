using Core.DataTypes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IntegerVariable gold;
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
        TownEvents.OnGoldSpent += GoldSpent;
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

    private void GoldSpent(int amount)
    {
        gold.value -= amount;
    }
    
    public static int CalculateStatDelta(int baseDelta, bool percentageBased, int statBaseValue, int efficiency = 0)
    {
        return percentageBased ? (baseDelta / 100) * statBaseValue : baseDelta - efficiency;
    }

    private void OnDestroy()
    {
        CombatEvents.OnCombatantAdded -= AddCombatant;
        TownEvents.OnGoldSpent -= GoldSpent;
        EndCombat();
    }
}
