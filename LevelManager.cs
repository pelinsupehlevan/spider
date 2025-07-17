using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Management")]
    public bool playerEnteredLevel;
    public bool playerExitedLevel;

    [Header("Drone Settings")]
    public GameObject drone;
    public Transform[] droneSpawners;
    private List<GameObject> spawnedDrones = new List<GameObject>(); // Track spawned drones

    [Header("Level Prefabs")]
    public GameObject[] levelPrefabs;
    public GameObject levelToDestroy;

    private bool spawned = false;
    private static int currentLevelIndex = 1;

    private void Awake()
    {
        playerEnteredLevel = false;
        playerExitedLevel = false;
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
            CleanupCurrentLevel();
        }
    }

    private void SpawnDrones()
    {
        // Clear the list of previously spawned drones
        spawnedDrones.Clear();

        // Spawn new drones and track them
        for (int i = 0; i < droneSpawners.Length; i++)
        {
            GameObject newDrone = Instantiate(drone, droneSpawners[i].position, Quaternion.identity);
            spawnedDrones.Add(newDrone);

            // Optional: Set the drone's parent to this level for better organization
            newDrone.transform.SetParent(transform);
        }

        Debug.Log($"Spawned {spawnedDrones.Count} drones for level {currentLevelIndex - 1}");
    }

    private void SpawnLevel()
    {
        if (levelPrefabs.Length == 0)
        {
            Debug.LogWarning("No level prefabs assigned to LevelManager!");
            return;
        }

        GameObject nextLevel = levelPrefabs[currentLevelIndex % levelPrefabs.Length];
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 630);

        GameObject newLevelObject = Instantiate(nextLevel, spawnPosition, Quaternion.identity);

        // Set up the new level's manager
        LevelManager newLevelManager = newLevelObject.GetComponent<LevelManager>();
        if (newLevelManager != null)
        {
            newLevelManager.levelToDestroy = this.gameObject;
            newLevelManager.levelPrefabs = this.levelPrefabs;
        }

        // Refill player health
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

        Debug.Log($"Spawned level index: {currentLevelIndex}");
        currentLevelIndex++;
    }

    private void CleanupCurrentLevel()
    {
        // Destroy all drones from this level
        DestroyAllDrones();

        // Destroy the level that should be destroyed
        if (levelToDestroy != null)
        {
            // Also cleanup any drones that might be children of the level being destroyed
            LevelManager destroyingLevelManager = levelToDestroy.GetComponent<LevelManager>();
            if (destroyingLevelManager != null)
            {
                destroyingLevelManager.DestroyAllDrones();
            }

            Debug.Log($"Destroying level: {levelToDestroy.name}");
            Destroy(levelToDestroy);
        }
    }

    public void DestroyAllDrones()
    {
        // Destroy all tracked drones
        for (int i = spawnedDrones.Count - 1; i >= 0; i--)
        {
            if (spawnedDrones[i] != null)
            {
                Debug.Log($"Destroying drone: {spawnedDrones[i].name}");
                Destroy(spawnedDrones[i]);
            }
        }

        spawnedDrones.Clear();

        // Fallback: Find and destroy any remaining drones that might be children of this level
        Drone[] childDrones = GetComponentsInChildren<Drone>();
        foreach (Drone drone in childDrones)
        {
            if (drone != null && drone.gameObject != null)
            {
                Debug.Log($"Destroying child drone: {drone.gameObject.name}");
                Destroy(drone.gameObject);
            }
        }
    }

    // Method to get all drones in this level (useful for other systems)
    public List<GameObject> GetActiveDrones()
    {
        // Remove null references and return active drones
        spawnedDrones.RemoveAll(drone => drone == null);
        return new List<GameObject>(spawnedDrones);
    }

    // Method to check if all drones are defeated
    public bool AreAllDronesDefeated()
    {
        spawnedDrones.RemoveAll(drone => drone == null);
        return spawnedDrones.Count == 0;
    }

    private void OnDestroy()
    {
        // Cleanup when this level manager is destroyed
        DestroyAllDrones();
    }
}