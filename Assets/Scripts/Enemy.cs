using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject deathEffect, pointsText;

    void OnCollisionEnter2D(Collision2D collInfo) {
        // less laggy option to ignore enemy colliding with another enemy
        // need to optimize
        if(collInfo.collider.gameObject.layer == 7) return;
        // reward or punish player(s)
        if (collInfo.collider.tag == "SlingItem") {
            Score();
        }
        else if (collInfo.collider.tag == "Ground") {
            LivesSystem.lives--;
        }
        Die();
    }

    void Die() {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        Destroy(gameObject);
    }

    private void Score() {
        GameScore.score += GameScore.points;
        GameObject point = Instantiate(pointsText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(GameScore.points.ToString());
    }
}
