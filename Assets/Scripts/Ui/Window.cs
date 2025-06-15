using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    private CanvasGroup _canvasScaler => GetComponent<CanvasGroup>();

    private void Start()
    {
        _canvasScaler.alpha = 0f;
    }

    public void Show()
    {
        _canvasScaler.alpha = 1f;
        _canvasScaler.interactable = true;
        _canvasScaler.blocksRaycasts = true;
        if(_animation != null)
            _animation.Play();
    }

    public void Hide()
    {
        _canvasScaler.alpha = 0f;
        _canvasScaler.interactable = false;
        _canvasScaler.blocksRaycasts = false;
    }
}
