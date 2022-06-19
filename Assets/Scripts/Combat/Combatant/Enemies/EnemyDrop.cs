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
    public int expDrop;
    public int goldDrop;

    private CombatantEvents _combatantEvents;

    private void Start()
    {
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnDied += Drop;
    }
    
    private int ExpDrop()
    {
        return expDrop;
    }
    
    private int GoldDrop()
    {
        return goldDrop;
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
