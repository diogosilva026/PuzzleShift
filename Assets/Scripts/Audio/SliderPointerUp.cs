using UnityEngine;
using UnityEngine.EventSystems;

public class SliderPointerUp : MonoBehaviour, IPointerUpHandler
{
    public System.Action onPointerUp;
    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
    }
}
