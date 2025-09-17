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
    
    
    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement) && playerMovement.IsSliding)
        {
            StartCoroutine(Destroy());
            return;
        }
        
        if (!other.TryGetComponent(out PlayerHealth playerHealth)) return;
        
        playerHealth.TakeAHit();
        StartCoroutine(Destroy());

        ScreenShake.instance.Shake();
        //VFXManager.instance.PlayHitEffect(transform.position);
    }

    private IEnumerator Destroy()
    {
        rb.AddForce(Vector3.forward * pushForwardForce + Vector3.up * pushUpwardForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
        
        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }
    
}
