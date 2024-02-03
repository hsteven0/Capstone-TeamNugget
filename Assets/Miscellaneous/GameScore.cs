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

    void Update() {
        scoreText.text = "Score: " + GameScore.score;
    }
}
