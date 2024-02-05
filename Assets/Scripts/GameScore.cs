using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    public static int points = 100;
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    void Start() {
        scoreText.color = Color.black ; // change text color (dependent on background color)
    }
    void Update() {
        scoreText.text = "<b>Score: " + GameScore.score;
    }
}
