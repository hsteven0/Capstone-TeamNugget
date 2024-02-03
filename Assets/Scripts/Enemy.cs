using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 1f;
    public GameObject death_effect;
    void OnCollisionEnter2D (Collision2D collInfo) {
        if ( collInfo.relativeVelocity.magnitude > health ) {
            Die() ; 
        }
    }

    void Die() {
        GameScore.score += GameScore.points;
        Instantiate(death_effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
