using UnityEngine;
using System.Collections;
using Cinemachine;
using System.Collections.Generic;

public class CinemachineTargetGroupRadiusUpdater : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _cinemachinTargetGroup;
    [SerializeField] private List<float> _radiusPerPowerLevel;

    private int _maxPowerLevel = 1;

    private void OnEnable()
    {
        UpdateTargetGroupRadius();
        PowerLevelManager.OnPowerUp += OnPowerUp;
    }

    private void OnDisable()
    {
        PowerLevelManager.OnPowerUp -= OnPowerUp;
    }

    private void OnPowerUp(int powerLevel)
    {
        if (powerLevel <= _maxPowerLevel)
        {
            return;
        }

        _maxPowerLevel = powerLevel;
        UpdateTargetGroupRadius();
    }

    private void UpdateTargetGroupRadius()
    {
        var radiusIndex = _maxPowerLevel - 1;
        if (radiusIndex < 0 || radiusIndex >= _radiusPerPowerLevel.Count)
        {
            Debug.LogError($"Invalid camera radius index of {radiusIndex}.");
            return;
        }

        var newTargets = new CinemachineTargetGroup.Target[_cinemachinTargetGroup.m_Targets.Length];
        for (var i = 0; i < _cinemachinTargetGroup.m_Targets.Length; i++)
        {
            var existingTarget = _cinemachinTargetGroup.m_Targets[i];
            newTargets[i] = new CinemachineTargetGroup.Target
            {
                target = existingTarget.target,
                radius = _radiusPerPowerLevel[radiusIndex],
                weight = existingTarget.weight
            };
        }

        _cinemachinTargetGroup.m_Targets = newTargets;
    }
}
