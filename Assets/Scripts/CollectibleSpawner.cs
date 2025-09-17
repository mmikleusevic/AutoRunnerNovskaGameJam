using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private Collectible collectiblePrefab;
    
    [SerializeField] private float zOffset = 2f;
    [SerializeField] private float zOffsetBetweenSets = 5f;
    [SerializeField] private int minPerSet = 3;
    [SerializeField] private int maxPerSet = 8;
    [SerializeField] private float slope = 0.5f;

    public void SpawnCollectibles(float maxZPosition)
    {
        float zPosition = 0;
        Lane[] lanes = (Lane[])Enum.GetValues(typeof(Lane));
        
        while (zPosition < maxZPosition)
        {
            zPosition += zOffsetBetweenSets;
            
            int count = Random.Range(minPerSet, maxPerSet + 1);
            bool sloped = count >= maxPerSet - 2 && Random.Range(0, 2) == 1;
            Lane lane = lanes[Random.Range(0, lanes.Length)];
            
            for (int i = 0; i < count; i++)
            {
                zPosition += zOffset;
                if (zPosition > maxZPosition) return;

                float yPosition = collectiblePrefab.transform.position.y;

                if (sloped)
                {
                    float upValue = (float)i / (count - 1);
                    
                    if (upValue <= 0.5f)
                    {
                        yPosition += upValue * slope * zOffset * count;
                    }
                    else
                    {
                        float downValue = 1 - upValue;
                        yPosition += downValue * slope * zOffset * count;
                    }
                }

                Vector3 spawnPosition = new Vector3((int)lane, yPosition, zPosition);
                Instantiate(collectiblePrefab, spawnPosition, collectiblePrefab.transform.rotation);
            }
        }
    }
}
