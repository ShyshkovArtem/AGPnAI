using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;    //A list of groups of enemies to spawn in the wave
        public int waveQuota;   //Amount of enemy in the wave
        public float spawnInterval;
        public int spawnCount;  //Amount of enemies already spawned
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;  //Amount of enemies to spawn in the wave
        public int spawnCount;  //Amount if enemies of THIS TYPE already spawned in the wave
        public GameObject enemyPrefab;
    }


    public List<Wave> waves;   //The list of all the waves in the game
    public int currentWaveCount;    //The index of the current wave 

    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed;   //The max of enemies allowed on the map
    public bool maxEnemiesReached = false;
    public float waveInterval;  //The interval between waves 

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; //A list to store all relative spawn points of enemies;

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuote();
    }

    
    void Update()
    {   
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)  //Check if the wave had ended
        {
            StartCoroutine(BeginNextWave());
        }


        spawnTimer += Time.deltaTime;
        //Checks if it is time to spawn new enemies
        if (spawnTimer > waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }


    IEnumerator BeginNextWave()
    {

        yield return new WaitForSeconds(waveInterval);

        if(currentWaveCount <  waves.Count -1)
        {
            currentWaveCount++;
            CalculateWaveQuote();
        }
    }


    void CalculateWaveQuote()
    {
        int currentWaveQuota = 0;

        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }


    void SpawnEnemies()
    {
        //Check if the minimum amount of enemies in the wwave has been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //Spawn each type of enemies until the quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //Check if the minimum number of enemies of this type have been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {   
                    //Limit the amount of enemies spawned at once
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        if(enemiesAlive <  maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }


    public void OnEnemyKill()
    {
        enemiesAlive--;
    }
}
