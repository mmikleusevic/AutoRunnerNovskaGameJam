using System;
using TMPro;
using UnityEngine;

public class OptionsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI controlsText;

    [SerializeField] private string pcText;
    [SerializeField] private string mobileText;
    
    private void Start()
    {
        #if UNITY_ANDROID
            controlsText.text = mobileText;
        #else
            controlsText.text = pcText;
        #endif
    }
}
