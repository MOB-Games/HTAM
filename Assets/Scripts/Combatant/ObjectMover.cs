using System.Collections;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private bool _finishedMoving = false;
    private Vector3 _ogLocation;
    private CombatantEvents _combatantEvents;
    
    private void Start()
    {
        _ogLocation = CombatantInfo.GetLocation(GetComponent<ID>().id);
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
        float elapsedTime = 0;
        const float seconds = 1f;
        var startingPos = gameObject.transform.position;
        while (elapsedTime < seconds)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gameObject.transform.position = end;
        _finishedMoving = true;
    }

    private void MoveToTarget(Vector3 target)
    {
        var directionFromTarget = target.x > 0 ? Vector3.left : Vector3.right;
        StartCoroutine(Move(target + 3 * directionFromTarget));
    }

    private void Return()
    {
        StartCoroutine(Move(_ogLocation));
    }

    private void OnDestroy()
    {
        _combatantEvents.OnMoveToTarget -= MoveToTarget;
        _combatantEvents.OnReturn -= Return;
    }
}
