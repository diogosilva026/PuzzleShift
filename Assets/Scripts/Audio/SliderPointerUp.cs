using UnityEngine;
using UnityEngine.EventSystems;

// This is a helper component for UI sliders that triggers an event when the pointer is released
public class SliderPointerUp : MonoBehaviour, IPointerUpHandler
{
    public System.Action onPointerUp;
    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
    }
}
