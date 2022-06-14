using Core.Enums;

[System.Serializable]
public class BlacksmithInfo
{
    private static readonly int StatIncrement = 3;
    private static readonly int StatDecrement = 1;
    private static readonly int IncrementChange = 1;
    public int price;
    public int maxDamage;
    public int minEnergyEfficiency;
    public int maxEnergyEfficiency;
    public int maxDefence;
    public int maxSpeed;

    public int GetIncrement(StatType statToInc, StatType advantage, StatType disadvantage)
    {
        if (statToInc == advantage) return StatIncrement + IncrementChange;
        if (statToInc == disadvantage) return StatIncrement - IncrementChange;
        return StatIncrement;
    }

    public int GetDecrement()
    {
        return StatDecrement;
    }
}
