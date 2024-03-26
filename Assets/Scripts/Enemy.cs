using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject deathEffect;

    void Update() {
        transform.Rotate(0f, 0f, 125f * Time.deltaTime, Space.Self);
    }

    void OnCollisionEnter2D(Collision2D collInfo) {
        if (collInfo.collider.CompareTag("Ground")) {
            SoundManager.soundManager.PlayEffect("AsteroidImpact");
            LivesSystem.lives--;
        } else {
            SoundManager.soundManager.PlayEffect("AsteroidDeath");
        }
        Die();
    }

    void Die() {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
