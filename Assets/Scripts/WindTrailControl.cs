using System;
using UnityEngine;

public class WindTrailControl : MonoBehaviour
{
    public ParticleSystem windTrail;
    public Rigidbody windTrailRB;
    public float speedThreshold = 5f;

    private void Update()
    {
        if (Mathf.Abs(windTrailRB.linearVelocity.x) > speedThreshold)
        {
            if(!windTrail.isEmitting)
            {
                windTrail.Play();
            }
        }
        else
        {
            if (windTrail.isEmitting)
            {
                windTrail.Stop();
            }
        }
    }
}
