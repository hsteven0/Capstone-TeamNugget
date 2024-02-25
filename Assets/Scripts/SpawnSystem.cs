using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    // A list containing various enemies to spawn and the Slingshots on the Game Scene
    public GameObject[] enemies, slingshots;
    private int activeLayer; // a reference to the "Active" layer
    private float timer, spawnDelay, slingshotBounds;

    void Start() {
        // time stuff
        timer = 0.0f;
        spawnDelay = 2.0f;
        // screen stuff
        slingshotBounds = Screen.width / 3;
        // idle check stuff
        activeLayer = LayerMask.NameToLayer("Active");
    }

    void Update()
    {
        timer += Time.deltaTime;
        // if time has reached spawn delay time
        if (timer >= spawnDelay) {
            // spawn at slingshot location bounds if it is active
            if (slingshots[0].layer == activeLayer) {
                SpawnLeft();
            }
            if (slingshots[1].layer == activeLayer) {
                SpawnMiddle();
            }
            if (slingshots[2].layer == activeLayer) {
                SpawnRight();
            }

            // subtract spawnDelay time from total time; this is more accurate over time
            timer -= spawnDelay;
        }
    }

    private void SpawnLeft() {
        // will need to account for sprite size, so half the enemy sprite does not
        // spawn outside of the screen (subtract from FIRST param of random range func)
        Vector3 randPos = Camera.main.ScreenToWorldPoint(
                new Vector3(
        /* x */     Random.Range(0, slingshotBounds), 
        /* y */     Screen.height, 
        /* z */     10)); // z must be greater than 0 to render sprite on screen using ScreenToWorldPoint()

            Instantiate(enemies[Random.Range(0, enemies.Length)], randPos, Quaternion.identity);
    }

    private void SpawnMiddle() {
        float middleBounds = Screen.width / 3;
        Vector3 randPos = Camera.main.ScreenToWorldPoint(
                new Vector3(
        /* x */     Random.Range(middleBounds, middleBounds + slingshotBounds), 
        /* y */     Screen.height, 
        /* z */     10));

            Instantiate(enemies[Random.Range(0, enemies.Length)], randPos, Quaternion.identity);
    }
    
    private void SpawnRight() {
        // will need to account for sprite size, so half the enemy sprite does not
        // spawn outside of the screen (subtract from SECOND param of random range func)
        float rightBounds = 2 * Screen.width / 3;
        Vector3 randPos = Camera.main.ScreenToWorldPoint(
                new Vector3(
        /* x */     Random.Range(rightBounds, rightBounds + slingshotBounds), 
        /* y */     Screen.height, 
        /* z */      10));

            Instantiate(enemies[Random.Range(0, enemies.Length)], randPos, Quaternion.identity);
    }
}