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
    
    [SerializeField] private PhoneButtons phoneButtons;
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
    private bool moveLeftPressed;
    private bool moveRightPressed;
    private bool jumpPressed;
    private bool downPressed;
    
    public void OnLeft() => moveLeftPressed = true;
    public void OnRight() => moveRightPressed = true;
    public void OnJump() => jumpPressed = true;
    public void OnDown() => downPressed = true;
    private bool IsChangingLane => currentLane != nextLane;

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
        if ((Input.GetKeyDown(KeyCode.A) || moveLeftPressed) && currentLane == Lane.Middle)
        {
            nextLane = Lane.Left;
        }
        else if ((Input.GetKeyDown(KeyCode.A) || moveLeftPressed) && currentLane == Lane.Right)
        {
            nextLane = Lane.Middle;
        }
        if ((Input.GetKeyDown(KeyCode.D) || moveRightPressed) && currentLane == Lane.Middle)
        {
            nextLane = Lane.Right;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || moveRightPressed) && currentLane == Lane.Left)
        {
            nextLane = Lane.Middle;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || jumpPressed) && IsGrounded())
        {
            StopSliding();
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        if ((Input.GetKeyDown(KeyCode.S) || downPressed))
        {
            if (IsGrounded())
            {
                slideCoroutine = StartCoroutine(Slide());
            }
            else
            {
                rb.AddForce(-transform.up * (jumpForce * foreDownMultiplier), ForceMode.Impulse);
            }
        }
        
        animator.SetBool(GameEvents.IsGrounded, IsGrounded());

        moveRightPressed = false;
        moveLeftPressed = false;
        jumpPressed = false;
        downPressed = false;

        if (!IsChangingLane)
        {
            CalculateSpeed();
            return;
        }
        
        targetPositionX = LaneData.Lanes[nextLane];
    }

    private void FixedUpdate()
    {
        if (IsChangingLane)
        {
            MoveToCorrectLane();
        }
    }

    private void MoveToCorrectLane()
    {
        Vector3 rbVelocity = rb.linearVelocity;
        
        rbVelocity.z = speed;

        if (IsChangingLane)
        {
            float direction = targetPositionX > rb.position.x ? 1f : -1f;
            float distanceToTarget = Mathf.Abs(targetPositionX - rb.position.x);

            float currentLaneSpeed = laneChangeSpeed;
            float laneStep = currentLaneSpeed * Time.fixedDeltaTime;
            
            if (distanceToTarget < laneStep)
            {
                currentLaneSpeed = distanceToTarget / Time.fixedDeltaTime;
            }

            rbVelocity.x = currentLaneSpeed * direction;
        }
        else
        {
            rbVelocity.x = 0f;
        }

        rb.linearVelocity = rbVelocity;

        if (Mathf.Abs(rb.position.x - targetPositionX) > 0.01f) return;
        
        rb.position = new Vector3(targetPositionX, rb.position.y, rb.position.z);
        currentLane = nextLane;
        rb.linearVelocity = new Vector3(0f, rbVelocity.y, rbVelocity.z);
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
        if (slowDownCoroutine != null) StopCoroutine(slowDownCoroutine);
        slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
    }

    public void PlayHitSound()
    {
        int index = Random.Range(0, hitSounds.Length);
        SoundManager.Instance.PlayOneShot(hitSounds[index]);
    }

    private IEnumerator SlowDownCoroutine()
    {
        float startSpeed = speed;
        float minSpeed = maxSpeed / speedSlowdownMultiplier;
        
        float startAnimSpeed = animator.speed;
        float minAnimSpeed = startAnimSpeed / speedSlowdownMultiplier;
        
        float elapsed = 0f;
        while (elapsed < slowdownDuration)
        {
            elapsed += Time.deltaTime;
            float duration = elapsed / slowdownDuration;
            
            speed = Mathf.Lerp(startSpeed, minSpeed, duration);
            animator.speed = Mathf.Lerp(startAnimSpeed, minAnimSpeed, duration);
            
            yield return null;
        }

        speed = minSpeed;
        animator.speed = minAnimSpeed;
        
        elapsed = 0f;
        while (elapsed < recoverDuration)
        {
            elapsed += Time.deltaTime;
            float duration = elapsed / recoverDuration;

            speed = Mathf.Lerp(minSpeed, maxSpeed, duration);
            animator.speed = Mathf.Lerp(minAnimSpeed, 1f, duration);
            
            yield return null;
        }

        speed = maxSpeed;
        animator.speed = 1f;

        yield return null;
    }

    private void CalculateSpeed()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.z = speed;
        rb.linearVelocity = velocity;
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
        phoneButtons.Disable();
        animator.SetTrigger(GameEvents.WIN);
        rb.linearVelocity = Vector3.zero;
        enabled = false;
    }

    public void OnCaught()
    {
        StopAllCoroutines();
        phoneButtons.Disable();
        speed = 0;
        CalculateSpeed();
        animator.speed = 1;
        animator.SetTrigger(GameEvents.Caught);
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