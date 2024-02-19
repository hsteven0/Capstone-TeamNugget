using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class will be used as calculating point multipliers upon contact with enemies.
*/
public class Ball : MonoBehaviour
{
    private int killCount = 0; // a count of enemies defeated with one ball

    void OnBecameInvisible() {
        killCount = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        killCount++;
        UpdatePoints();
        // Debug.Log(killCount);
    }

    private void UpdatePoints() {
        // reward player with more points, based on number of enemies defeated with a single ball
    }
}
