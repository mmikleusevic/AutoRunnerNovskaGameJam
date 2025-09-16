using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public ParticleSystem clickEffect;

    public void PlayClickEffect()
    {
        if (clickEffect != null)
            return;
        
        clickEffect.transform.position = transform.position;
        
        clickEffect.Play();
    }
}
