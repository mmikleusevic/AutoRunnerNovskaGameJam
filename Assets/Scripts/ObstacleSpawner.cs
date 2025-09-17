using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle[] obstaclePrefabs;

    [SerializeField] private float startOffsetZ = 5f;
    [SerializeField] private int maxObstacles = 15;
    
    public void SpawnObstacles(float maxZPosition)
    {
        if (maxObstacles <= 0) return;
        
        float spacing = maxZPosition / maxObstacles;

        Lane[] lanes = (Lane[])Enum.GetValues(typeof(Lane));

        for (int i = 0; i < maxObstacles; i++)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            Obstacle obstaclePrefab = obstaclePrefabs[randomIndex];    
            
            float zPosition = startOffsetZ + spacing * i;
            
            Lane lane = lanes[Random.Range(0, lanes.Length)];

            Vector3 spawnPos = new Vector3((int)lane, obstaclePrefab.transform.position.y, zPosition);
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }
    }
}
