using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject[] enemies;
    private float timer = 0.0f;
    private float spawnDelay = 2.0f;

    void Update()
    {
        timer += Time.deltaTime;

        // if time has reached spawn delay time
        if (timer >= spawnDelay) {
            Vector3 randPos = new Vector3(Random.Range(-21, 22), 16, 0);
            Instantiate(enemies[Random.Range(0, enemies.Length)], randPos, Quaternion.identity);

            // subtract spawnDelay time from total time; this is more accurate over time
            timer -= spawnDelay;
        }
    }
}
