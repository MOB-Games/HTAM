using System.Collections;
using Core.Enums;
using UnityEngine;

public class CombatantMover : MonoBehaviour
{
    private bool _finishedMoving = false;
    private CombatantId _id;
    private Vector3 _deviation;
    private CombatantEvents _combatantEvents;
    
    private void Start()
    {
        _id = GetComponent<CombatId>().id;
        var bounds = GetComponent<SpriteRenderer>().bounds;
        var referencePoint = CombatantInfo.GetLocation(_id).x < 0 ? bounds.min : new Vector3(bounds.max.x, bounds.min.y, 0);
        _deviation = transform.position - referencePoint; 
        _combatantEvents = GetComponent<CombatantEvents>();
        _combatantEvents.OnMoveToTarget += MoveToTarget;
        _combatantEvents.OnReturn += Return;
    }

    private void Update()
    {
        if (_finishedMoving)
        {
            _combatantEvents.FinishedMoving();
            _finishedMoving = false;
        }
    }

    private IEnumerator Move(Vector3 end)
    {
        end += _deviation;
        float elapsedTime = 0;
        var startingPos = gameObject.transform.position;
        var seconds = Vector3.Distance(startingPos, end) / 8;
        while (elapsedTime < seconds)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gameObject.transform.position = end;
        _finishedMoving = true;
    }

    private void MoveToTarget(CombatantId targetId)
    {
        var targetLocation = CombatantInfo.GetLocation(targetId);
        var directionFromTarget = targetLocation.x > 0 ? Vector3.left : Vector3.right;
        var distanceFromTarget = CombatantInfo.GetDimensions(_id).Width + CombatantInfo.GetDimensions(targetId).Width;
        StartCoroutine(Move(targetLocation + distanceFromTarget * directionFromTarget));
    }

    private void Return()
    {
        StartCoroutine(Move(CombatantInfo.GetLocation(_id)));
    }

    private void OnDestroy()
    {
        _combatantEvents.OnMoveToTarget -= MoveToTarget;
        _combatantEvents.OnReturn -= Return;
    }
}
