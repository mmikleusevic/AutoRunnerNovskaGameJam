using System;
using UnityEngine;

public class EnemyHitEffect : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.2f;

    public void TriggerHitEffect(Vector3 transformPosition)
    {
        if (hitEffectPrefab)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        
        ScreenShake.instance?.Shake(shakeDuration, shakeAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player"))
        {
            TriggerHitEffect(transform.position);

            Destroy(gameObject);
        }
    }
}
