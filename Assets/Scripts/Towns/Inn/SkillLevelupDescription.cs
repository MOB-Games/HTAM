using UnityEngine;
using UnityEngine.EventSystems;

public class SkillLevelupDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Desc
    {
        get => _desc;
        set
        {
            _desc = value;
            if (_showing)
                ActiveSkillsManager.SkillDescChanged(_desc);
        }
    }

    private string _desc = "";
    private bool _showing;


    public void OnPointerEnter(PointerEventData eventData)
    {
        _showing = true;
        ActiveSkillsManager.SkillDescChanged(Desc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _showing = false;
        ActiveSkillsManager.SkillDescChanged("");
    }
}
