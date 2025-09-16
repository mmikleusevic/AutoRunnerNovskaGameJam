using System;
using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static event Action OnPlayerCaught;
    
    [SerializeField] private PlayerHealth player;
    [SerializeField] private float laneFollowSpeed = 10f;
    [SerializeField] private float verticalFollowSpeed = 10f;
    [SerializeField] private float reduceDistanceDuration = 1f;
    
    private float followDistance;

    private void Awake()
    {
        followDistance = player.MaxHits;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTookAHit += ReduceFollowDistance;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTookAHit -= ReduceFollowDistance;
    }

    private void LateUpdate()
    {
        if (!player) return;

        Vector3 targetPos = player.transform.position;
        
        targetPos.z -= followDistance;

        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, targetPos.x, laneFollowSpeed * Time.deltaTime);
        newPos.y = Mathf.Lerp(newPos.y, targetPos.y, verticalFollowSpeed * Time.deltaTime);
        newPos.z = targetPos.z;

        transform.position = newPos;
    }

    private void ReduceFollowDistance()
    {
        StartCoroutine(ReduceFollowDistanceCoroutine());
    }

    private IEnumerator ReduceFollowDistanceCoroutine()
    {
        float startDistance = followDistance;
        float targetDistance = followDistance - 1f;
        float elapsed = 0f;

        while (elapsed < reduceDistanceDuration)
        {
            elapsed += Time.deltaTime;
            followDistance = Mathf.Lerp(startDistance, targetDistance, elapsed / reduceDistanceDuration);
            yield return null;
        }

        followDistance = targetDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            OnPlayerCaught?.Invoke();
        }
    }
}
