using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManager : MonoBehaviour
{
    [SerializeField] private int _maxStages;
    [SerializeField] private StageGenerator _stageGenerator;

    private int _currentMaximumPowerLevel = 1;
    private List<Stage> _stages = new List<Stage>();

    private void OnEnable()
    {
        PowerLevelManager.OnPowerUp += OnPowerUp;
    }

    private void OnDisable()
    {
        PowerLevelManager.OnPowerUp -= OnPowerUp;
    }

    public void GenerateStages()
    {
        for (int i = 1; i <= _maxStages; i++)
        {
            var stage = _stageGenerator.GenerateStage(i);
            if (stage != null)
            {
                _stages.Add(stage);
            }
        }
    }

    private void OnPowerUp(int powerLevel)
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

        foreach (var door in currentStage.Doors)
        {
            door.OpenDoor();
        }

        var previousStage = _stages.FirstOrDefault(s => s.PowerLevel == _currentMaximumPowerLevel - 2);
        if (previousStage != null)
        {
            StartCoroutine(RemoveObstacleImmunity(currentStage.Obtacles, currentStage.ObstacleImmunityDuration));
        }
    }

    private IEnumerator RemoveObstacleImmunity(List<GameObject> destructibleObjects, float time)
    {
        yield return new WaitForSeconds(time);

        foreach (var destructibleObject in destructibleObjects)
        {
            var gameObjectHealth = destructibleObject.GetComponent<Health>();
            gameObjectHealth?.SetImmune(false);
        }
    }
}
