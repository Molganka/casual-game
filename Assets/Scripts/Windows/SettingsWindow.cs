using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _gameSoundButton;
    [SerializeField] private Button _gameUIButton;
    [SerializeField] private Image _image1;
    [SerializeField] private Image _imageIcon1;
    [SerializeField] private Image _image2;
    [SerializeField] private Image _imageIcon2;
    [SerializeField] private Color _color1;
    [SerializeField] private Color _color2;
    [SerializeField] private Color _offColor;

    private void Awake()
    {
        _gameSoundButton.onClick.AddListener(ChangeGameSoundCondition);
        _gameUIButton.onClick.AddListener(ChangeUISoundCondition);
    }

    private void Start()
    {
        GameData.Sensitivity = _slider.value;
        GameData.OnGameSound = true;
        GameData.OnUISound = true;

        _slider.onValueChanged.AddListener((v) =>
        {
            GameData.Sensitivity = v;
        });       
    }

    private void ChangeGameSoundCondition()
    {
        if(_image1.color == _offColor)
        {
            _image1.color = _color1;
            _imageIcon1.color = _color1;
            GameData.OnGameSound = true;
        }
        else
        {
            _image1.color = _offColor;
            _imageIcon1.color = _offColor;
            GameData.OnGameSound = false;
        }
    }

    private void ChangeUISoundCondition()
    {
        if (_image2.color == _offColor)
        {
            _image2.color = _color2;
            _imageIcon2.color = _color2;
            GameData.OnUISound = true;
        }
        else
        {
            _image2.color = _offColor;
            _imageIcon2.color = _offColor;
            GameData.OnUISound = false;
        }
    }
}
