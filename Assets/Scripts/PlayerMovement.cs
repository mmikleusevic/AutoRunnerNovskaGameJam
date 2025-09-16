using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform groundCheckOrigin;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float laneChangeSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sphereRadius = 0.4f;
    [SerializeField] private float checkDistance = 1f;
    [SerializeField] private float foreDownMultiplier = 2f;
    [SerializeField] private float slowdownDuration = 1f;
    [SerializeField] private float recoverDuration = 2f;
    [SerializeField] private float speedSlowdownMultiplier = 10f;
    
    private Rigidbody rb;

    private Lane currentLane;
    private Lane nextLane;
    private int targetPositionX;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentLane = Lane.Middle;
        nextLane = currentLane;
        rb.linearVelocity = transform.forward * speed;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTookAHit += SlowDown;
    }
    
    private void OnDisable()
    {
        PlayerHealth.OnPlayerTookAHit -= SlowDown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && nextLane > Lane.Left)
        {
            nextLane += (int)Lane.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && nextLane < Lane.Right)
        {
            nextLane += (int)Lane.Right;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.S) && !IsGrounded())
        {
            rb.AddForce(Vector3.down * (jumpForce * foreDownMultiplier), ForceMode.Impulse);
        }
        
        if (currentLane == nextLane) return;
        
        targetPositionX = (int)nextLane;
    }

    private void FixedUpdate()
    {
        if (currentLane == nextLane) return;
        
        MoveToCorrectLane();
    }

    private void MoveToCorrectLane()
    {
        Vector3 position = transform.position;
        Vector3 targetPosition = new Vector3(targetPositionX, position.y, position.z);
        Vector3 newPosition = Vector3.MoveTowards(position, targetPosition, laneChangeSpeed * Time.fixedDeltaTime);
        
        newPosition += transform.forward * (speed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        if (Mathf.Abs(position.x - targetPositionX) > 0.01f) return;
        
        currentLane = nextLane;
        transform.position = new Vector3(targetPositionX, position.y, position.z);
    }
    
    private bool IsGrounded()
    {
        return Physics.SphereCast(
            groundCheckOrigin.position,
            sphereRadius,
            Vector3.down,
            out RaycastHit hit,
            checkDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    private void SlowDown()
    {
        StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        float originalSpeed = speed;
        float minSpeed = speed / speedSlowdownMultiplier;
        
        float elapsed = 0f;
        while (elapsed < slowdownDuration)
        {
            elapsed += Time.deltaTime;
            speed = Mathf.Lerp(originalSpeed, minSpeed, elapsed / slowdownDuration);
            yield return null;
        }

        speed = minSpeed;
        
        elapsed = 0f;
        while (elapsed < recoverDuration)
        {
            elapsed += Time.deltaTime;
            speed = Mathf.Lerp(minSpeed, originalSpeed, elapsed / recoverDuration);
            yield return null;
        }

        speed = originalSpeed;
    }
    
    // For IsGrounded Testing
    // private void OnDrawGizmosSelected()
    // {
    //     if (groundCheckOrigin == null) return;
    //
    //     bool grounded = IsGrounded();
    //
    //     Gizmos.color = grounded ? Color.green : Color.red;
    //     
    //     Vector3 start = groundCheckOrigin.position;
    //     
    //     Vector3 end = start + Vector3.down * checkDistance;
    //     
    //     Gizmos.DrawWireSphere(start, sphereRadius);
    //     Gizmos.DrawWireSphere(end, sphereRadius);
    //     Gizmos.DrawLine(start, end);
    // }
}