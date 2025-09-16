using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private FinishLine finishBlockPrefab;
    
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
        
        for (i = 0; i < maxBlocks; i++)
        {
            int randomIndex = Random.Range(0, blockPrefabs.Length);   
            GameObject blockPrefab = blockPrefabs[randomIndex];
            Vector3 spawnPosition = new Vector3(blockPrefab.transform.position.x,
                blockPrefab.transform.position.y + zOffset * i, blockPrefab.transform.position.z);
            Instantiate(blockPrefab, spawnPosition, blockPrefab.transform.rotation);
        }

        i++;
        
        Vector3 finishSpawnPosition = new Vector3(finishBlockPrefab.transform.position.x,
            finishBlockPrefab.transform.position.y + zOffset * i, finishBlockPrefab.transform.position.z);
        Instantiate(finishBlockPrefab, finishSpawnPosition, finishBlockPrefab.transform.rotation);
        
        Time.timeScale = 1;
    }
}
