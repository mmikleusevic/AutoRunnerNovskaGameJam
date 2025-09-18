using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float destroyAfter = 1f;
    [SerializeField] private float pushForwardForce = 100f;
    [SerializeField] private float pushUpwardForce = 10f;
    [SerializeField] private float torqueForce = 10f;
    
    private Collider obstacleCollider;
    private Rigidbody rb;
    
    private void Awake()
    {
        obstacleCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        
        if (playerMovement) playerMovement.PlayHitSound();
        
        if (playerMovement && playerMovement.IsSliding)
        {
            obstacleCollider.enabled = false;
            StartCoroutine(Destroy());
            return;
        }
        
        if (!other.TryGetComponent(out PlayerHealth playerHealth)) return;
        
        obstacleCollider.enabled = false;
        playerHealth.TakeAHit();
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        rb.AddForce(Vector3.forward * pushForwardForce + Vector3.up * pushUpwardForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
        
        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }
}
