using System;
using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerHealth player;
    [SerializeField] private  AudioClip audioClip;
    
    [SerializeField] private float laneFollowSpeed = 10f;
    [SerializeField] private float verticalFollowSpeed = 10f;
    [SerializeField] private float reduceDistanceDuration = 1f;
    [SerializeField] private float distanceOffset = 0.5f;
    
    private Animator animator;
    
    private float followDistance;
    private bool hasCaught;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        followDistance = player.MaxHits + distanceOffset;
        rb.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - followDistance);
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTookAHit += ReduceFollowDistance;
        PlayerMovement.OnPlayerCaught += PlayerCaught;
        StopFollowing.OnStopFollowing += OnFinish;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTookAHit -= ReduceFollowDistance;
        PlayerMovement.OnPlayerCaught -= PlayerCaught;
        StopFollowing.OnStopFollowing -= OnFinish;
    }

    private void FixedUpdate()
    {
        if (hasCaught)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            Follow(targetPosition);            
            return;
        }
        
        if (!player) return;

        Follow(player.transform.position);
    }

    private void Follow(Vector3 targetPosition)
    {
        targetPosition.z -= followDistance;

        Vector3 currentPosition = rb.position;
        Vector3 newPosition = currentPosition;

        newPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, laneFollowSpeed * Time.fixedDeltaTime);
        newPosition.y = Mathf.Lerp(currentPosition.y, targetPosition.y, verticalFollowSpeed * Time.fixedDeltaTime);
        newPosition.z = targetPosition.z;

        rb.MovePosition(newPosition);
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

    public void Ground()
    {
        hasCaught = true;
    }

    private void PlayerCaught()
    {
        animator.SetTrigger(GameEvents.Pull);
        SoundManager.Instance.PlayOneShot(audioClip);
    }

    private void OnFinish()
    {
        enabled = false;
    }
}
