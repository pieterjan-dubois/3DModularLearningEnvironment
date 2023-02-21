using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject player;
    public Transform[] spawnPoints;

    private bool hasSpawned = false;
    
    void Start()
    {
        if (!hasSpawned)
        {
            SpawnPlayer();
            hasSpawned = true;
            Debug.Log("Spawned Player is true");
        }
    }

    private void SpawnPlayer()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Debug.Log("Spawn Point Index: " + spawnPointIndex);
        Transform spawnPoint = spawnPoints[spawnPointIndex];
        Debug.Log("Spawned at " + spawnPoint.name);
        player.transform.position = spawnPoint.position;
    }
}
