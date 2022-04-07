using UnityEngine;
using System.Collections;
using System;

public class LevelMaster : MonoBehaviour
{
    public delegate void StageStartEvent();
    public static StageStartEvent OnStageStart;

    [SerializeField] private float _slowMotionSpeed = 0.2f;
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
            SoundManager.Instance?.Stop(SoundClipNames.TUTORIAL_MUSIC);
            SoundManager.Instance?.Play(SoundClipNames.MAIN_BATTLE_MUSIC);
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

        StartCoroutine(SlowGameDown(() =>
        {
            SetGameplayDisabled(true);
            OnStageStart?.Invoke();
            DialogueManager.Instance.StartConversation(playerIsWinner, () =>
            {
                SetGameplayDisabled(false);

                var playerPowerLevelManager = characterHealth.GetComponent<PowerLevelManager>();
                playerPowerLevelManager.PowerUp();

                if (_aiController.ReachedMaxPowerLevel())
                {
                    SoundManager.Instance?.Stop(SoundClipNames.MAIN_BATTLE_MUSIC);
                    SoundManager.Instance?.Play(SoundClipNames.FINAL_BATTLE_MUSIC);
                }
            });
        }));
    }

    private void OnDeath(CharacterHealth characterHealth)
    {
        var playerIsWinner = characterHealth.gameObject.layer == (int)CollisionLayer.Enemy;

        SetGameplayDisabled(true);
        DialogueManager.Instance.StartEndGameConversation(playerIsWinner, () =>
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

    private IEnumerator SlowGameDown(Action onComplete)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.01f);
        while (Time.timeScale < 1f)
        {
            Time.timeScale += _slowMotionSpeed * Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        onComplete.Invoke();
    }
}
