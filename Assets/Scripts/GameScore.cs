using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    public static int points;
    public static int score;
    public TextMeshProUGUI scoreText;

    void Start() {
        // scoreText.color = Color.black ; // change text color (dependent on background color)
        score = 0;
        points = 100;
    }

    void Update() {
        scoreText.text = "Score: " + GameScore.score;
    }
}
