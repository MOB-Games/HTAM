using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
        var currentEnergy = CombatantInfo.GetStatBlock(userId).energy.value;
        var currentHp = CombatantInfo.GetStatBlock(userId).hp.value;
        foreach (var (skillWithLevel, index) in skillsWithLevels.Select((skillWithLevel, i) => (skillWithLevel, i)))
        {
            var inst = Instantiate(skillWithLevel.skillGo,
                targetLocation + Vector3.Scale(_buttonOffsets[index], offsetDirectionVector),
                Quaternion.identity, skillMenu.transform);
            var skill = inst.GetComponent<Skill>();
            var button = inst.GetComponent<Button>();
            button.onClick.AddListener(() => CombatEvents.SkillChosen(targetId, skill, skillWithLevel.level));
            button.interactable = skill.energyCost <= currentEnergy && skill.hpCost <= currentHp;
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
                var currentPos = tooltipBox.transform.position;
                var newPos = new Vector3(currentPos.x, hoveredGo.transform.position.y, currentPos.z);
                tooltipBox.transform.position = newPos;
                var tooltip = tooltipBox.GetComponentInChildren<TextMeshProUGUI>();
                var message = skill.GetDescription();
                if (!hoveredGo.GetComponent<Button>().interactable)
                {
                    message += "\n\n<color=red>*Insufficient energy or hp</color>";
                }
                tooltip.text = message;
                tooltipBox.SetActive(true);
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
