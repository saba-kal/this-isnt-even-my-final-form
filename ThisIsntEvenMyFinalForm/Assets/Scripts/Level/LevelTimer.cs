using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    private bool _timerEnabled = false;
    private float _elapsedTime = 0f;

    void Update()
    {
        if (_timerEnabled &&
            !DialogueManager.Instance.ConversationInProgress())
        {
            _elapsedTime += Time.deltaTime;
            _timerText.text = TimeSpan.FromSeconds(_elapsedTime).ToString("mm':'ss'.'ff");
        }
    }

    public void StartTimer()
    {
        _timerEnabled = true;
    }

    public void StopTimer()
    {
        _timerEnabled = false;
    }

    public float GetElapsedTimeInSeconds()
    {
        return _elapsedTime;
    }
}
