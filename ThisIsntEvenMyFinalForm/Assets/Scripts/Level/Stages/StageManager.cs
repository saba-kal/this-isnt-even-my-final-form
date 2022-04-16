using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManager : MonoBehaviour
{
    [SerializeField] private int _maxStages;
    [SerializeField] private List<Stage> _stages;

    private int _currentMaximumPowerLevel = 1;

    private void Awake()
    {
        SetStagesActive(false);
    }

    public void SetStagesActive(bool active)
    {
        foreach (var stage in _stages)
        {
            stage.gameObject.SetActive(active);
        }
    }

    public void GoToNextStage(int powerLevel)
    {
        if (powerLevel > _currentMaximumPowerLevel)
        {
            _currentMaximumPowerLevel = powerLevel;
        }
        else
        {
            return;
        }

        var previousPowerLevel = _currentMaximumPowerLevel - 1;
        var currentStage = _stages.FirstOrDefault(s => s.PowerLevel == previousPowerLevel);
        if (currentStage == null)
        {
            Debug.LogError($"A stage does not exist for power level {previousPowerLevel}.");
            return;
        }

        currentStage.OpenDoors();
        currentStage.RemoveImmunity();
    }
}
