using System;
using Core.Enums;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

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
        Invoke(nameof(LoadScene), 0.5f);
        Invoke(nameof(PartyEnter), 1);
        Invoke(nameof(StartCombat), 2);
    }
    
    private void LoadScene()
    {
        CombatEvents.LoadScene();
    }

    private void PartyEnter()
    {
        CombatEvents.PartyEnter();
    }

    private void StartCombat()
    {
        CombatEvents.StartCombat();
    }

    private void EndCombat()
    {
        CombatantInfo.Reset();
    }

    private void OnDestroy()
    {
        EndCombat();
    }
}
