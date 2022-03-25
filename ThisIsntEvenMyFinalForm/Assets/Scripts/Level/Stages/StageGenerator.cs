using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGenerator : MonoBehaviour
{

    [SerializeField] private GameObject _foregroundContainer;

    [Header("Stage 1")]
    [SerializeField] private GameObject _slidingDoorPrefab;
    [SerializeField] private GameObject _stage1PillarPrefab;
    [SerializeField] private float _pillarImmunityDuration;
    [SerializeField] private int _pillarHealth;


    [Header("Stage 2")]
    [SerializeField] private GameObject _rotatingDoorPrefab;
    [SerializeField] private float _doorImmunityDuration;
    [SerializeField] private int _doorHealth;


    public Stage GenerateStage(int stageNumber)
    {
        //TODO: do this in a more generic way.
        switch (stageNumber)
        {
            case 1:
                return GenerateHexagonalStage();
            case 2:
                return GenerateSquareStage();
            default:
                Debug.LogError($"Stage {stageNumber} not implemented yet.");
                return null;
        }
    }

    private Stage GenerateHexagonalStage()
    {
        var stage = new Stage(1)
        {
            Obtacles = new List<GameObject>(),
            Doors = new List<Door>(),
            ObstacleImmunityDuration = _pillarImmunityDuration
        };

        const int hypotenuse = 10;
        for (var i = 0; i < 6; i++)
        {
            var angle = 2 * Mathf.PI * (i / 6f);

            var pillar = Instantiate(_stage1PillarPrefab, _foregroundContainer.transform);
            pillar.transform.position = new Vector2(
                hypotenuse * Mathf.Cos(angle),
                hypotenuse * Mathf.Sin(angle));
            pillar.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg + 120f, Vector3.forward);

            var pillarHealth = pillar.AddComponent<Health>();
            pillarHealth.MaxHealth = _pillarHealth;
            pillarHealth.SetImmune(true);

            stage.Obtacles.Add(pillar);

            var door = Instantiate(_slidingDoorPrefab, pillar.transform);
            var slidingDoor = door.GetComponent<SlidingDoor>();
            if (slidingDoor != null)
            {
                stage.Doors.Add(slidingDoor);
            }
        }

        return stage;
    }

    private Stage GenerateSquareStage()
    {
        var stage = new Stage(2)
        {
            Obtacles = new List<GameObject>(),
            Doors = new List<Door>(),
            ObstacleImmunityDuration = _doorImmunityDuration
        };

        const float distance = 20f;
        var squareCorners = new[] {
            new Vector2(-distance, distance),
            new Vector2(distance, distance),
            new Vector2(distance, -distance),
            new Vector2(-distance, -distance)
        };

        for (var i = 0; i < squareCorners.Length; i++)
        {
            var door = Instantiate(_rotatingDoorPrefab, _foregroundContainer.transform);
            door.transform.position = squareCorners[i];
            door.transform.rotation = Quaternion.AngleAxis(-90 * i, Vector3.forward);

            var health = door.AddComponent<Health>();
            health.MaxHealth = _pillarHealth;
            health.SetImmune(true);

            stage.Obtacles.Add(door);

            var rotatingDoor = door.GetComponent<RotatingDoor>();
            if (rotatingDoor != null)
            {
                stage.Doors.Add(rotatingDoor);
            }
        }

        return stage;
    }
}
