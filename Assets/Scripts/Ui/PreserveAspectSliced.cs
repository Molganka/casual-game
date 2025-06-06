using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PreserveAspectSliced : MonoBehaviour
{
    [SerializeField] private float aspectRatio = 1f; // ��������, 1.0 ��� ���������� ������
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateAspect();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateAspect();
    }

    void UpdateAspect()
    {
        float width = rectTransform.rect.width;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width / aspectRatio);
    }
}
