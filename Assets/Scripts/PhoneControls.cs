using System;
using UnityEngine;

public class PhoneButtons : MonoBehaviour
{
    [SerializeField] private GameObject phoneButtons;

    private void Start()
    {
#if UNITY_ANDROID
        phoneButtons.SetActive(true);
#endif
    }

    public void Disable()
    {
        phoneButtons.SetActive(false);
    }
}
