using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health = 1f;
    public GameObject death_effect, pointsText;
    public string displayText;

    void OnCollisionEnter2D (Collision2D collInfo) {
        if (collInfo.relativeVelocity.magnitude > health) {
            Die();
        }
    }

    void Die() {
        Score();
        Instantiate(death_effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Score() {
        GameScore.score += GameScore.points;
        GameObject point = Instantiate(pointsText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(displayText);
    }
}
