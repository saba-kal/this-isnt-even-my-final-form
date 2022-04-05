using UnityEngine;
using System.Collections;

public class LevelMaster : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AIController _aiController;
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private GameObject _endGameScreen;
    [SerializeField] private TutorialStage _tutorialStage;

    private void OnEnable()
    {
        CharacterHealth.OnHealthLost += OnCharacterHealthLost;
        CharacterHealth.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        CharacterHealth.OnHealthLost -= OnCharacterHealthLost;
        CharacterHealth.OnDeath -= OnDeath;
    }

    private void Start()
    {
        _endGameScreen.SetActive(false);
        _aiController.SetDisabled(true);
    }

    public void StartLevel()
    {
        SetGameplayDisabled(true);
        _tutorialStage.RemoveTutorialTooltips();
        DialogueManager.Instance.StartInitialConversation(() =>
        {
            _tutorialStage.RemoveTutorialStage(() =>
            {
                CinemachineShake.Instance?.Shake();
                _stageManager.SetStagesActive(true);
                SetGameplayDisabled(false);

                var playerPowerLevelManager = _playerController.GetComponent<PowerLevelManager>();
                playerPowerLevelManager.PowerUp();
            });
        });
    }

    private void OnCharacterHealthLost(CharacterHealth characterHealth)
    {
        var playerIsWinner = characterHealth.gameObject.layer == (int)CollisionLayer.Enemy;

        SetGameplayDisabled(true);
        DialogueManager.Instance.StartConversation(playerIsWinner, () =>
        {
            SetGameplayDisabled(false);

            var playerPowerLevelManager = characterHealth.GetComponent<PowerLevelManager>();
            playerPowerLevelManager.PowerUp();
        });
    }

    private void OnDeath(CharacterHealth characterHealth)
    {
        SetGameplayDisabled(true);
        DialogueManager.Instance.StartEndGameConversation(() =>
        {
            //TODO: death effects?
            _endGameScreen.SetActive(true);
        });
    }

    private void SetGameplayDisabled(bool disabled)
    {
        _aiController.SetDisabled(disabled);
        _playerController.SetDisabled(disabled);
    }
}
