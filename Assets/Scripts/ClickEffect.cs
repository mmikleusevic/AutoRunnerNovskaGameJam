using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public ParticleSystem clickEffect;

    public void PlayClickEffect()
    {
        if (clickEffect != null)
            return;

        Vector3 screenPos = Input.mousePosition;

        screenPos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        clickEffect.transform.position = worldPos;
        clickEffect.Play();

        ParticleSystem ps = Instantiate(clickEffect, worldPos, Quaternion.identity);
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
