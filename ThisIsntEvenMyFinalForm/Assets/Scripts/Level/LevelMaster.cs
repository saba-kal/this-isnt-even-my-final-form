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
    [SerializeField] private EndGameView _endGameView;
    [SerializeField] private TutorialStage _tutorialStage;
    [SerializeField] private LevelTimer _levelTimer;

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
                _levelTimer.StartTimer();
                _stageManager.GoToNextStage(playerPowerLevelManager.GetPowerLevel());
            });
        });
    }

    private void OnCharacterHealthLost(CharacterHealth characterHealth)
    {
        var playerIsWinner = characterHealth.gameObject.layer == (int)CollisionLayer.Enemy;
        var powerLevelManager = characterHealth.GetComponent<PowerLevelManager>();
        var powerUpEffect = characterHealth.GetComponent<CharacterPowerUpEffect>();

        StartCoroutine(SlowGameDown(() =>
        {
            SetGameplayDisabled(true);
            OnStageStart?.Invoke();
            DialogueManager.Instance.StartConversation(
                playerIsWinner,
                () => //On character power up start.
                {
                    powerUpEffect?.EnablePoweringUpEffect();
                },
                () => //On character power up end.
                {
                    powerLevelManager.PowerUp();
                    powerUpEffect?.EnablePoweredUpEffect();
                },
                () => //On conversation complete.
                {
                    _stageManager.GoToNextStage(powerLevelManager.GetPowerLevel());
                    SetGameplayDisabled(false);
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
            _levelTimer.StopTimer();
            _endGameView.ShowEndGame(new EngGameResult
            {
                PlayerWon = playerIsWinner,
                ElapsedTimeSeconds = _levelTimer.GetElapsedTimeInSeconds(),
                PlayerPowerLevel = _playerController.GetPowerLevel(),
                EnemyPowerLevel = _aiController.GetPowerLevel()
            });
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
        SoundManager.Instance?.SetAllPitch(Time.timeScale);
        yield return new WaitForSeconds(0.01f);
        while (Time.timeScale < 1f)
        {
            Time.timeScale += _slowMotionSpeed * Time.deltaTime;
            SoundManager.Instance?.SetAllPitch(Time.timeScale);
            yield return null;
        }
        Time.timeScale = 1f;
        SoundManager.Instance?.SetAllPitch(Time.timeScale);
        onComplete.Invoke();
    }
}
