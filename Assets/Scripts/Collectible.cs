using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            GameManager.Instance.IncreaseCollectibleScore();
            Destroy(gameObject);
        }
    }
}
