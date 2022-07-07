using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Core.SkillsAndConditions;
using UnityEngine.EventSystems;

public class ConditionIcon
{
    public readonly ConditionId Id;
    public readonly GameObject ConditionGo;
    public int Ticks;

    public ConditionIcon(GameObject conditionGo, ConditionId id)
    {
        Id = id;
        ConditionGo = conditionGo;
        Ticks = 0;
    }
}

public class StatusHub : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CombatantId id;
    public HpBarModifier hpBarModifier;
    public EnergyBarModifier energyBarModifier;
    public TextMeshProUGUI nameText;
    public GameObject tooltipBox;
    public GameObject glow;

    private bool _inCombat = true; 
    private const int NumberOfIconsInFirstRow = 8;
    [CanBeNull] private CombatantEvents _combatantEvents;
    private readonly List<ConditionIcon> _conditions = new();

    private readonly List<int> _levels = new();

    private void Start()
    {
        CombatEvents.OnWin += CombatEnded;
        CombatEvents.OnLose += CombatEnded;
    }

    private static Vector3 GetConditionIconLocation(int index)
    {
        return index < NumberOfIconsInFirstRow ? new Vector3(-0.9f + 0.3f * index, 0f, 0) : 
            new Vector3(-0.9f + 0.3f * index - NumberOfIconsInFirstRow, -0.9f, 0);
    }

    public void Connect(GameObject combatant)
    {
        nameText.text = combatant.name.Split("(")[0];
        var stats = combatant.GetComponent<StatModifier>().stats;
        _combatantEvents = combatant.GetComponent<CombatantEvents>();
        if (_combatantEvents == null)
            throw new NullReferenceException("Status Hub found no combatant events component");
        _combatantEvents.OnStatChange += ModifyBars;
        _combatantEvents.OnConditionAdded += AddConditionIcon;
        _combatantEvents.OnConditionRemoved += RemoveConditionIcon;
        _combatantEvents.OnEndTurn += Tick;
        CombatEvents.OnStartTurn += StartGlow;
        if (CombatantInfo.Mirror)
            transform.position = Vector3.Scale(transform.position,new Vector3(-1,1,1));
        hpBarModifier.Init(stats.hp);
        energyBarModifier.Init(stats.energy);
        foreach (var condition in combatant.GetComponent<ConditionManager>().conditions)
        {
            var conditionId = condition.conditionGo.GetComponent<Condition>().id;
            var inst = Instantiate(condition.conditionGo, Vector3.zero, Quaternion.identity, transform);
            inst.transform.localPosition = GetConditionIconLocation(_conditions.Count);
            _conditions.Add(new ConditionIcon(inst, conditionId));
            _levels.Add(condition.level);
        }
    }

    private void CombatEnded()
    {
        _inCombat = false;
    }

    private void ModifyBars(StatType stat, int delta)
    {
        switch (stat)
        {
            case StatType.Hp:
                hpBarModifier.Change(delta);
                break;
            case StatType.Energy:
                energyBarModifier.Change(delta);
                break;
            default:
                return;
        }
    }

    private void StartGlow(CombatantId targetId)
    {
        if (targetId != id) return;
        nameText.color = Color.white;
        glow.SetActive(true);
    }

    private void StopGlow()
    {
        nameText.color = Color.black;
        glow.SetActive(false);
    }

    private void AddConditionIcon(GameObject conditionGo, ConditionId conditionId, int level)
    {
        var conditionIcon = _conditions.Find(c => c.Id == conditionId);
        if (conditionIcon != null)
        {
            conditionIcon.Ticks = 0;
            return;
        }
        var inst = Instantiate(conditionGo, Vector3.zero, Quaternion.identity, transform);
        inst.transform.localPosition = GetConditionIconLocation(_conditions.Count);
        _conditions.Add(new ConditionIcon(inst, conditionId));
        _levels.Add(level);
    }

    private void RemoveConditionIcon(ConditionId conditionId)
    {
        var conditionIcon = _conditions.Find(c => c.Id == conditionId);
        var conditionIndex = _conditions.FindIndex(c => c.Id == conditionId);
        _levels.RemoveAt(conditionIndex);
        _conditions.Remove(conditionIcon);
        Destroy(conditionIcon.ConditionGo);
        foreach (var (condition, index) in _conditions.Select((c,i)=> (c,i)))
        {
            condition.ConditionGo.transform.localPosition = GetConditionIconLocation(index);
        }
    }

    private void Tick()
    {
        foreach (var condition in _conditions)
            condition.Ticks++;
        StopGlow();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_inCombat) return;
        foreach (var hoveredGo in eventData.hovered)
        {
            if (hoveredGo.TryGetComponent<Condition>(out var condition))
            {
                var conditionIndex = _conditions.FindIndex(c => c.ConditionGo == condition.gameObject);
                var desc = "";
                if (condition is TurnSkipCondition turnSkipCondition)
                    desc += turnSkipCondition.GetDescription(_levels[conditionIndex]);
                else 
                    desc += condition.GetDescription(_levels[conditionIndex]);
                desc +=
                    $"\n\nExpires in {condition.TurnsLeft(_conditions[conditionIndex].Ticks, _levels[conditionIndex])} Turns";
                tooltipBox.GetComponentInChildren<TextMeshProUGUI>().text = desc;
                tooltipBox.SetActive(true);
                break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipBox.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_combatantEvents != null)
        {
            _combatantEvents.OnStatChange -= ModifyBars;
            _combatantEvents.OnConditionAdded -= AddConditionIcon;
            _combatantEvents.OnConditionRemoved -= RemoveConditionIcon;
            _combatantEvents.OnEndTurn -= Tick;
        }
        CombatEvents.OnStartTurn -= StartGlow;
        CombatEvents.OnWin -= CombatEnded;
        CombatEvents.OnLose -= CombatEnded;
    }
}
