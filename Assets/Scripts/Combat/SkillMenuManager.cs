using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillMenuManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject skillMenu;
    public GameObject TooltipBox;

    private void Start()
    {
        CombatEvents.OnOpenMenu += SetupMenu;
        CombatEvents.OnSkillChosen += CloseMenu;
    }

    private void SetupMenu(CombatantId userId, CombatantId targetId, List<GameObject> skillPrefabs)
    {
        var currentEnergy = CombatantInfo.GetStatBlock(userId).energy.value;
        var currentHp = CombatantInfo.GetStatBlock(userId).hp.value;
        float buttonHeight = 3;
        foreach (var inst in skillPrefabs.Select(skillPrefab => Instantiate(skillPrefab,
                     new Vector3(0, buttonHeight, 0), Quaternion.identity, skillMenu.transform)))
        {
            buttonHeight -= 1.5f;
            var skill = inst.GetComponent<Skill>();
            var button = inst.GetComponent<Button>();
            button.onClick.AddListener(() => CombatEvents.SkillChosen(targetId, skill));
            button.interactable = skill.energyCost < currentEnergy && skill.hpCost < currentHp;
            if (!button.interactable)
                inst.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
        skillMenu.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var hoveredGo in eventData.hovered)
        {
            if (hoveredGo.TryGetComponent<Skill>(out var skill))
            {
                var currentPos = TooltipBox.transform.position;
                var newPos = new Vector3(currentPos.x, hoveredGo.transform.position.y, currentPos.z);
                TooltipBox.transform.position = newPos;
                var tooltip = TooltipBox.GetComponentInChildren<TextMeshProUGUI>();
                var message = skill.GetDescription();
                if (!hoveredGo.GetComponent<Button>().interactable)
                {
                    message += "\n\n<color=red>*Insufficient energy or hp</color>";
                }
                tooltip.text = message;
                TooltipBox.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipBox.SetActive(false);
    }

    private void CloseMenu(CombatantId targetId, Skill skill)
    {
        skillMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        CombatEvents.OnOpenMenu -= SetupMenu;
        CombatEvents.OnSkillChosen -= CloseMenu;
    }
}
