using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesSystem : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public static int lives;
    public Image[] hearts;
    public Sprite fullHeart, emptyHeart;
    private bool updateHearts;

    void Start()
    {
        updateHearts = true;
        lives = 3;
    }

    void Update()
    {
        if (updateHearts)
        {
            foreach (Image i in hearts)
            {
                i.sprite = emptyHeart;
            }
            for (int i = 0; i < lives; i++)
            {
                hearts[i].sprite = fullHeart;
            }
        }

        if (lives <= 0)
        {
            // "<" incase multiple enemies die at the same time when lives is 1
            GameOver();
            updateHearts = false; 
        }
    }

    public void GameOver() {
        gameOverScreen.Show();
    }
}
