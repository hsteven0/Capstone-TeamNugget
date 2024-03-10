using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleChecker : MonoBehaviour
{
    private float timer, idleTime;
    public static bool slingshotActive = false;

    void Start()
    {
        timer = 0;
        idleTime = 65.0f;
    }

    void Update()
    {
        if (slingshotActive && gameObject.name.Contains("Sling")) return;
        
        timer += Time.deltaTime;
        if (timer >= idleTime) {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
