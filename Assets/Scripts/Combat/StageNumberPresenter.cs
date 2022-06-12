using TMPro;
using UnityEngine;

public class StageNumberPresenter : MonoBehaviour
{
    private TextMeshProUGUI _stageNumberText;

    private void Start()
    {
        _stageNumberText = GetComponent<TextMeshProUGUI>();
        CombatEvents.OnPublishStageNumber += UpdateStageNumbers;
    }

    private void UpdateStageNumbers(int stageNumber, int pathLength)
    {
        _stageNumberText.text = $"Stage {stageNumber}/{pathLength}";
    }

    private void OnDestroy()
    {
        CombatEvents.OnPublishStageNumber -= UpdateStageNumbers;
    }
}
