using UnityEngine;
using System.Collections;

public class LevelMaster : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AIController _aiController;
    [SerializeField] private StageManager _stageManager;

    private void Start()
    {
        _aiController.SetDisabled(true);
    }

    public void StartLevel()
    {
        _stageManager.GenerateStages();
        _aiController.SetDisabled(false);

        var playerPowerLevelManager = _playerController.GetComponent<PowerLevelManager>();
        playerPowerLevelManager.PowerUp(0);
    }
}
