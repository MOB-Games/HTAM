using UnityEngine;
using UnityEngine.UI;

public class SkillTreeEdge : MonoBehaviour
{
    public SkillTreeNode source;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        TownEvents.OnSkillTreeRefresh += Refresh;
        Refresh(0);
        if (source.level.value >= 0)
            _image.color = Color.white;
    }

    private void Refresh(int _)
    {
        _image.color = source.skillWithLevel.level >= 0 ? Color.white : Color.black;
    }

    private void OnDestroy()
    {
        TownEvents.OnSkillTreeRefresh -= Refresh;
    }
}
