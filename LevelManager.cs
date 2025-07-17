using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool playerEnteredLevel;
    public bool playerExitedLevel;

    private bool spawned = false;
    public GameObject drone;
    public Transform[] droneSpawners;

    [Header("Level Prefabs")]
    public GameObject[] levelPrefabs; // Array of different level prefabs

    public GameObject levelToDestroy;

    private static int currentLevelIndex = 1; // Tracks which level to spawn next

    private void Awake()
    {
        playerEnteredLevel = false;
        spawned = false;
    }

    private void Update()
    {
        if (playerEnteredLevel && !spawned)
        {
            SpawnDrones();

            SpawnLevel();

            spawned = true;
        }

        if (playerExitedLevel)
        {
            if (levelToDestroy != null)
            {
                Destroy(levelToDestroy);
            }
        }
    }

    private void SpawnDrones()
    {
        for (int i = 0; i < droneSpawners.Length; i++)
        {
            Instantiate(drone, droneSpawners[i].position, Quaternion.identity);
        }
    }

    private void SpawnLevel()
    {
        if (levelPrefabs.Length == 0)
        {
            Debug.LogWarning("No level prefabs assigned to LevelManager!");
            return;
        }

        GameObject nextLevel = levelPrefabs[currentLevelIndex % levelPrefabs.Length];

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 630);

        GameObject destObj = Instantiate(nextLevel, pos, Quaternion.identity);

        LevelManager destLevelManager = destObj.GetComponent<LevelManager>();
        if (destLevelManager != null)
        {
            destLevelManager.levelToDestroy = this.gameObject;
            destLevelManager.levelPrefabs = this.levelPrefabs; 
        }

        PlayerManager player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerManager>();

        if (player != null)
        {
            Debug.Log("Refilling player health from LevelManager.");
            player.RefillHealth();
        }
        else
        {
            Debug.LogWarning("PlayerManager not found when trying to refill health.");
        }


        Debug.Log("Spawned level index: " + currentLevelIndex);

        currentLevelIndex++;
    }


}
