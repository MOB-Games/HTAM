using UnityEngine;

public class DropManager : MonoBehaviour
{
    private int _exp;
    private int _gold;
    
    private void Start()
    {
        CombatEvents.OnDrop += RegisterDrop;
        CombatEvents.OnWin += PublishFinalDrop;
    }

    private void RegisterDrop(Drop drop)
    {
        _exp += drop.Exp;
        _gold += drop.Gold;
    }

    private void PublishFinalDrop()
    {
        CombatEvents.FinalDrop(new Drop(_exp, _gold));
    }

    private void OnDestroy()
    {
        CombatEvents.OnDrop -= RegisterDrop;
        CombatEvents.OnWin -= PublishFinalDrop;
    }
}
