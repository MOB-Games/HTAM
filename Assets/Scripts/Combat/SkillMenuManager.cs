using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Core.SkillsAndConditions;
using Core.DataTypes;

public class SkillMenuManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject skillMenu;
    public GameObject tooltipBox;

    private readonly List<GameObject> _buttons = new List<GameObject>();

    private readonly List<Vector3> _buttonOffsets = new List<Vector3>()
    {
        new Vector3(0.5f,0,0),
        new Vector3(0.5f,0.75f,0),
        new Vector3(0.5f,-0.75f,0),
        new Vector3(0.5f,1.5f,0),
        new Vector3(0.5f,-1.5f,0),
        new Vector3(-0.25f,1.5f,0),
        new Vector3(-0.25f,-1.5f,0),
        new Vector3(-1f,1.5f,0),
        new Vector3(-1f,-1.5f,0)
    };
    private void Start()
    {
        CombatEvents.OnOpenMenu += SetupMenu;
        CombatEvents.OnSkillChosen += CloseMenu;
    }

    private void SetupMenu(CombatantId userId, CombatantId targetId, List<SkillWithLevel> skillsWithLevels)
    {
        var targetDimensions = CombatantInfo.GetDimensions(targetId);
        var targetLocation = CombatantInfo.GetLocation(targetId);
        var offsetDirectionVector = new Vector3(targetLocation.x > 0 ? -1 : 1, 1 , 0);
        targetLocation += new Vector3(offsetDirectionVector.x * targetDimensions.Width,
            targetDimensions.Height / 2f, 0);
        skillMenu.transform.position = targetLocation;
        foreach (var button in _buttons)
            Destroy(button);
        var statBlock = CombatantInfo.GetStatBlock(userId);
        var energy = statBlock.energy;
        var energyEfficiency = statBlock.energyEfficiency.value;
        var hp = statBlock.hp;
        foreach (var (skillWithLevel, index) in skillsWithLevels.Select((skillWithLevel, i) => (skillWithLevel, i)))
        {
            var inst = Instantiate(skillWithLevel.skillGo,
                targetLocation + Vector3.Scale(_buttonOffsets[index], offsetDirectionVector),
                Quaternion.identity, skillMenu.transform);
            var skill = inst.GetComponent<Skill>();
            var button = inst.GetComponent<Button>();
            button.onClick.AddListener(() => CombatEvents.SkillChosen(targetId, skill, skillWithLevel.level));
            var energyCost = GameManager.CalculateStatDelta(skill.energyCost, skill.costIsPercentBased, 
                energy.baseValue, energyEfficiency);
            var hpCost = GameManager.CalculateStatDelta(skill.hpCost, skill.costIsPercentBased, hp.baseValue);
            button.interactable = energyCost <= energy.value && hpCost <= hp.value;
            if (!button.interactable)
                inst.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            _buttons.Add(inst);
        }
        skillMenu.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var hoveredGo in eventData.hovered)
        {
            if (hoveredGo.TryGetComponent<Skill>(out var skill))
            {
                var message = skill.GetDescription();
                if (!hoveredGo.GetComponent<Button>().interactable)
                {
                    message += $"\n\n<color=red>*Insufficient {(skill.energyCost > 0 ? "energy" : "HP")}</color>";
                }
                tooltipBox.GetComponentInChildren<TextMeshProUGUI>().text = message;
                tooltipBox.SetActive(true);
                break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipBox.SetActive(false);
    }

    private void CloseMenu(CombatantId targetId, Skill skill, int level)
    {
        skillMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        CombatEvents.OnOpenMenu -= SetupMenu;
        CombatEvents.OnSkillChosen -= CloseMenu;
    }
}
