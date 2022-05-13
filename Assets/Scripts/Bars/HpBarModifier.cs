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

    public void Change(int delta)
    {
        var newValue = _slider.value + delta;
        if (newValue < 0)
            newValue = 0;
        else if (newValue > _slider.maxValue)
            newValue = _slider.maxValue;
        _slider.value = newValue;
    }
}
