using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillLevelupDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public string desc = "";


    public void OnPointerEnter(PointerEventData eventData)
    {
        ActiveSkillsManager.SkillDescChanged(desc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ActiveSkillsManager.SkillDescChanged("");
    }
}
