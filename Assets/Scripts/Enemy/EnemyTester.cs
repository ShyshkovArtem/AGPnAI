using System.Collections.Generic;
using UnityEngine;

public class EnemyTester : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public List<GameObject> enemyPrefabs;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public KeyCode nextEnemyKey = KeyCode.E;
    public KeyCode spawnKey = KeyCode.Space;

    private int currentIndex = 0;

    void Update()
    {
        if (enemyPrefabs.Count == 0) return;

        // Cycle to next enemy
        if (Input.GetKeyDown(nextEnemyKey))
        {
            currentIndex = (currentIndex + 1) % enemyPrefabs.Count;
            Debug.Log($"Selected Enemy: {enemyPrefabs[currentIndex].name}");
        }

        // Spawn selected enemy
        if (Input.GetKeyDown(spawnKey))
        {
            GameObject enemyToSpawn = enemyPrefabs[currentIndex];
            Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
            Debug.Log($"Spawned: {enemyToSpawn.name} at {spawnPoint.position}");
        }
    }
}
