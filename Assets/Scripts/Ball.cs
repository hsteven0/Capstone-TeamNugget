using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class will be used as calculating point multipliers upon contact with enemies.
*/
public class Ball : MonoBehaviour
{
    private int killCount = 0; // a count of enemies defeated with one ball
    private int oldAmount;

    void Start() {
        oldAmount = GameScore.points;
    }

    void OnDestroy() {
        if (gameObject.tag != "SlingItem") return;
        killCount = 0;
        GameScore.points = oldAmount;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Enemies") return;
        UpdatePoints();
        killCount++;
    }

    private void UpdatePoints() {
        // reward player with more points, based on number of enemies defeated with a single ball
        GameScore.points = oldAmount * (int) Math.Pow(2, killCount);
    }
}
