using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private int[] _costsEveryTime;
    //[SerializeField] private 

    [SerializeField] private Material _grayMaterial;

    private Image[] _childImages => GetComponentsInChildren<Image>();
    private Button _button => GetComponentInChildren<Button>(); 
    private Animation _animation => GetComponentInChildren<Animation>();

    private int _currentCost;
    private bool _canAfford;
    private bool _isThisFirstAffordCheck = true;

    private void Awake()
    {
        _currentCost = _costsEveryTime[0];
    }

    private void Start()
    {
        _button.onClick.AddListener(ButtonClicked);
    }

    private void Update()
    {
        TryChangeAfford();
    }

    private void TryChangeAfford()
    {
        if (UiController.Instance.Money >= _currentCost && (!_canAfford || _isThisFirstAffordCheck))
        {
            _canAfford = true;
            foreach(var image in _childImages) 
                image.material = null;
        }            
        else if(UiController.Instance.Money < _currentCost && (_canAfford || _isThisFirstAffordCheck))
        {
            _canAfford = false;
            foreach (var image in _childImages)
                image.material = _grayMaterial;
        }
        _isThisFirstAffordCheck = false;
    }

    private void ButtonClicked()
    {
        if (_canAfford)
        {
            _animation.Play();
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Item);
        }
        else
        {
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Close);
        }
    }
}
