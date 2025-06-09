using UnityEngine;
using UnityEngine.EventSystems;

public class TouchDetector : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        UiController.AnyPlaceTouched();
    }
}
