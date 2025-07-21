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
    private List<GameObject> spawnedDrones = new List<GameObject>(); 

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
        spawnedDrones.Clear();

        for (int i = 0; i < droneSpawners.Length; i++)
        {
            GameObject newDrone = Instantiate(drone, droneSpawners[i].position, Quaternion.identity);
            spawnedDrones.Add(newDrone);

            newDrone.transform.SetParent(transform);
        }

    }

    private void SpawnLevel()
    {
        if (levelPrefabs.Length == 0)
        {
            return;
        }

        GameObject nextLevel = levelPrefabs[currentLevelIndex % levelPrefabs.Length];
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 630);

        GameObject newLevelObject = Instantiate(nextLevel, spawnPosition, Quaternion.identity);

        LevelManager newLevelManager = newLevelObject.GetComponent<LevelManager>();
        if (newLevelManager != null)
        {
            newLevelManager.levelToDestroy = this.gameObject;
            newLevelManager.levelPrefabs = this.levelPrefabs;
        }

        PlayerManager player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerManager>();
        if (player != null)
        {
            player.RefillHealth();
        }
        else
        {
        }

        currentLevelIndex++;
    }

    private void CleanupCurrentLevel()
    {
        DestroyAllDrones();

        if (levelToDestroy != null)
        {
            LevelManager destroyingLevelManager = levelToDestroy.GetComponent<LevelManager>();
            if (destroyingLevelManager != null)
            {
                destroyingLevelManager.DestroyAllDrones();
            }

            Destroy(levelToDestroy);
        }
    }

    public void DestroyAllDrones()
    {
        for (int i = spawnedDrones.Count - 1; i >= 0; i--)
        {
            if (spawnedDrones[i] != null)
            {
                Destroy(spawnedDrones[i]);
            }
        }

        spawnedDrones.Clear();

        Drone[] childDrones = GetComponentsInChildren<Drone>();
        foreach (Drone drone in childDrones)
        {
            if (drone != null && drone.gameObject != null)
            {
                Destroy(drone.gameObject);
            }
        }
    }

    public List<GameObject> GetActiveDrones()
    {
        spawnedDrones.RemoveAll(drone => drone == null);
        return new List<GameObject>(spawnedDrones);
    }

    public bool AreAllDronesDefeated()
    {
        spawnedDrones.RemoveAll(drone => drone == null);
        return spawnedDrones.Count == 0;
    }

    private void OnDestroy()
    {
        DestroyAllDrones();
    }
}