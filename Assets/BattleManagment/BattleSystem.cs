using System;
using System.Collections.Generic;
using System.Linq;
using Core.CharacterTypes;
using Core.Stats;
using TMPro;
using UnityEngine;
using Core.Enums;

public class BattleSystem : MonoBehaviour
{
    public Stat playerSpeed;
    public GameObject enemyPrefab;
    public GameIntEvent startTurnEvent;
    public GameObject winWindow;
    public TextMeshProUGUI expText;

    private bool _battleFinished = false;
    private int _exp = 0;
    private List<int> _turnOrder = new List<int>(6);
    private readonly Dictionary<int, int> _combatantsSpeed = new Dictionary<int, int>();
    private readonly Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();

    void Start()
    {
        _combatantsSpeed.Add(CharacterId.Player, playerSpeed.value);
        var inst = Instantiate(enemyPrefab, new Vector3(8, 0, 0), Quaternion.identity);
        var enemyScript = inst.GetComponent<EnemyScript>();
        _combatantsSpeed.Add(CharacterId.Enemy1, enemyScript.speed);
        _enemies.Add(CharacterId.Enemy1, enemyScript);
        SetTurnOrderForRound();
        NextTurn();
    }

    void SetTurnOrderForRound()
    {
        _turnOrder = _combatantsSpeed.OrderByDescending(c => c.Value).Select(c => c.Key).ToList();
    }

    public void NextTurn() 
    {
        if(_battleFinished)
            return;
        if (_turnOrder.Count == 0) 
            SetTurnOrderForRound();
        var nextTurnId = _turnOrder[0];
        _turnOrder.RemoveAt(0);
        startTurnEvent.Raise(nextTurnId);
    }

    public void Death(int id)
    {
        _combatantsSpeed.Remove(id);
        _turnOrder.Remove(id);
        if (_enemies.TryGetValue(id, out var deadEnemy))
        {
            _enemies.Remove(id);
            _exp += deadEnemy.ExpDrop();
            if (_enemies.Count == 0)
            {
                _battleFinished = true;
                expText.text = _exp.ToString();
                winWindow.SetActive(true);
            }
        }
    }
}
