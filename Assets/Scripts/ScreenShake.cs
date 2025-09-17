using UnityEngine;
using Unity.Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Shake(float force = 1f)
    {
        if (impulseSource != null)
            impulseSource.GenerateImpulse(force);
        else
            Debug.LogWarning("No CinemachineImpulseSource assigned!");
    }
}