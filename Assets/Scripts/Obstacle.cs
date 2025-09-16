using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement) && playerMovement.IsSliding) return;
        if (!other.TryGetComponent(out PlayerHealth playerHealth)) return;
        
        playerHealth.TakeAHit();
        Destroy(gameObject);
    }
}
