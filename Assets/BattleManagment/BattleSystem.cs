using System;
using System.Collections.Generic;
using System.Linq;
using Core.Events;
using Core.Stats;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public Stat playerSpeed;
    public GameObject enemyPrefab;
    public GameIntEvent startTurnEvent;
    private int _enemySpeed;
    private Stack<int> _turnOrder = new Stack<int>(2);

    void Start()
    {
        var inst = Instantiate(enemyPrefab, new Vector3(8, 0, 0), Quaternion.identity);
        _enemySpeed = inst.GetComponent<EnemyScript>().speed;
        SetTurnOrderForRound();
        NextTurn();
    }

    void SetTurnOrderForRound()
    {
        _turnOrder.Push(2);
        _turnOrder.Push(1);
    }

    public void NextTurn()
    {
        if (_turnOrder.Count == 0) 
            SetTurnOrderForRound();
        startTurnEvent.Raise(_turnOrder.Pop());
    }
}
