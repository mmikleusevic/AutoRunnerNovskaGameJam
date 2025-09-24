using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private FinishLine finishBlockPrefab;
    [SerializeField] private CollectibleSpawner collectibleSpawner;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    
    [SerializeField] private float zOffset;
    [SerializeField] private int maxBlocks;
    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Start()
    {
        SpawnBlocks();
    }

    private void SpawnBlocks()
    {
        int i;

        if (blockPrefabs.Length == 0)
        {
            Time.timeScale = 1;
            return;
        }
        
        GameObject blockPrefabZero = blockPrefabs[0];
        Vector3 spawnPositionStarting = new Vector3(blockPrefabZero.transform.position.x,
            blockPrefabZero.transform.position.y , blockPrefabZero.transform.position.y - zOffset);
        Instantiate(blockPrefabZero, spawnPositionStarting, blockPrefabZero.transform.rotation);
        
        for (i = 0; i < maxBlocks; i++)
        {
            int randomIndex = Random.Range(0, blockPrefabs.Length);   
            GameObject blockPrefab = blockPrefabs[randomIndex];
            Vector3 spawnPosition = new Vector3(blockPrefab.transform.position.x,
                blockPrefab.transform.position.y , blockPrefab.transform.position.y + zOffset * i);
            Instantiate(blockPrefab, spawnPosition, blockPrefab.transform.rotation);
        }
        
        Vector3 finishSpawnPosition = new Vector3(finishBlockPrefab.transform.position.x,
            finishBlockPrefab.transform.position.z, finishBlockPrefab.transform.position.y + zOffset * i);
        FinishLine finish = Instantiate(finishBlockPrefab, finishSpawnPosition, finishBlockPrefab.transform.rotation);
        
        float maxZPosition = finish.transform.position.z - zOffset - zOffset / 2;
        
        collectibleSpawner.SpawnCollectibles(maxZPosition);
        obstacleSpawner.SpawnObstacles(maxZPosition);
        
        Time.timeScale = 1;
    }
}
