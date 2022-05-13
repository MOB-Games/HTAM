using Core.Stats;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarModifier : MonoBehaviour
{
    private Slider _slider;

    public void Init(Stat energyStat)
    {
        _slider = GetComponentInChildren<Slider>();
        _slider.maxValue = energyStat.baseValue;
        _slider.value = energyStat.value;
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