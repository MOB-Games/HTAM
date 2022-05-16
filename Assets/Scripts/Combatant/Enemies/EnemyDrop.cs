using UnityEngine;
using Random = UnityEngine.Random;

public class Drop
{
    public int Exp;
    public int Gold;

    public Drop(int exp, int gold)
    {
        Exp = exp;
        Gold = gold;
    }
}

public class EnemyDrop : MonoBehaviour
{
    // range of exp this enemy can give
    public int minExpDrop;
    public int maxExpDrop;
    
    // range of gold this enemy can give
    public int minGoldDrop;
    public int maxGoldDrop;

    private CombatantEvents _combatantEvents;

    private void Start()
    {
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnDied += Drop;
    }
    
    private int ExpDrop()
    {
        return Random.Range(minExpDrop, maxExpDrop);
    }
    
    private int GoldDrop()
    {
        return Random.Range(minGoldDrop, maxGoldDrop);
    }

    private void Drop()
    {
        CombatEvents.Drop(new Drop(ExpDrop(),GoldDrop()));
    }

    private void OnDestroy()
    {
        _combatantEvents.OnDied -= Drop;
    }
}
