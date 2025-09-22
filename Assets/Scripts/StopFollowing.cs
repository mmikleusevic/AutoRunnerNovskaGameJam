using System;
using UnityEngine;

public class StopFollowing : MonoBehaviour
{
    public static event Action OnStopFollowing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            OnStopFollowing?.Invoke();
        }
    }
}
