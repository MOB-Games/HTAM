using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuManager : MonoBehaviour
{
    public GameObject skillMenu;

    private void Start()
    {
        CombatEvents.OnOpenMenu += SetupMenu;
        CombatEvents.OnSkillChosen += CloseMenu;
    }

    private void SetupMenu(CombatantId targetId, List<GameObject> skillPrefabs)
    {
        float buttonHeight = 3;
        foreach (var inst in skillPrefabs.Select(skillPrefab => Instantiate(skillPrefab,
                     new Vector3(0, buttonHeight, 0), Quaternion.identity, skillMenu.transform)))
        {
            buttonHeight -= 1.5f;
            var skill = inst.GetComponent<Skill>();
            inst.GetComponent<Button>().onClick.AddListener(() => CombatEvents.SkillChosen(targetId, skill));
        }
        skillMenu.SetActive(true);
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
