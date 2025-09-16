using System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static event Action OnPlayerCaught;
    
    [SerializeField] private Transform player;
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float laneFollowSpeed = 10f;
    [SerializeField] private float verticalFollowSpeed = 10f;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerCaught += CatchUpToPlayer;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerCaught -= CatchUpToPlayer;
    }

    private void LateUpdate()
    {
        if (!player) return;

        Vector3 targetPos = player.position;
        
        targetPos.z -= followDistance;

        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, targetPos.x, laneFollowSpeed * Time.deltaTime);
        newPos.y = Mathf.Lerp(newPos.y, targetPos.y, verticalFollowSpeed * Time.deltaTime);
        newPos.z = targetPos.z;

        transform.position = newPos;
    }

    private void CatchUpToPlayer()
    {
        followDistance--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            OnPlayerCaught?.Invoke();
        }
    }
}
