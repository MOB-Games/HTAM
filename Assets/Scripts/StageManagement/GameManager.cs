using System.Collections;
using Core.DataTypes;
using Core.Enums;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IntegerVariable gold;
    public static GameManager Instance { get; private set; }

    private const int StatIncrement = 3;
    private const int StatDecrement = 1;
    private const int IncrementChange = 1;

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
        CombatEvents.OnFinalDrop += LootGold;
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

    private void LootGold(Drop drop)
    {
        gold.value += drop.Gold;
    }
    
    public static int CalculateStatDelta(int baseDelta, bool percentageBased, int statBaseValue, int efficiency = 0)
    {
        var newDelta = percentageBased ? (baseDelta / 100) * statBaseValue : baseDelta - efficiency;
        return baseDelta switch
        {
            < 0 when newDelta >= 0 => -1,
            > 0 when newDelta <= 0 => 1,
            _ => newDelta
        };
    }
    
    public static IEnumerator PlayVisualEffect(GameObject visualEffect, Vector3 location)
    {
        var inst = Instantiate(visualEffect, location, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(inst);
    }
    
    public static int GetStatIncrement(StatType statToInc, StatType advantage, StatType disadvantage)
    {
        if (statToInc == advantage) return StatIncrement + IncrementChange;
        if (statToInc == disadvantage) return StatIncrement - IncrementChange;
        return StatIncrement;
    }
    
    public static int GetStatDecrement()
    {
        return StatDecrement;
    }

    private void OnDestroy()
    {
        CombatEvents.OnCombatantAdded -= AddCombatant;
        TownEvents.OnGoldSpent -= GoldSpent;
        CombatEvents.OnFinalDrop -= LootGold;
        EndCombat();
    }
}
