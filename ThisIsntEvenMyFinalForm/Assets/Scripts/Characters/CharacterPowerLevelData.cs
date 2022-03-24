using System;
using UnityEngine;

[Serializable]
public class CharacterPowerLevelData
{
    public int PowerLevel = 1;
    public float CharacterSpeed = 5f;
    public float AIDesiredDistanceFromPlayer = 10f;
    public GameObject Sprite;
}
