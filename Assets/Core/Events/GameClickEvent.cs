using Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Game Event (click)")]
public class GameClickEvent : GameEventTemplate<PointerEventData> { }
