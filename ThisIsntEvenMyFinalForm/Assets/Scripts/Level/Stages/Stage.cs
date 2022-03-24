using System;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public int PowerLevel { get; set; }
    public List<Door> Doors { get; set; }
    public List<GameObject> Obtacles { get; set; }
    public float ObstacleImmunityDuration { get; set; }

    public Stage(int stageNumber)
    {
        PowerLevel = stageNumber;
    }
}
