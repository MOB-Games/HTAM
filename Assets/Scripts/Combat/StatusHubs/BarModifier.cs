using System.Collections;
using Core.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarModifier : MonoBehaviour
{
    private Slider _slider;
    private TextMeshProUGUI _text;
    
    public void Init(Stat stat)
    {
        _slider = GetComponentInChildren<Slider>();
        _slider.maxValue = stat.baseValue;
        _slider.value = stat.value;
        _slider.direction = transform.position.x > 0 ? Slider.Direction.RightToLeft : Slider.Direction.LeftToRight;
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = stat.value.ToString();
    }

    private IEnumerator ChangeGradually(float endValue)
    {
        float elapsedTime = 0;
        var seconds = 0.2f + Mathf.Abs(_slider.value - endValue) / _slider.maxValue; 
        var startingValue = _slider.value;
        while (elapsedTime < seconds)
        {
            var newValue = Mathf.Lerp(startingValue, endValue, elapsedTime / seconds);
            _slider.value = newValue;
            _text.text = ((int)newValue).ToString();
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _slider.value = endValue;
        _text.text = ((int)endValue).ToString();
    }

    public void Change(int delta, bool percentage)
    {
        delta = ChangeCalculator.Calculate(delta, percentage, (int)_slider.maxValue);
        var newValue = _slider.value + delta;
        if (newValue < 0)
            newValue = 0;
        else if (newValue > _slider.maxValue)
            newValue = _slider.maxValue;
        StartCoroutine(ChangeGradually(newValue));
    }
}
