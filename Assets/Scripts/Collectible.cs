using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(180 * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            GameManager.Instance.IncreaseCollectibleScore();
            Destroy(gameObject);
        }
    }
}
