using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void Show() {
        if (LivesSystem.lives <= 0 && !gameObject.activeInHierarchy) {
            gameObject.SetActive(true);
            scoreText.text = "Score: " + GameScore.score;
        }
    }

    public void RestartGame() {
        SoundManager.soundManager.PlayEffect("ButtonClick");
        CircleTransition.circleTransition.ClosingScreen("GameScene");
    }

    public void ExitToMainMenu() {
        SoundManager.soundManager.PlayEffect("ButtonClick");
        CircleTransition.circleTransition.ClosingScreen("MainMenuScene");
    }
}
