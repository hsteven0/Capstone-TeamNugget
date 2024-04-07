using System.Collections;
using TouchScript.Examples.Tap;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    // A list containing various enemies to spawn and the Slingshots on the Game Scene
    public GameObject[] enemies, slingshots;

    private int activeLayer; // a reference to the "Active" layer
    private float timer, totalTime, spawnDelay, slingshotBounds;


    void Start() {
        // time stuff
        timer = 0.0f;
        totalTime = 0.0f;
        spawnDelay = 3.5f;
        // screen stuff
        slingshotBounds = Screen.width / 3;
        // idle check stuff
        activeLayer = LayerMask.NameToLayer("Active");
    }

    void Update()
    {
        StartTimer();
        // Random number generator; favors SpawnMiddle()
        int randVal = Random.Range(1,11); 

        // if time has reached spawn delay time
        if (timer >= spawnDelay) {
            // spawn at slingshot location bounds if it is active
            // less probability to spawn to far side, more likely in activeLayer and middle
            if (slingshots[0].layer == activeLayer) {
                if (randVal % 2 == 0) {     //  50% chance   
                    SpawnMiddle(); 
                }
                else if (randVal == 1) {    //  10% chance
                    SpawnRight();  
                }
                SpawnLeft();                // 100% chance
            }
            if (slingshots[1].layer == activeLayer) {
                if (randVal < 3) {
                    SpawnLeft();            //  20% chance  
                }
                else if (randVal > 8) {
                    SpawnRight();           //  20% chance
                }
                SpawnMiddle();              // 100% chance
            }
            if (slingshots[2].layer == activeLayer) {
                if (randVal % 2 == 1) {
                    SpawnMiddle();          //  50% chance
                }
                else if (randVal == 8) {
                    SpawnLeft();            //  10% chance
                }
                SpawnRight();               // 100% chance
            }
            /* All 3 player scenarios, at MOST: 6 enemies spawn (for one outcome)
            1 : 2L, 2M, 1R
            2 : 2L, 2M, 1R
            3 : 1L, 2M, 1R
            4 : 1L, 2M, 1R
            5 : 1L, 2M, 1R
            6 : 1L, 2M, 1R
            7 : 1L, 2M, 1R
            8 : 2L, 2M, 2R
            9 : 1L, 2M, 2R
           10 : 1L, 2M, 2R 
            */

            IncreaseDifficulty(timer);
            // subtract spawnDelay time from total time; this is more accurate over time
            timer -= spawnDelay;
        }
    }

    private void IncreaseDifficulty(float time) {
        if (totalTime < 125.0f) totalTime += time; // stop counting after time reaches 125 seconds
        if (totalTime < 25.0f && spawnDelay < 5.5f) {
            if ((slingshots[0].layer == activeLayer && slingshots[2].layer == activeLayer) || (slingshots[0].layer == activeLayer && slingshots[1].layer == activeLayer) || (slingshots[1].layer == activeLayer && slingshots[2].layer == activeLayer))
                spawnDelay = 5.5f; // increases spawnDelay to offset difficulty given 3 players at the start
        }
        // things can get serious pretty quickly; be mindful of spawnDelay values
        else if (totalTime > 25.0f && totalTime < 45.0f) spawnDelay = 5.0f;
        else if (totalTime > 45.0f && totalTime < 65.0f) spawnDelay = 4.5f;
        else if (totalTime > 65.0f && totalTime < 80.0f) spawnDelay = 4.0f;
        else if (totalTime > 80.0f && totalTime < 120.0f) spawnDelay = 3.0f;
    }

    private void StartTimer() {
        // only increment timer here
        if (timer > 0) {
            timer += Time.deltaTime;
            return;
        }
        foreach (var slingshot in slingshots) {
            // if A slingshot's layer is the active layer, start the timer and break the loop
            if (slingshot.layer == activeLayer) {
                timer += Time.deltaTime;
                break;
            }
        }
    }

    private void SpawnLeft() {
        // will need to account for sprite size, so half the enemy sprite does not
        // spawn outside of the screen (subtract from FIRST param of random range func)
        Vector3 randPos = Camera.main.ScreenToWorldPoint(
                new Vector3(
        /* x */     Random.Range(0+128, slingshotBounds), 
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
        float rightBounds = (2 * Screen.width) / 3;
        Vector3 randPos = Camera.main.ScreenToWorldPoint(
                new Vector3(
        /* x */     Random.Range(rightBounds, rightBounds + slingshotBounds - 128), 
        /* y */     Screen.height, 
        /* z */     10));

            Instantiate(enemies[Random.Range(0, enemies.Length)], randPos, Quaternion.identity);
    }
}