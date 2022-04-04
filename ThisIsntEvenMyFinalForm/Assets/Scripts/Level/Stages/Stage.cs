using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int PowerLevel;
    [SerializeField] private float _obstacleImmunityDuration;

    private List<Door> _doors;
    private List<Health> _healths;

    private void Awake()
    {
        _doors = GetComponentsInChildren<Door>().ToList();
        _healths = GetComponentsInChildren<Health>().ToList();

        foreach (var health in _healths)
        {
            health.SetImmune(true);
        }
    }

    public void OpenDoors()
    {
        foreach (var door in _doors)
        {
            door.OpenDoor();
        }
    }

    public void RemoveImmunity()
    {
        StartCoroutine(RemoveImmunityAfterTime());
    }

    private IEnumerator RemoveImmunityAfterTime()
    {
        yield return new WaitForSeconds(_obstacleImmunityDuration);

        foreach (var health in _healths)
        {
            health?.SetImmune(false);
        }
    }
}
