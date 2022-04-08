using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class EndGameView : MonoBehaviour
{

    [SerializeField] private int _scoreMultiplier = 100;
    [SerializeField] private int _basePoints;
    [SerializeField] private int _victoryPoints;
    [SerializeField] private int _defeatPoints;
    [SerializeField] private int _pointsPerSecond;
    [SerializeField] private int _pointsPerPlayerPowerLevel;
    [SerializeField] private int _pointsPerEnemyPowerLevel;

    [SerializeField] private GameObject _endGameScreen;
    [SerializeField] private GameObject _victoryTitle;
    [SerializeField] private GameObject _defeatTitle;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _playerPowerLevelText;
    [SerializeField] private TextMeshProUGUI _enemyPowerLevelText;

    private void Awake()
    {
        _endGameScreen.SetActive(false);
    }

    public void ShowEndGame(EngGameResult result)
    {
        _endGameScreen.SetActive(true);

        _victoryTitle.SetActive(result.PlayerWon);
        _defeatTitle.SetActive(!result.PlayerWon);

        _scoreText.text = CalculateScore(result).ToString();
        _timeText.text = TimeSpan.FromSeconds(result.ElapsedTimeSeconds).ToString("mm':'ss'.'ff");
        _playerPowerLevelText.text = result.PlayerPowerLevel.ToString();
        _enemyPowerLevelText.text = result.EnemyPowerLevel.ToString();
    }

    private int CalculateScore(EngGameResult result)
    {
        return _scoreMultiplier * (_basePoints +
            (result.PlayerWon ? _victoryPoints : _defeatPoints) +
            ((int)result.ElapsedTimeSeconds * _pointsPerSecond) +
            (result.PlayerPowerLevel * _pointsPerPlayerPowerLevel) +
            (result.EnemyPowerLevel * _pointsPerEnemyPowerLevel));
    }
}

public class EngGameResult
{
    public bool PlayerWon { get; set; }
    public float ElapsedTimeSeconds { get; set; }
    public int PlayerPowerLevel { get; set; }
    public int EnemyPowerLevel { get; set; }
}
