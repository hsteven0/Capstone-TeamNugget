using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void LoadScene(String sceneName) {
        SoundManager.soundManager.PlayEffect("ButtonClick");
        SceneManager.LoadScene(sceneName);
    }
}
