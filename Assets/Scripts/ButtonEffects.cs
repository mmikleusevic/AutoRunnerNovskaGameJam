using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector3 normalScale = new Vector3(2f, 2f, 2f);
    public Vector3 hoverScale = new Vector3(2.2f, 2.2f, 2.2f);
    public Vector3 clickScale = new Vector3(1.9f, 1.9f, 2.2f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = normalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = clickScale;
        Invoke("ResetScale", 0.1f);
    }

    public void ResetScale()
    {
        transform.localScale = normalScale;
    }
}
