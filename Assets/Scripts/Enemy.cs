using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject deathEffect;

    void OnCollisionEnter2D(Collision2D collInfo) {
        if (collInfo.collider.tag == "Ground") {
            LivesSystem.lives--;
        }
        Die();
    }

    void Die() {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
