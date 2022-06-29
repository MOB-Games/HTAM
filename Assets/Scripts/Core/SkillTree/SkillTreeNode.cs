using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataTypes;
using Core.SkillsAndConditions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class SkillTreeNode : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject dragImage;
    public List<SkillTreeNode> parents;
    public SkillBase content;
    public SkillWithLevel skillWithLevel;
    public IntegerVariable level;

    private bool _clickable;
    private bool _draggable;
    private bool _locked;
    private bool _isClick;
    private bool _isPassive;
    private int _maxLevel;
    private Vector3 _dragOffset;
    private SkillLevelupDescription _levelupDescription;
    private Image _image;
    private Camera _mainCamera;
    private GameObject _dragImageInstance;
    
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _mainCamera = Camera.main;

        skillWithLevel.level = level.value;
        _levelupDescription = GetComponent<SkillLevelupDescription>();
        _levelupDescription.Desc = content.GetLevelupDescription(skillWithLevel.level);
        _maxLevel = content.GetMaxLevel();
        _isPassive = content is not Skill;
        Refresh();

        TownEvents.OnSkillTreeRefresh += Refresh;
    }

    private void SetActivity()
    {
        if (parents.Any(stn => stn.skillWithLevel.level < 0))
        {
            _clickable = false;
            _draggable = false;
            _locked = true;
            return;
        }
        

        _clickable = skillWithLevel.level != _maxLevel;
        _draggable = skillWithLevel.level >= 0;
        _locked = false;
        _image.color = _draggable? Color.white : new Color(1, 1, 1, 0.5f);
    }

    private void SetDesc()
    {
        var desc = "";
        if (_locked)
            desc += "LOCKED\n";
        _levelupDescription.Desc = desc + content.GetLevelupDescription(skillWithLevel.level);
    }

    private void Refresh()
    {
        SetActivity();
        SetDesc();
    }

    private Vector3 GetMousePosition()
    {
        var pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickable && _isClick) TownEvents.LevelupSkill(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isClick = true;
        if (_isPassive || !_draggable) return;
        _dragOffset = transform.position - GetMousePosition();
        _dragImageInstance = Instantiate(dragImage, transform.position, Quaternion.identity, transform);
        _dragImageInstance.GetComponent<Image>().sprite = _image.sprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isPassive || !_draggable) return;
        Destroy(_dragImageInstance);
        foreach (var hoveredGo in eventData.hovered)
        {
            if (hoveredGo.TryGetComponent<OffensiveSlotIndex>(out var offensiveSlotIndex))
            {
                TownEvents.AddSkillToActive(this, offensiveSlotIndex.index, true);
                break;
            }
            if (hoveredGo.TryGetComponent<DefensiveSlotIndex>(out var defensiveSlotIndex))
            {
                TownEvents.AddSkillToActive(this, defensiveSlotIndex.index, false);
                break;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isClick = false;
        if (_isPassive || !_draggable) return;
        _dragImageInstance.transform.position = GetMousePosition() + _dragOffset;
    }

    private void OnDestroy()
    {
        TownEvents.OnSkillTreeRefresh -= Refresh;
        level.value = skillWithLevel.level;
    }
}
