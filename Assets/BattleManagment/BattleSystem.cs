using System;
using System.Collections.Generic;
using System.Linq;
using Core.CharacterTypes;
using Core.Stats;
using TMPro;
using UnityEngine;
using Core.Enums;
using Random = UnityEngine.Random;

public class BattleSystem : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public GameObject[] enemyPrefabs;
    public GameIntEvent startTurnEvent;
    public GameObject winWindow;
    public GameObject loseWindow;
    public TextMeshProUGUI expText;

    private bool _battleFinished = false;
    private int _currentTurn;
    private int _exp = 0;
    private List<int> _turnOrder = new List<int>(6);
    private readonly Dictionary<int, int> _combatantsSpeed = new Dictionary<int, int>(6);
    private readonly Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>(3);

    private void Start()
    {
        _combatantsSpeed.Add(CharacterId.Player, characterInfo.GetStatBlock(CharacterId.Player).speed.value);
        // in the future will probably have a class of Path that will have a CreateRandomEnemies that will this
        // and the battle system will either have all paths or a reference to 1 that will update somehow 
        for (int i = CharacterId.Enemy1; i <= GetNumberOfEnemiesToCreate(); i++)
        {
            var enemyScript = CreateEnemy(GetEnemyPrefabToCreate(), i);
            _combatantsSpeed.Add(i, characterInfo.GetStatBlock(i).speed.value);
            _enemies.Add(i, enemyScript);
        }
        SetTurnOrderForRound();
        NextTurn();
    }

    private static int GetNumberOfEnemiesToCreate()
    {
        return Random.Range(0, 100) switch
        {
            < 30 => CharacterId.Enemy1,
            < 50 => CharacterId.Enemy2,
            _ => CharacterId.Enemy3
        };
    }

    private GameObject GetEnemyPrefabToCreate()
    {
        return Random.Range(0, 100) switch
        {
            < 75 => enemyPrefabs[0],
            _ => enemyPrefabs[1]
        };
    }
    
    private Enemy CreateEnemy(GameObject enemyPrefab, int id)
    {
        var inst = Instantiate(enemyPrefab, characterInfo.GetLocation(id), Quaternion.identity);
        var enemyScript = inst.GetComponent<Enemy>();
        enemyScript.id = id;
        enemyScript.InitStats();
        return enemyScript;
    }

    private void SetTurnOrderForRound()
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
        _currentTurn = nextTurnId;
        startTurnEvent.Raise(nextTurnId);
    }

    public void Death(int id)
    {
        _turnOrder.Remove(id);
        _combatantsSpeed.Remove(id);
        if (id == CharacterId.Player)
        {
            _battleFinished = true;
            loseWindow.SetActive(true);
        }
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
        if (id == _currentTurn) 
        {
            // if the combatant who's turn it is just dies he can't end his turn
            // this is mostly necessary because the next turn can be triggered before the death and then the turn is never played 
            NextTurn();
        }
    }
}