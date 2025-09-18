using System;
using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static event Action OnPlayerCaught;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerHealth player;
    [SerializeField] private  AudioClip audioClip;
    
    [SerializeField] private float laneFollowSpeed = 10f;
    [SerializeField] private float verticalFollowSpeed = 10f;
    [SerializeField] private float reduceDistanceDuration = 1f;
    [SerializeField] private float distanceOffset = 0.5f;
    
    private float followDistance;

    private void Awake()
    {
        followDistance = player.MaxHits + distanceOffset;
        rb.position = new Vector3(transform.position.x, transform.position.y, -followDistance);
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTookAHit += ReduceFollowDistance;
        FinishLine.OnFinish += OnFinish;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTookAHit -= ReduceFollowDistance;
        FinishLine.OnFinish -= OnFinish;
    }

    private void FixedUpdate()
    {
        if (!player) return;

        Vector3 targetPos = player.transform.position;
        targetPos.z -= followDistance;

        Vector3 currentPos = rb.position;
        Vector3 newPos = currentPos;

        newPos.x = Mathf.Lerp(currentPos.x, targetPos.x, laneFollowSpeed * Time.fixedDeltaTime);
        newPos.y = Mathf.Lerp(currentPos.y, targetPos.y, verticalFollowSpeed * Time.fixedDeltaTime);
        newPos.z = targetPos.z;

        rb.MovePosition(newPos);
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

        if (targetDistance < 0) targetDistance = 0;
        
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
            SoundManager.Instance.PlayOneShot(audioClip);
            OnPlayerCaught?.Invoke();
        }
    }

    private void OnFinish()
    {
        enabled = false;
    }
}
