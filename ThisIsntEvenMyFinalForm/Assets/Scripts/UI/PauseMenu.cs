using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused { get; private set; } = false;

    [SerializeField] private GameObject _pauseScreen;

    void Start()
    {
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        _pauseScreen.SetActive(false);
    }

    public void PauseGame()
    {
        GameIsPaused = true;
        Time.timeScale = 0;
        _pauseScreen.SetActive(true);
    }
}
