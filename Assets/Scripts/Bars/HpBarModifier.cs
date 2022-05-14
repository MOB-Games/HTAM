using System.Collections;
using Core.Stats;
using UnityEngine;
using UnityEngine.UI;

public class HpBarModifier : MonoBehaviour
{
    private Slider _slider;
    
    public void Init(Stat hpStat)
    {
        _slider = GetComponentInChildren<Slider>();
        _slider.maxValue = hpStat.baseValue;
        _slider.value = hpStat.value;
    }

    private IEnumerator ChangeGradually(float newValue)
    {
        float elapsedTime = 0;
        var seconds = 0.2f + Mathf.Abs(_slider.value - newValue) / _slider.maxValue; 
        var startingValue = _slider.value;
        while (elapsedTime < seconds)
        {
            _slider.value = Mathf.Lerp(startingValue, newValue, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _slider.value = newValue;
    }

    public void Change(int delta)
    {
        var newValue = _slider.value + delta;
        if (newValue < 0)
            newValue = 0;
        else if (newValue > _slider.maxValue)
            newValue = _slider.maxValue;
        StartCoroutine(ChangeGradually(newValue));
    }
}
