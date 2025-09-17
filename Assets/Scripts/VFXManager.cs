using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;
    
    [SerializeField] private GameObject hitEffectPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHitEffect(Vector3 transformPosition, Quaternion rotation = default)
    {
        Instantiate(hitEffectPrefab, transformPosition, rotation);
    }
}
