using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesSystem : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    private float prevTime;
    private float idleTime;
    public static int lives;

    void Start()
    {
        prevTime = Time.timeScale;
        lives = 3;
    }

    void Update()
    {
        // "<" incase multiple enemies die at the same time when lives is 1
        if (lives <= 0) {
            GameOver();
        }
    }

    private void PauseGame() {
        // call when there are no more lives left
        Time.timeScale = 0;
    }

    private void ResumeGame() {
        Time.timeScale = prevTime;
    }

    public void GameOver() {
        gameOverScreen.Show();
    }
}
