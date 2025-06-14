using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private int[] _costs;
    [SerializeField] private float[] _upgrades;
    [SerializeField] private UpgradeTypes _upgradeType;

    [SerializeField] private Material _grayMaterial;   
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _updateIcon;

    private Image[] _childImages => GetComponentsInChildren<Image>();
    private Button _button => GetComponentInChildren<Button>(); 
    private Animation _animation => GetComponentInChildren<Animation>();

    private int _currentUpgradeIndex;
    private bool _canAfford;
    private bool _isThisFirstAffordCheck = true;
    private bool _onUpgradeMax = false;

    private enum UpgradeTypes
    {
        Multiplier,
        GemSpawnRepeat
    }

    private void Awake()
    {
        _updateIcon.SetActive(true);
    }

    private void Start()
    {
        if (_upgradeType == UpgradeTypes.Multiplier)
            _currentUpgradeIndex = SaveManager.LoadCoinUpgradeIndex();
        else if (_upgradeType == UpgradeTypes.GemSpawnRepeat)
            _currentUpgradeIndex = SaveManager.LoadGemUpgradeIndex();

        ChangeUpgrade(_currentUpgradeIndex, false);

        _button.onClick.AddListener(ButtonClicked);        
    }

    private void Update()
    {
        TryChangeAfford();
    }

    private void TryChangeAfford()
    {
        if (_onUpgradeMax) return;
        if (UiController.Instance.Money >= _costs[_currentUpgradeIndex] && (!_canAfford || _isThisFirstAffordCheck))
        {
            _canAfford = true;
            foreach(var image in _childImages) 
                image.material = null;
        }            
        else if(UiController.Instance.Money < _costs[_currentUpgradeIndex] && (_canAfford || _isThisFirstAffordCheck))
        {
            _canAfford = false;
            foreach (var image in _childImages)
                image.material = _grayMaterial;
        }
        _isThisFirstAffordCheck = false;
    }

    private void ButtonClicked()
    {
        if (_canAfford && !_onUpgradeMax)
        {
            ChangeUpgrade(_currentUpgradeIndex+1, true);           

            _animation.Play();
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Item);
        }
        else
        {
            SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Close);
        }
    }

    private void ChangeUpgrade(int upgradeIndex, bool haveToPay)
    {
        if (upgradeIndex < _costs.Length)
            _priceText.SetText($"{_costs[upgradeIndex]}");
        else
        {
            _priceText.SetText("MAX");
            _onUpgradeMax = true;
            _updateIcon.SetActive(false);
        }

        _levelText.SetText($"LVL {upgradeIndex+1}");

        if (haveToPay)
        {
            UiController.Instance.RemoveMoney(_costs[_currentUpgradeIndex]);
            UiController.Instance.SaveMoney();
        }

        if (_upgradeType == UpgradeTypes.Multiplier)
        {
            GameData.BlockCost = _upgrades[upgradeIndex];
            SaveManager.SaveCoinUpgradeIndex(upgradeIndex);
        }
        else if (_upgradeType == UpgradeTypes.GemSpawnRepeat)
        {
            GameData.GemSpawnRepeat = _upgrades[upgradeIndex];
            SaveManager.SaveGemUpgradeIndex(upgradeIndex);
        }

        _currentUpgradeIndex = upgradeIndex;
    }
}
