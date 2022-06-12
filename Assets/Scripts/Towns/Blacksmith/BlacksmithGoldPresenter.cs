using Core.DataTypes;
using TMPro;
using UnityEngine;

public class BlacksmithGoldPresenter : MonoBehaviour
{
    private IntegerVariable _gold;
    private TextMeshProUGUI _goldText;

    private void Start()
    {
        _gold = GameManager.Instance.gold;
        _goldText = GetComponent<TextMeshProUGUI>();
        UpdateGold(0);
        TownEvents.OnGoldSpent += UpdateGold;
    }

    private void UpdateGold(int _)
    {
        _goldText.text = $"Gold: {_gold.value}";
    }

    private void OnDestroy()
    {
        TownEvents.OnGoldSpent -= UpdateGold;
    }
}
