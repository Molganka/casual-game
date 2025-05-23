using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class OpenItemButton : MonoBehaviour
{
    [SerializeField] private int[] _costs;
    [SerializeField] private Sprite _accessibleSprite;
    [SerializeField] private Sprite _inaccessibleSprite;

    private Button _button => GetComponent<Button>();
    private Image _image => GetComponent<Image>();
    private Animation _animation => GetComponent<Animation>();
    private TextMeshProUGUI _costText => GetComponentInChildren<TextMeshProUGUI>();

    private int _currentCostIndex;
    private bool _onButtonAccessible = true;

    private void Awake()
    {
        _currentCostIndex = 0;
    }

    private void Start()
    {
        _button.onClick.AddListener(ButtonPressed);
        ChangeCost(_currentCostIndex);
    }

    private void Update()
    {
        UpdateButtonState();       
    }

    private void ButtonPressed()
    {
        if(_onButtonAccessible)
        {
            ItemsWindow.Instance.OpenRandomItem();
            UiController.Instance.RemoveMoney(_costs[_currentCostIndex]);
            ChangeCost(_currentCostIndex + 1);
            _animation.Play("ButtonClick");
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Open);
        }
        else
        {
            _animation.Play("ButtonNotClick");
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Close);
        }
    }

    private void UpdateButtonState()
    {
        if (!ItemsWindow.Instance.IsThereAccessibleItems())
        {
            _onButtonAccessible = false;
            _image.sprite = _inaccessibleSprite;
            _costText.SetText("MAX");
        }
        else if (UiController.Instance.Money >= _costs[_currentCostIndex])
        {
            _onButtonAccessible = true;
            _image.sprite = _accessibleSprite;
            return;
        }
        else
        {
            _onButtonAccessible = false;
            _image.sprite = _inaccessibleSprite;
        }      
    }

    private void ChangeCost(int index)
    {
        if(index < _costs.Length)
        {
            _costText.SetText($"{_costs[index]}");
            _currentCostIndex = index;
        }
    }
}
