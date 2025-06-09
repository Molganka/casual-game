using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] SoundUI.AudioClipsEnum _audioClip;
    private Animation _animation;
    [SerializeField] private Button _button;

    private void Start()
    {
        if (_button == null) _button = GetComponent<Button>();

        _button.onClick.AddListener(() => SoundUI.Instance.PlaySound(_audioClip));
        if(TryGetComponent<Animation>(out Animation animation))
        {
            _animation = animation;
            _button.onClick.AddListener(PlayAnimation);
        }          
    }

    private void PlayAnimation()
    {
        if (_animation.GetClip("ButtonClick") != null)
            _animation.Play("ButtonClick");
        else
            _animation.Play("ButtonClick2");
    }
}
