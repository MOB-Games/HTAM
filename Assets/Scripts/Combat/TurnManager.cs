using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private bool _battleFinished = false;
    private int _numEnemies;
    private CombatantId _currentTurn;
    private List<CombatantId> _turnOrder = new List<CombatantId>(6);
    private readonly Dictionary<CombatantId, int> _combatantsSpeed = new Dictionary<CombatantId, int>(6);

    private void Start()
    {
        CombatEvents.OnStartCombat += StartCombat;
        CombatEvents.OnEndTurn += NextTurn;
        CombatEvents.OnCombatantDied += RemoveCombatant;
    }

    private void StartCombat()
    {
        // getting active party members
        foreach (var id in new List<CombatantId>()
                 { CombatantId.Player, CombatantId.PartyMemberTop, CombatantId.PartyMemberBottom })
        {
            try
            { _combatantsSpeed.Add(id, CombatantInfo.GetStatBlock(id).speed.value); }
            catch (KeyNotFoundException)
            { /*ignored*/ }
        }
        // getting enemies
        foreach (var id in new List<CombatantId>()
                 { CombatantId.EnemyCenter, CombatantId.EnemyTop, CombatantId.EnemyBottom })
        {
            try
            {
                _combatantsSpeed.Add(id, CombatantInfo.GetStatBlock(id).speed.value);
                _numEnemies++;
            }
            catch (KeyNotFoundException)
            { /*ignored*/ }
        }
        NextTurn();
    }

    private void SetTurnOrderForRound()
    {
        _turnOrder = _combatantsSpeed.OrderByDescending(c => c.Value)
            .Select(c => c.Key).ToList();
    }

    private void NextTurn() 
    {
        if(_battleFinished)
            return;
        if (_turnOrder.Count == 0) 
            SetTurnOrderForRound();
        var nextTurnId = _turnOrder[0];
        _turnOrder.RemoveAt(0);
        _currentTurn = nextTurnId;
        CombatEvents.StartTurn(nextTurnId);
    }

    private static bool IsEnemy(CombatantId id)
    {
        return id is CombatantId.EnemyCenter or CombatantId.EnemyTop or CombatantId.EnemyBottom;
    }

    private void RemoveCombatant(CombatantId id)
    {
        _turnOrder.Remove(id);
        _combatantsSpeed.Remove(id);
        if (id == CombatantId.Player)
        {
            _battleFinished = true;
            CombatEvents.Lose();
        }
        if (IsEnemy(id))
        {
            _numEnemies--;
            if (_numEnemies == 0)
            {
                _battleFinished = true;
                CombatEvents.Win();
            }
        }
        if (id == _currentTurn) 
        {
            // if the combatant who's turn it is just dies he can't end his turn
            // this is mostly necessary because the next turn can be triggered before the death and then the turn is never played 
            NextTurn();
        }
    }

    private void OnDestroy()
    {
        CombatEvents.OnStartCombat -= StartCombat;
        CombatEvents.OnEndTurn -= NextTurn;
        CombatEvents.OnCombatantDied -= RemoveCombatant;
    }
}
