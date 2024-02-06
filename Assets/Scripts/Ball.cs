using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private int killCount = 0; // a count of enemies defeated with one ball

    void OnBecameInvisible() {
        Destroy(gameObject);
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
