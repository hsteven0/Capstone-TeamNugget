using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesSystem : MonoBehaviour
{
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
        
    }

    private void PauseGame() {
        // call when there are no more lives left
        Time.timeScale = 0;
    }

    private void ResumeGame() {
        Time.timeScale = prevTime;
    }

    private void ExitToMainMenu() {
        // use Time.unscaleTime to calculate time when the game is paused.
        // if time.unscaled >= idleTime { ExitToMainMenu(); }
    }
}
