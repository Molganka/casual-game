using DG.Tweening;
using System;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleter : MonoBehaviour
{   
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

    private int _coins = 0;

    private int _totalCions = 0;
    private int _doubleCoins = 0;

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
        if(CollisionHandler.FinishType == 1)
        {
            _totalCions = _coins;
            Debug.Log("Total type 1: " + _totalCions);
        }
        else if(CollisionHandler.FinishType == 2)
        {
            _totalCions = Convert.ToInt32(PlayerAppearance.FinalScore * CollisionHandler.Multiplier);
        }

        _doubleCoins = _totalCions * 2;
        Debug.Log("Double Money: " + _doubleCoins);
        _basicMoneyText.SetText(_totalCions.ToString());
        _bonusMoneyText.SetText(_doubleCoins.ToString());

        StartCounting(_totalCions);
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
        _totalMoneyObject.SetActive(false);
        _coinCollectEffect.StartShowCoins(_totalCions);
        DisableButtons();
    }

    public void CollectDoubleCoins()
    {
        Debug.Log("COLLECT DOUBLE");
        _totalMoneyObject.SetActive(false);
        _coinCollectEffect.StartShowCoins(_doubleCoins);
        DisableButtons();
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

    public void AddCoins(int value = 5)
    {
        
        _coins += value;
        Debug.Log("AddCoins coins: " + _coins + " + " + value);
    }
}
