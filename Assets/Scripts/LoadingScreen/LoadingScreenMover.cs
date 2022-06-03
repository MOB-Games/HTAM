using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenMover : MonoBehaviour
{
    private Slider _slider;
    private void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        GameEvents.OnOpenLoadingScreen += Open;
    }

    private IEnumerator OpenGradually()
    {
        float elapsedTime = 0;
        const float seconds = 1.5f; 
        const float startingValue = 1f;
        while (elapsedTime < seconds)
        {
            _slider.value = Mathf.Lerp(startingValue, 0, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _slider.value = 0;
    }
    
    private void Open()
    {
        _slider.direction = CombatantInfo.Mirror ? Slider.Direction.LeftToRight : Slider.Direction.RightToLeft;
        StartCoroutine(OpenGradually());
    }

    private void OnDestroy()
    {
        GameEvents.OnOpenLoadingScreen -= Open;
    }
}
