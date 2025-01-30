using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMovement pm;
    public GameObject currentChunk;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    public GameObject lastChunk;
    public float MaxOpDist; //Must be > LnW of the tilemap
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;

    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }


    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }


        if (pm.moveDir.x > 0 && pm.moveDir.y == 0)   //right
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)   //left
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0)   //up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0)   //down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y > 0)   //right up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("TopRight").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("TopRight").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y < 0)   //right down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("DownRight").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("DownRight").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y > 0)   //left up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("TopLeft").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("TopLeft").position;
                SpawnChunk();
            }
        }
        if (pm.moveDir.x < 0 && pm.moveDir.y < 0)   //left down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("DownLeft").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("DownLeft").position;
                SpawnChunk();
            }
        }
    }


    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        lastChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(lastChunk);
    }


    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown < 0)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > MaxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
