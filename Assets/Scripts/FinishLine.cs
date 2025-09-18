using System;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public static event Action OnFinish;
    
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private AudioClip audioClip;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            OnFinish?.Invoke();
            objectToDisable.SetActive(false);
            SoundManager.Instance.PlayOneShot(audioClip);
            
            foreach (ParticleSystem particleSystem in particles)
            {
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
            }
        }
    }
}
