using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class OpenItemButton : MonoBehaviour
{
    [SerializeField] private int[] _openCostEveryTime; 
    [SerializeField] private Sprite _buttonAvailableSprite;
    [SerializeField] private Sprite _buttonNotAvailableSprite;

    private Button _button => GetComponent<Button>();
    private Image _image => GetComponent<Image>();
    private Animation _animation => GetComponent<Animation>();
    private TextMeshProUGUI _costText => GetComponentInChildren<TextMeshProUGUI>();

    private int _currentCostIndex;
    private bool _canAfford;

    private void Awake()
    {
        _currentCostIndex = 0;
    }

    private void Start()
    {
        _button.onClick.AddListener(ButtonPressed);
        ChangeCostText(_openCostEveryTime[_currentCostIndex]);
    }

    private void Update()
    {
        TryChangeButtonState();
    }

    private void ButtonPressed()
    {
        if(_canAfford)
        {
            ItemsWindow.Instance.OpenRandomItem();
            UiController.Instance.RemoveMoney(_openCostEveryTime[_currentCostIndex]);
            _animation.Play("ButtonClick");
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Open);
        }
        else
        {
            _animation.Play("ButtonNotClick");
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Close);
        }
    }

    private void TryChangeButtonState()
    {
        if(UiController.Instance.Money >= _openCostEveryTime[_currentCostIndex])
        {
            ChangeButtonState(true);
            _image.sprite = _buttonAvailableSprite;
        }
        else
        {
            ChangeButtonState(false);
            _image.sprite = _buttonNotAvailableSprite;
        }
    }

    private void ChangeButtonState(bool state)
    {
        _canAfford = state;
    }

    private void ChangeCostText(int cost)
    {
        _costText.SetText($"{cost}");
    }
}
