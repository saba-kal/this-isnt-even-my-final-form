using System;
using UnityEngine;

[Serializable]
public class CharacterPowerLevelData
{
    public int PowerLevel = 1;
    public int HealthAmount;
    public float CharacterSpeed = 5f;
    public float ColliderRadius = 1f;
    public float AIDesiredDistanceFromPlayer = 10f;
    public GameObject Sprite;
    public GameObject BulletShooterContainer;
    public Health HealthObject;
}
