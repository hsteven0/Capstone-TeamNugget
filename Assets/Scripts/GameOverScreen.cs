using UnityEngine;
using UnityEngine.SceneManagement;
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
        SceneManager.LoadScene("GameScene");
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
