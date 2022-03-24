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
    [SerializeField] private GameObject _roatatingDoorPrefab;
    [SerializeField] private GameObject _stage2PillarPrefab;
    [SerializeField] private float _doorImmunityDuration;


    public Stage GenerateStage(int stageNumber)
    {
        //TODO: do this in a more generic way.
        switch (stageNumber)
        {
            case 1:
                return GenerateHexagonalStage();
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
}
