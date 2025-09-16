using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Material glowMaterial;
    public Color baseEmission = Color.black;
    public Color hoverEmission = Color.white * 5f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        SetEmission(baseEmission);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetEmission(hoverEmission);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetEmission(baseEmission);
    }

    void SetEmission(Color color)
    {
        if (glowMaterial != null)
        {
            glowMaterial.SetColor("EmissionColor", color);
            DynamicGI.SetEmissive(rend, color);
        }
    }
}
