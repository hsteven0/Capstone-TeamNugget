using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleChecker : MonoBehaviour
{
    private float timer, idleTime;

    void Start()
    {
        timer = 0;
        idleTime = 5.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= idleTime) {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
