using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleter : MonoBehaviour
{   
    public static LevelCompleter Instance;

    [SerializeField] private Animation _animation;
    [SerializeField] private Button _button1;
    [SerializeField] private Button _button2;
    [SerializeField] private CoinCollectEffect _coinCollectEffect;
    [SerializeField] private GameObject _totalMoneyObject;
    [SerializeField] private TextMeshProUGUI _basicMoneyText;
    [SerializeField] private TextMeshProUGUI _bonusMoneyText;
    [SerializeField] private TextMeshProUGUI _totalMoneyText;

    [SerializeField] private float _timeToCountTotalMoney;
    [SerializeField] private float _duration = 2f;

    private int _totalCoins = 0;
    private int _doubleCoins = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        UiController.OnGameStarted += EnableButtons;
    }

    public void StartLevelComplete()
    {
        Invoke(nameof(CountTotalMoney), _timeToCountTotalMoney);
    }

    private void CountTotalMoney()
    {
        if(CollisionHandler.FinishType == CollisionHandler.FinishTypes.Basic)
        {
            _totalCoins = Convert.ToInt32(BasicFinishWindow.Money);
        }
        else if(CollisionHandler.FinishType == CollisionHandler.FinishTypes.Bonus)
        {
            _totalCoins = BonusFinishWindow.Instance.Gems * GameData.GemCost;
        }

        _doubleCoins = _totalCoins * 2;
        Debug.Log("Double Money: " + _doubleCoins);
        _basicMoneyText.SetText(_totalCoins.ToString());
        _bonusMoneyText.SetText(_doubleCoins.ToString());

        StartCounting(_totalCoins);
    }

    private void StartCounting(int targetNumber)
    {
        int currentNumber = 0;
        DOTween.To(() => currentNumber, x => currentNumber = x, targetNumber, _duration)
               .OnUpdate(() => 
               {
                   _totalMoneyText.text = currentNumber.ToString();
               })
               .OnComplete(() =>
               {
                   Debug.Log("Counting completed!");
                   // Запускаем следующую анимацию
                   PlayNextAnimation();
               })
               .SetEase(Ease.OutQuad); // Устанавливаем замедление в конце
    }
    
    private void PlayNextAnimation()
    {
        _animation.Play("ShowLevelCompleteWindow2");
    }

    public void CollectCoins()
    {
        Debug.Log("COLLECT BASIC");
        DisableButtons();
        _totalMoneyObject.SetActive(false);
        _coinCollectEffect.StartShowCoins(_totalCoins);       
    }

    public void CollectDoubleCoins()
    {
        Debug.Log("COLLECT DOUBLE");
        DisableButtons();
        CrazyGamesSDK.ShowRewardedAd();      
    }

    public void CollectDoubleCoinsAfterAd()
    {
        _totalMoneyObject.SetActive(false);
        _coinCollectEffect.StartShowCoins(_doubleCoins);
    }

    private void DisableButtons()
    {
        _button1.enabled = false;
        _button2.enabled = false;
    }

    private void EnableButtons()
    {
        _button1.enabled = true;
        _button2.enabled = true;
    }
}
