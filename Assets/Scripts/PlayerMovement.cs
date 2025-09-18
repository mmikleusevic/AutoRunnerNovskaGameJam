using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private float maxSpeed;
    [SerializeField] private float laneChangeSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sphereRadius = 0.4f;
    [SerializeField] private float checkDistance = 1f;
    [SerializeField] private float foreDownMultiplier = 2f;
    [SerializeField] private float slowdownDuration = 1f;
    [SerializeField] private float recoverDuration = 2f;
    [SerializeField] private float speedSlowdownMultiplier = 10f;
    [SerializeField] private float slideTime = 0.5f;
    
    [SerializeField] private AudioClip[] hitSounds;
    
    public bool IsSliding { get; private set; }

    private Rigidbody rb;
    private Animator animator;
    private Coroutine slideCoroutine;
    private Coroutine slowDownCoroutine;
    
    private Lane currentLane;
    private Lane nextLane;
    private float targetPositionX;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentLane = Lane.Middle;
        nextLane = currentLane;
        speed = maxSpeed;
        rb.linearVelocity = transform.forward * speed;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTookAHit += SlowDown;
        FinishLine.OnFinish += OnFinish;
    }
    
    private void OnDisable()
    {
        PlayerHealth.OnPlayerTookAHit -= SlowDown;
        FinishLine.OnFinish -= OnFinish;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && nextLane == Lane.Middle)
        {
            StopSlowDown();
            nextLane = Lane.Left;
        }
        else if (Input.GetKeyDown(KeyCode.A) && nextLane == Lane.Right)
        {
            StopSlowDown();
            nextLane = Lane.Middle;
        }
        if (Input.GetKeyDown(KeyCode.D) && nextLane == Lane.Middle)
        {
            StopSlowDown();
            nextLane = Lane.Right;
        }
        else if (Input.GetKeyDown(KeyCode.D) && nextLane == Lane.Left)
        {
            StopSlowDown();
            nextLane = Lane.Middle;
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            StopSliding();
            StopSlowDown();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.S) && !IsGrounded())
        {
            rb.AddForce(Vector3.down * (jumpForce * foreDownMultiplier), ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.S) && IsGrounded())
        {
            StopSlowDown();
            slideCoroutine = StartCoroutine(Slide());
        }
        
        animator.SetBool(GameEvents.IsGrounded, IsGrounded());
        
        if (currentLane == nextLane) return;
        
        targetPositionX = LaneData.Lanes[nextLane];
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

        Debug.Log(rb.linearVelocity);
        rb.MovePosition(newPosition);

        if (Mathf.Abs(position.x - targetPositionX) > 0.1f) return;
        
        currentLane = nextLane;
        transform.position = new Vector3(targetPositionX, position.y, position.z);
    }
    
    private bool IsGrounded()
    {
        return Physics.SphereCast(
            transform.position + Vector3.up,
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
        slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
    }

    public void PlayHitSound()
    {
        int index = Random.Range(0, hitSounds.Length);
        SoundManager.Instance.PlayOneShot(hitSounds[index]);
    }

    private void StopSlowDown()
    {
        if (slowDownCoroutine != null) StopCoroutine(slowDownCoroutine);
        animator.speed = 1;
        speed = maxSpeed;
        CalculateSpeed();
    }

    private IEnumerator SlowDownCoroutine()
    {
        float originalSpeed = speed;
        float minSpeed = speed / speedSlowdownMultiplier;
        
        float originalAnimSpeed = animator.speed;
        float minAnimSpeed = originalAnimSpeed / speedSlowdownMultiplier;
        
        float elapsed = 0f;
        while (elapsed < slowdownDuration)
        {
            elapsed += Time.deltaTime;
            float duration = elapsed / slowdownDuration;
            
            speed = Mathf.Lerp(originalSpeed, minSpeed, duration);
            animator.speed = Mathf.Lerp(originalAnimSpeed, minAnimSpeed, duration);
            
            CalculateSpeed();
            yield return null;
        }

        speed = minSpeed;
        animator.speed = minAnimSpeed;
        CalculateSpeed();
        
        elapsed = 0f;
        while (elapsed < recoverDuration)
        {
            elapsed += Time.deltaTime;
            float duration = elapsed / recoverDuration;

            speed = Mathf.Lerp(minSpeed, originalSpeed, duration);
            animator.speed = Mathf.Lerp(minAnimSpeed, originalAnimSpeed, duration);
            
            CalculateSpeed();
            yield return null;
        }

        speed = originalSpeed;
        animator.speed = originalAnimSpeed;
        CalculateSpeed();
    }

    private void CalculateSpeed()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    private void StopSliding()
    {
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        SetIsSliding(false);
    }

    private IEnumerator Slide()
    {
        SetIsSliding(true);
        
        yield return new WaitForSeconds(slideTime);
        
        SetIsSliding(false);
    }

    private void SetIsSliding(bool value)
    {
        IsSliding = value;
        animator.SetBool(GameEvents.IsSliding, IsSliding);
    }

    private void OnFinish()
    {
        animator.SetTrigger(GameEvents.WIN);
        rb.linearVelocity = Vector3.zero;
        enabled = false;
    }

    // For IsGrounded Testing Gizmos
    // private void OnDrawGizmosSelected()
    // {
    //     bool grounded = IsGrounded();
    //
    //     Gizmos.color = grounded ? Color.green : Color.red;
    //     Vector3 start = transform.position + Vector3.up;
    //     Debug.Log(start);
    //     
    //     Vector3 end = start + Vector3.down * checkDistance;
    //     
    //     Gizmos.DrawWireSphere(start, sphereRadius);
    //     Gizmos.DrawWireSphere(end, sphereRadius);
    //     Gizmos.DrawLine(start, end);
    // }
}